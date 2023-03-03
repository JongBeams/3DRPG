using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Priest_Char : Char_Base
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

    // Update is called once per frame
    void Update()
    {
        //Invoke("UpdateCharStatus", 0.1f);
        UpdateCharStatus();
        Recovery();
        SkillCooTimer();
    }


    #region 스테이터스

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


    #region 상태

    public Action skillAction = null;

    public async Task Skill(float _delayTime)
    {
        await Task.Delay(TimeSpan.FromSeconds(_delayTime));
        skillAction?.Invoke();
    }

    void SetAction(int _idx)
    {

        if (m_nPlayerMP<DBManager.SkillData[CharStatus.SID[_idx]].SM)
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
                skillAction = TargetHealing;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = AllHealing;
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



    public override void SetCharStatus(CharState _CS) // 한번 실행
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


    public override void UpdateCharStatus()// 지속 실행
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


    #region 알고리즘

    void SetAlgorithm()
    {

        int count = 0;
        //적 아군 확인 체크
        m_bCheck[0] = false;
        //자신 체력의 회복 여부 확인
        bool mineCheck = false;
        //
        m_bCheck[1] = false;


        List<GameObject> targetobj = new List<GameObject>();
        List<GameObject> allyobj = new List<GameObject>();
        if (CharStatus.TYP == LayerMask.NameToLayer("Enemy"))
        {
            targetobj.Add(GameManager.Instance.objPlayer);
            targetobj.Add(InGameSceneManager.Instance.PInfo[0].objPartner);
            targetobj.Add(InGameSceneManager.Instance.PInfo[1].objPartner);
            allyobj.Add(InGameSceneManager.Instance.objEnemy);
        }
        if (CharStatus.TYP == LayerMask.NameToLayer("Player") || CharStatus.TYP == LayerMask.NameToLayer("Partner"))
        {
            targetobj.Add(InGameSceneManager.Instance.objEnemy);
            allyobj.Add(GameManager.Instance.objPlayer);
            allyobj.Add(InGameSceneManager.Instance.PInfo[0].objPartner);
            allyobj.Add(InGameSceneManager.Instance.PInfo[1].objPartner);
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
            if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].SM)
            {
                for (int i = 0; i < allyobj.Count; i++)
                {
                    Char_Base Allychar = allyobj[i].GetComponent<Char_Base>();
                    if (Allychar.m_nPlayerHP <= Allychar.CharStatus.HP / 2)
                    {
                        objTarget = allyobj[i];
                        m_bCheck[0] = true;
                        count++;
                    }
                    if (allyobj[i]==this.gameObject)
                    {
                        mineCheck = true;
                    }
                        
                }
            }
        }

        if (!m_bCheck[0])
        {
            objTarget = InGameSceneManager.Instance.objEnemy;
        }

        

        if (objTarget!= null)
        {
            vecMovePoint = objTarget.transform.position;

            if (m_bCheck[0])// 아군 타깃
            {
                if (count > 1)
                {
                    m_nActionIdx = 2;
                    SetCharStatus(CharState.Action);
                    return;
                }
                if (count == 1)
                {
                    m_nActionIdx = 1;
                    SetCharStatus(CharState.Action);
                    return;
                }
            }

            if(!m_bCheck[0])// 적군 타깃
            {
                float dis = Vector3.Distance(transform.position, objTarget.transform.position);

                if (dis > 20f)//거리 20 보다 멀때
                {
                    agent.SetDestination(PlayerLookingPoint());
                    SetCharStatus(CharState.Move);
                    return;
                }

                if (dis < 15f) //거리 15보다 가까울때
                {
                    m_bCheck[1] = true;
                    transform.LookAt(PlayerLookingPoint());
                    vecMovePoint = (this.transform.position - (this.transform.forward * 10));
                    agent.SetDestination(vecMovePoint);
                    SetCharStatus(CharState.Move);
                    return;
                }

                if(dis >= 15f&& dis <= 20f) //적정거리
                {
                    m_nActionIdx = 0;
                    SetCharStatus(CharState.Action);
                    return;
                }

            }
        }


    }

    void MoveAlgorithm()
    {

        float dis = Vector3.Distance(transform.position, vecMovePoint);
        float dis2 = Vector3.Distance(transform.position, objTarget.transform.position);

        if (m_bCheck[1] && dis < 0.5f)
        {
            m_bCheck[1] = false;
            SetCharStatus(CharState.Idle);
            return;
        }
        if (!m_bCheck[1] && dis2 <= 20)
        {
            Debug.Log("Check");
            m_nActionIdx = 0;
            SetCharStatus(CharState.Action);
            return;
        }

    }

    #endregion


    #region 스킬


    void SingleTargetBullet()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];

        if (objTarget != null || objTarget.activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<Bullet>().Setting(objTarget, this, CharStatus.SID[0]);
        }


    }



    void TargetHealing()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];


        objTarget.GetComponent<Char_Base>().HealingHP((int)(CharStatus.MP * SkillDB.getSkillCeofficientPer1()));


    }

    void AllHealing()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];


        //objTarget.GetComponent<Char_Base>().HealingHP((int)(CharStatus.MP * SkillDB.getSkillCeofficientPer1()));

        int m_nMask = m_nMask = m_nTargetLayer[1];
        //m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));

        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Base>().HealingHP((int)(CharStatus.MP * SkillDB.getSkillCeofficientPer1()));
            count++;

        }


    }


    #endregion


    #region 연산



    #endregion


}
