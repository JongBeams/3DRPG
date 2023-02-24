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
    }

    // Update is called once per frame
    void Update()
    {
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
        //Debug.Log("Check");

        // 범위 탐색
        int m_nMask = m_nTargetLayer[0] | m_nTargetLayer[1];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;

        //공격 딜레이 타임
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //적 아군 확인 체크
        m_bCheck[1] = false;
        //자신 체력의 회복 여부 확인
        bool mineCheck = false;
        //타겟지정
        GameObject Target = null;


        if (GameManager.Instance.m_nScreenIdx !=2)
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
            //탐색 콜라이더 선별
            while (count < hitcol.Length)
            {
                if (hitcol[count].gameObject.layer == m_nTargetLayer[1])// 플레이어 또는 동료일때
                {
                    Char_Base cs = hitcol[count].gameObject.GetComponent<Char_Base>();
                    if (cs.m_nPlayerHP <= cs.CharStatus.HP / 2 && cs.CS != CharState.Death)//체력이 반이하일때
                    {
                        if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana() && m_bSkillOn[1])// 스킬 사용가능 여부 확인
                        {
                            objTarget = hitcol[count].gameObject;// 타겟지정
                            Target = hitcol[count].gameObject;
                            m_bCheck[1] = false;//아군 체크 (적이 아니다)
                            break;
                        }

                    }
                }
                else //몬스터일때
                {
                    objTarget = hitcol[count].gameObject;
                    Target = hitcol[count].gameObject;
                    //PartnerTarget = hitcol[count].gameObject;
                    m_bCheck[1] = true;// 적 체크
                                       //setEnemy = true;
                }

                count++;

            }

            // 자신의 체력 상태에 때른 타겟 변화
            if (m_nPlayerHP <= CharStatus.HP / 2 && m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana()) // 자신이 회복이 필요할때
            {
                objTarget = this.gameObject;// 타겟을 자신으로

                if (m_bCheck[1])// 회복할 아군캐릭터는 없을때
                {
                    mineCheck = true; //회복할 아군없음 구분
                    m_bCheck[1] = false;// 회복으로 조건 진행
                }

            }
        }


        if (Target != null)
        {
            //타겟과의 거리
            Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            float dis = Vector3.Distance(transform.position, vecEnemyLookingPoint);

            // 행동 설정
            if (objTarget != null)// 타겟이 존재할때
            {
                if (m_bCheck[1])//TargetEnemyCheck // 적일때
                {
                    if (dis > 20f)//거리 20 보다 멀때
                    {
                        m_bCheck[0] = false;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else if (dis < 15f) //거리 15보다 가까울때
                    {
                        m_bCheck[0] = true;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else //적정거리
                    {
                        m_nActionIdx = 0;
                        SetCharStatus(CharState.Action);
                    }
                }
                else //아군일때
                {
                    if (dis > 15f) //거리 15보다 멀때(힐 거리가 안될 때)
                    {
                        m_bCheck[0] = false;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else// 스킬 사거리 일때
                    {
                        if (objTarget == gameObject)// 타겟이 자신일때
                        {
                            if (mineCheck) // 치료해야할 동료가 없을때
                            {
                                m_nActionIdx = 1;
                                SetCharStatus(CharState.Action);
                            }
                            else //치료해야할 동료가 있을때
                            {
                                if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[2]].getSkillUsingMana() && m_bSkillOn[2])//전체힐 사용 조건
                                {
                                    m_nActionIdx = 2;
                                    SetCharStatus(CharState.Action);
                                }
                                else
                                {
                                    m_nActionIdx = 1;
                                    SetCharStatus(CharState.Action);
                                }
                            }
                        }
                        else
                        {
                            if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana() && m_bSkillOn[1])//전체힐 사용 조건
                            {
                                m_nActionIdx = 1;
                                SetCharStatus(CharState.Action);
                            }
                            else
                            {
                                SetCharStatus(CharState.Idle);
                            }

                        }
                    }
                }
            }



        }
    }

    void MoveAlgorithm()
    {


        // 타겟과의 거리
        Vector3 vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, gameObject.transform.position.y, objTarget.transform.position.z);
        float dis = Vector3.Distance(gameObject.transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // 타겟 체크
        if (m_bCheck[1])//타겟이 적일 때
        {
            if (dis > 20f) //거리가 20보다 멀때
            {
                agent.SetDestination(vecEnemyLookingPoint);
                return;
            }
            if (dis < 15)//거리가 15보다 가까울때
            {

                vecMovePoint = (this.transform.position - (this.transform.forward * 12));
                agent.SetDestination(vecMovePoint);
                return;
            }
            if (dis >= 15 && dis <= 20)
            {
                agent.SetDestination(this.transform.position);
                agent.velocity = Vector3.zero;
                SetAlgorithm();
                //AlgorithmManager.SetAlgorithm(_id,_PD);
                return;
            }
        }
        else // 타겟이 아군일때
        {
            if (dis > 15) //거리가 15보다 멀때
            {
                transform.LookAt(vecEnemyLookingPoint);

                agent.SetDestination(vecEnemyLookingPoint);

                return;
            }
            else //적정 거리
            {
                SetAlgorithm();
                return;
            }
        }


        if (agent.remainingDistance <= 0.2f)
        {
            agent.velocity = Vector3.zero;
            SetCharStatus(CharState.Idle);
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

        int m_nMask = 0;
        //m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        m_nMask = m_nTargetLayer[1];
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
