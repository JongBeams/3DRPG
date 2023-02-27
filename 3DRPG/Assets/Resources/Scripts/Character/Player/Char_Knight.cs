using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Char_Knight : Char_Base
{
    // Start is called before the first frame update
    void Start()
    {
        SetComponents();
        SetCheck(2);
        m_nPlayerHP = CharStatus.HP;
        m_nPlayerMP = CharStatus.MP;
        m_nIdentityPoint = CharStatus.ISP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharStatus();
        Recovery();
        SkillCooTimer();

        Debug.DrawLine(this.transform.position,vecMovePoint);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(vecMovePoint, 0.5f);
    }


    #region 스테이터스
    float m_nIdentityRecovery=0;


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

        if (m_nIdentityPoint < CharStatus.ISP)
        {
            if (m_nIdentityRecovery <= 0)
            {
                m_nIdentityPoint += CharStatus.ISPRP;
                m_nIdentityRecovery = CharStatus.ISPRT;
            }
            else
            {
                m_nIdentityRecovery -= Time.deltaTime;
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
        switch ((ActionState)_idx)
        {
            case ActionState.Attack:
                skillAction = MeleeAttack;
                animator.SetBool("Attack", true);
                strActionAniName = "Attack";
                break;
            case ActionState.Skill1:
                skillAction = ShieldBash;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = ShieldRush;
                animator.SetBool("Skill2", true);
                strActionAniName = "Skill2";
                break;
            case ActionState.Skill3:
                skillAction = ProtectZone;
                animator.SetBool("Skill3", true);
                strActionAniName = "Skill3";
                break;
            case ActionState.Skill4:
                skillAction = TargetTaunt;
                animator.SetBool("Skill4", true);
                strActionAniName = "Skill4";
                break;
            case ActionState.IdentitySkill:
                skillAction = OnShield;
                animator.SetBool("Identity", true);
                strActionAniName = "Identity";
                break;
        }

    }

    void AniBoolOffAll()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Skill1", false);
        animator.SetBool("Skill2", false);
        animator.SetBool("Skill3", false);
        animator.SetBool("Skill4", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Identity", false);
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
                agent.SetDestination(vecMovePoint);
                animator.SetBool("Move", true);
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
                    //AlgorithmManager.SetAlgorithm(CharStatus.getID(), CharStatus);
                    ////Debug.Log(this.gameObject.name+","+this.gameObject.layer);
                }
                break;
            case CharState.Move:
                //MoveAlgorithm();
                Invoke("MoveAlgorithm",0.1f);
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
    void MoveAlgorithm()
    {

        if (agent.remainingDistance <= 0.2f)
        {
            //Debug.Log("Check");
            agent.velocity = Vector3.zero;
            SetCharStatus(CharState.Idle);
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


    void MeleeAttack()
    {
        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        if(m_bAttackCheck)
        {
            int m_nMask = m_nTargetLayer[0];



            Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
                Quaternion.Euler(new Vector3(0, GetAngle(transform.position, PlayerLookingPoint()), 0)), m_nMask);

            if (hitcol.Length != 0)
            {
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
            }
            m_bAttackCheck = false;
        }
        


    }


    void ShieldBash()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        if (m_bAttackCheck)
        {
            int m_nMask = m_nTargetLayer[0];
            Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
                Quaternion.Euler(new Vector3(0, GetAngle(transform.position, PlayerLookingPoint()), 0)), m_nMask);

            //Debug.Log(hitcol[0].gameObject);
            if (hitcol.Length != 0)
            {
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.DEF * SkillDB.getSkillCeofficientPer1()));
            }
        }



    }

    float m_fRushTimer = 0;

    void ShieldRush()
    {
        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];

        Vector3 MousePoint = vecMovePoint;



        if (!m_bSkillUsing[2])
        {

            Vector3 vecEndPos = this.transform.position + (this.transform.forward * 15f);

            
            agent.SetDestination(vecEndPos);
            agent.speed = CharStatus.SPD * 3.5f;
            agent.acceleration = 16;
            m_bSuperArmor = true;
            m_fRushTimer = 0;
            m_bSkillUsing[2] = true;
        }
        else
        {


            if (agent.remainingDistance <= 0.2f)
            {
                m_bSkillUsing[2] = false;
                m_bSuperArmor = false;
                agent.speed = CharStatus.SPD;
                agent.acceleration = 8;
                return;
            }

            //gameObject.transform.Translate(Vector3.forward * CharStatus.SPD * 3.5f * Time.deltaTime);

            int m_nMask = m_nTargetLayer[0];

            Collider[] hitcol = Physics.OverlapSphere(gameObject.transform.position, 1f, m_nMask);

            if (hitcol.Length != 0)
            {
                //hitcol[0].GetComponent<Char_Base>().GetDamage((int)(CS.getDEF() * ((m_fRustDist / 30) * SkillDB.getSkillCeofficientPer1())));
                hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.DEF * ((m_fRushTimer / 3f) * SkillDB.getSkillCeofficientPer1())));
                m_bSkillUsing[2] = false;
                m_bSuperArmor = false;
                agent.speed = CharStatus.SPD;
                agent.acceleration = 8;
                return;
                //CD.SetCharStatus(CharState.Idle);
            }

        }


    }

    void ProtectZone()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[3]];


        GameObject objProtectZone = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), gameObject.transform.position, Quaternion.identity);
        objProtectZone.transform.parent = gameObject.transform;
        objProtectZone.GetComponent<ParticleSystem>().Play();
        Destroy(objProtectZone, SkillDB.getSkillUsingTime() + 3f);

        int m_nMask = m_nTargetLayer[1];
        Collider[] hitcol = Physics.OverlapSphere(gameObject.transform.position, SkillDB.getSkillRange1(), m_nMask);
        int count = 0;


        OnDrBuff(SkillDB.getSkillUsingTime(), SkillDB.getSkillCeofficientPer1());

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Base>().OnDrBuff(SkillDB.getSkillUsingTime(), SkillDB.getSkillCeofficientPer1());

            count++;
        }


    }


    void TargetTaunt()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[4]];

        int m_nMask = m_nTargetLayer[0];

        Collider[] hitcol = Physics.OverlapSphere(gameObject.transform.position, 15f, m_nMask);

        if (hitcol.Length != 0)
        {

            hitcol[0].GetComponent<Char_Base>().OnTaunt(5f,gameObject);

        }

    }

    void OnShield()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[5];

        float dis = Vector3.Distance(transform.position, PlayerLookingPoint());


        if (!m_bSkillUsing[5])
        {
            // 타깃 바라보기
            transform.LookAt(PlayerLookingPoint());


            if (m_nIdentityPoint > 0)
            {
                delGetDamage = UseIdentitiy;
                m_bSkillUsing[5] = true;
                //변동확인hp = 지금HP
            }
            else
            {
                delGetDamage = GetDamage;
                m_bSkillUsing[5] = false;
                SetCharStatus(CharState.Idle);
            }

        }
        else
        {
            dis = Vector3.Distance(transform.position, PlayerLookingPoint());

            //if(변동확인hp < 지금HP)

            //체력이 맞아서 줄고 그체력을 다시 회복시키고 고유자원을 깐다

            if (m_nIdentityPoint <= 0)
            {
                delGetDamage = GetDamage;
                m_bSkillUsing[5] = false;
            }

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
