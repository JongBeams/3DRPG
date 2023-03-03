using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Wizard_Char : Char_Base
{
    // Start is called before the first frame update
    void Start()
    {
        SetComponents();
        SetCheck(2);
        m_nPlayerHP = CharStatus.HP;
        m_nPlayerMP = CharStatus.MP;
        agent.speed = CharStatus.SPD;
    }

    private void FixedUpdate()
    {
        UpdateCharStatus();
        Recovery();
        SkillCooTimer();
    }

    #region �������ͽ�

    public override void Recovery()
    {
        if (m_nPlayerMP < CharStatus.MP)
        {
            if (m_fPlayerMPRecoveryTimer <= 0)
            {
                m_nPlayerMP += (int)CharStatus.MPRP;
                m_fPlayerMPRecoveryTimer = 5;
            }
            else
            {
                m_fPlayerMPRecoveryTimer -= Time.deltaTime;
            }
        }
    }

    protected override void SetSkillCoolTime()
    {
        m_bSkillOn[m_nActionIdx] = false;
        m_fSkillCoolTimer[m_nActionIdx] = DBManager.SkillData[CharStatus.SID[m_nActionIdx]].SCT;
    }


    #endregion


    #region ����

    public Action skillAction = null;

    public async Task Skill(float _delayTime)
    {
        await Task.Delay(TimeSpan.FromSeconds(_delayTime));
        skillAction?.Invoke();
    }

    void SetAction(int _idx)
    {

        if (m_nPlayerMP < DBManager.SkillData[CharStatus.SID[_idx]].SM)
        {
            skillAction = SingleTargetBullet;
            animator.SetBool("Attack", true);
            strActionAniName = "Attack";
            m_nActionIdx = 0;
            return;
        }

        switch ((ActionState)_idx)
        {
            case ActionState.Attack:
                skillAction = SingleTargetBullet;
                animator.SetBool("Attack", true);
                strActionAniName = "Attack";
                break;
            case ActionState.Skill1:
                skillAction = FireBall;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = FireBreath;
                animator.SetBool("Skill2", true);
                strActionAniName = "Skill2";
                break;
        }

    }

    void AniBoolOffAll()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Skill1", false);
        animator.SetBool("Skill2", false);
        animator.SetBool("Hit", false);
    }



    public override void SetCharStatus(CharState _CS) // �ѹ� ����
    {
        switch (_CS)
        {
            case CharState.Idle:
                objTarget = null;
                agent.velocity = Vector3.zero;
                agent.SetDestination(this.transform.position);
                AniBoolOffAll();
                break;
            case CharState.Move:
                animator.SetBool("Move", true);
                agent.SetDestination(PlayerLookingPoint());
                break;
            case CharState.Action:
                agent.velocity = Vector3.zero;
                agent.SetDestination(this.transform.position);
                SetAction(m_nActionIdx);
                transform.LookAt(PlayerLookingPoint());
                SetSkillCoolTime();
                skillAction?.Invoke();
                break;
            case CharState.Hit:
                AniBoolOffAll();
                animator.SetBool("Hit", true);
                break;
            case CharState.Death:
                animator.SetBool("Death", true);
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().isTrigger = true;
                //Destroy(this.gameObject, 5f);
                break;
            case CharState.Stay:
                AniBoolOffAll();
                break;

        }

        CS = _CS;
    }


    public override void UpdateCharStatus()// ���� ����
    {
        switch (CS)
        {
            case CharState.Idle:
                if (CheckEndAni("Idle"))
                {
                    SetAlgorithm();
                }
                break;
            case CharState.Move:
                //Invoke("MoveAlgorithm", 0.1f);
                MoveAlgorithm();
                break;
            case CharState.Action:
                if (m_bSkillUsing[m_nActionIdx])
                {
                    skillAction?.Invoke();
                }
                else
                {
                    if (CheckEndAni(strActionAniName))
                    {
                        UseMana(DBManager.SkillData[CharStatus.SID[m_nActionIdx]].SM);
                        SetCharStatus(CharState.Idle);
                    }
                }
                break;
            case CharState.Hit:
                if (CheckEndAni("Hit"))
                {
                    SetCharStatus(CharState.Idle);
                }
                break;
            case CharState.Death:

                break;
            case CharState.Stay:
                break;

        }
    }


    #endregion


    #region �˰���

    void SetAlgorithm()
    {

        //
        m_bCheck[1] = false;


        List<GameObject> targetobj = new List<GameObject>();
        if (CharStatus.TYP == LayerMask.NameToLayer("Enemy"))
        {
            targetobj.Add(GameManager.Instance.objPlayer);
            targetobj.Add(InGameSceneManager.Instance.PInfo[0].objPartner);
            targetobj.Add(InGameSceneManager.Instance.PInfo[1].objPartner);
        }

        if (CharStatus.TYP == LayerMask.NameToLayer("Player") || CharStatus.TYP == LayerMask.NameToLayer("Partner"))
        {
            targetobj.Add(InGameSceneManager.Instance.objEnemy);
        }

        for (int i = 0; i < targetobj.Count; i++)
        {
            if (targetobj[i].GetComponent<Char_Base>().CS == CharState.Death)
            {
                targetobj.RemoveAt(i);
            }
        }

        if (targetobj.Count == 0)
        {
            SetCharStatus(CharState.Stay);
            return;
        }
        if (GameManager.Instance.m_nScreenIdx != 2)
        {
            SetCharStatus(CharState.Stay);
            return;
        }
        if (GameManager.Instance.m_nScreenIdx == 2 && InGameSceneManager.Instance.m_bGameEnd)
        {
            SetCharStatus(CharState.Stay);
            return;
        }


        if (!m_bTaunt)
        {
            int TargetRan = UnityEngine.Random.Range(0, targetobj.Count);
            objTarget = targetobj[TargetRan].gameObject;
        }


        if (objTarget != null)
        {
            vecMovePoint = objTarget.transform.position;


            float dis = Vector3.Distance(transform.position, objTarget.transform.position);


            int ran = UnityEngine.Random.Range(0, 3);
            if (ran == 0)
            {
                m_nActionIdx = 0;
            }
            if (ran == 1)
            {
                m_nActionIdx = 1;
            }
            if (ran == 2)
            {
                m_nActionIdx = 2;
            }


            if (dis > 15f)//�Ÿ� 20 ���� �ֶ�
            {
                agent.SetDestination(vecMovePoint);
                SetCharStatus(CharState.Move);
                return;
            }

            if (dis < 10f) //�Ÿ� 15���� ����ﶧ
            {
                m_bCheck[1] = true;
                transform.LookAt(PlayerLookingPoint());
                vecMovePoint = (this.transform.position - (this.transform.forward * 7.5f));
                agent.SetDestination(vecMovePoint);
                SetCharStatus(CharState.Move);
                return;
            }

            if (dis >= 10f && dis <= 15f) //�����Ÿ�
            {
                SetCharStatus(CharState.Action);
                return;
            }


        }

    }

    void MoveAlgorithm()
    {

        float dis = Vector3.Distance(transform.position, vecMovePoint);
        float dis2 = Vector3.Distance(transform.position, objTarget.transform.position);

        if (m_bCheck[1] && dis < 0.5f)
        {

            Debug.Log("Check1");
            m_bCheck[1] = false;
            SetCharStatus(CharState.Idle);
            return;

        }

        if (!m_bCheck[1]&&dis2 <= 15f)
        {
            Debug.Log("Check1");
            SetCharStatus(CharState.Action);
            return;
        }


    }

    #endregion


    #region ��ų




    void SingleTargetBullet()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];

        if (objTarget != null || objTarget.activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<Bullet>().Setting(objTarget, this, CharStatus.SID[0]);
        }


    }

    void FireBall()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];

        if (objTarget != null || objTarget.activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<Bullet>().Setting(objTarget, this, CharStatus.SID[1]);
        }


    }

    void FireBreath()
    {

        //��ų ����
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];



        if (objTarget != null || objTarget.activeSelf == false)
        {

            GameObject FireBreathEffect = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity, AttackPos);
            FireBreathEffect.transform.parent = AttackPos;
            FireBreathEffect.transform.localPosition = Vector3.zero;
            FireBreathEffect.transform.localRotation = Quaternion.identity;
            FireBreathEffect.transform.parent = null;
            FireBreathEffect.GetComponent<ParticleSystem>().Play();
            FireBreathEffect.GetComponent<FireBreath>().Setting(this, CharStatus.SID[2]);

        }

    }


    #endregion


    #region ����

    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    #endregion


}
