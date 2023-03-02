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
    }

    // Update is called once per frame
    void Update()
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
        //Debug.Log("Check");

        // ���� Ž��
        int m_nMask = m_nTargetLayer[0] | m_nTargetLayer[1];

        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;

        //���� ������ Ÿ��
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //Ÿ������
        GameObject Target = null;


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

        if (hitcol != null)
        {
            if (!m_bTaunt)
            {
                objTarget = hitcol[0].gameObject;
                Target = hitcol[0].gameObject;
                vecMovePoint = objTarget.transform.position;
            }


            //Ÿ�ٰ��� �Ÿ�
            Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, gameObject.transform.position.y, Target.transform.position.z);
            float dis = Vector3.Distance(gameObject.transform.position, vecEnemyLookingPoint);

            // �ൿ ����
            if (objTarget != null)// Ÿ���� �����Ҷ�
            {

                if (dis > 20f)//�Ÿ� 20 ���� �ֶ�
                {
                    SetCharStatus(CharState.Move);
                    return;
                }
                if (dis < 15f) //�Ÿ� 15���� ����ﶧ
                {
                    SetCharStatus(CharState.Move);
                    return;
                }
                if (dis >= 15 && dis <= 20) //�����Ÿ�
                {
                    int ran = UnityEngine.Random.Range(0, 3);
                    if (ran == 2)
                    {
                        if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[2]].getSkillUsingMana() && m_bSkillOn[2])
                        {
                            m_nActionIdx = 2;

                        }
                        else
                        {
                            m_nActionIdx = 0;
                        }
                    }
                    else if (ran == 1)
                    {
                        if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana() && m_bSkillOn[1])
                        {
                            m_nActionIdx = 1;
                        }
                        else
                        {
                            m_nActionIdx = 0;
                        }
                    }
                    else
                    {
                        m_nActionIdx = 0;
                    }
                    SetCharStatus(CharState.Action);
                    return;

                }
            }


        }
    }

    void MoveAlgorithm()
    {


        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, transform.position.y, objTarget.transform.position.z);
        float dis = Vector3.Distance(transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // Ÿ�� üũ

        if (dis > 20f) //�Ÿ��� 20���� �ֶ�
        {
            agent.SetDestination(vecEnemyLookingPoint);
            return;
        }
        if (dis < 15)//�Ÿ��� 15���� ����ﶧ
        {

            vecMovePoint = (this.transform.position - (this.transform.forward * 12));
            agent.SetDestination(vecMovePoint);

            return;
        }
        if (dis >= 15 && dis <= 20)
        {
            SetAlgorithm();
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
