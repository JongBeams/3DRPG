using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Thief_Char : Char_Base
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

        if (m_nPlayerMP < DBManager.SkillData[CharStatus.SID[_idx]].SM)
        {
            Debug.Log(m_nPlayerMP + " "+ DBManager.SkillData[CharStatus.SID[_idx]].SM);
            skillAction = MeleeAttack_BackAttackOn;
            animator.SetBool("Attack", true);
            strActionAniName = "Attack";
            m_nActionIdx = 0;
            return;
        }

        switch ((ActionState)_idx)
        {
            case ActionState.Attack:
                skillAction = MeleeAttack_BackAttackOn;
                animator.SetBool("Attack", true);
                strActionAniName = "Attack";
                break;
            case ActionState.Skill1:
                skillAction = FlipOver;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = BackStep;
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

        //공격 딜레이 타임
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //타겟지정

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
            objTarget = InGameSceneManager.Instance.objEnemy;
            vecMovePoint = objTarget.transform.position;
        }

        if (objTarget != null)
        {
            m_nActionIdx = 0;
            if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[2]].SM && m_bCheck[0] && m_bSkillOn[2])
            {
                m_nActionIdx = 2;
            }
            if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].SM && !m_bCheck[0] && m_bSkillOn[1])
            {
                m_nActionIdx = 1;

            }

            float dis = Vector3.Distance(transform.position, objTarget.transform.position);


            if (dis > 3.5f)
            {
                //Debug.Log("Check");
                agent.SetDestination(vecMovePoint);
                SetCharStatus(CharState.Move);
                return;
            }
            if (dis <= 3.5f)
            {
                SetCharStatus(CharState.Action);
                return;
            }
        }
    }

    void MoveAlgorithm()
    {
        float dis = Vector3.Distance(transform.position, objTarget.transform.position);

        if (dis <= 3.5f)
        {
            SetCharStatus(CharState.Action);
            return;
        }

    }

    #endregion


    #region 스킬
    bool m_bAttackCheck;



    void AttackCheck()
    {
        m_bAttackCheck = true;
    }


    void MeleeAttack_BackAttackOn()
    {
        
        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];

        //공격 범위
        int m_nMask = m_nTargetLayer[0];

        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(transform.position, PlayerLookingPoint()), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= SkillDB.getSkillRange1())
            {
                
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                m_bCheck[0]=true;// 백어택 판정으로 사용
                
            }
            else
            {
                
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
                m_bCheck[0] = false;
            }

        }




    }


    void FlipOver()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];

        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
                                                                                                            //int count = 0;

        if (hitcol[0] != null)
        {
            transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4.5f;
        }

        // 타깃 바라보기
        transform.LookAt(new Vector3(hitcol[0].transform.position.x, transform.position.y, hitcol[0].transform.position.z));


    }


    void BackStep()
    {
        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];





        //공격 범위
        int m_nMask = m_nTargetLayer[0];

        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(transform.position, PlayerLookingPoint()), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= SkillDB.getSkillRange1())
            {
                
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                m_bCheck[0] = true;
                //Debug.Log("BackHit");
            }
            else
            {
                
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
                m_bCheck[0] = false;
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }




    }


    #endregion


    #region 연산

    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    #endregion


}
