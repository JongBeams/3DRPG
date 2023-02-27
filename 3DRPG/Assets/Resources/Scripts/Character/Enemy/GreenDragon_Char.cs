using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GreenDragon_Char : Char_Base
{
    // Start is called before the first frame update
    void Start()
    {
        SetComponents();
        SetCheck(2);
        m_nPlayerHP = CharStatus.HP;
        m_bSuperArmor = true;
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

    }


    protected override void SetSkillCoolTime()
    {
        m_bSkillOn[m_nActionIdx-1] = false;
        m_fSkillCoolTimer[m_nActionIdx - 1] = DBManager.SkillData[CharStatus.SID[m_nActionIdx-1]].SCT;
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
        switch ((ActionState)_idx)
        {
            case ActionState.Skill1:
                skillAction = MeleeTargetAttack;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = RangeAngleAttack1;
                animator.SetBool("Skill2", true);
                strActionAniName = "Skill2";
                break;
            case ActionState.Skill3:
                skillAction = RangeAngleAttack2;
                animator.SetBool("Skill3", true);
                strActionAniName = "Skill3";
                break;
            case ActionState.Skill4:
                skillAction = RangeAngleAttack3;
                animator.SetBool("Skill4", true);
                strActionAniName = "Skill4";
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
                skillAction?.Invoke();
                SetSkillCoolTime();
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

        int m_nMask = m_nTargetLayer[0];

        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;

        if (!m_bTaunt)
        {
            int TargetRan = Random.Range(0, hitcol.Length);
            while (count < hitcol.Length)
            {
                if (hitcol[TargetRan].GetComponent<Char_Base>().CS != CharState.Death)
                {
                    objTarget = hitcol[TargetRan].gameObject;
                    break;
                }
                else
                {
                    TargetRan = Random.Range(0, hitcol.Length);

                }
                count++;
            }
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

        int random = Random.Range(0, 4);
        if (Vector3.Distance(objTarget.transform.position, gameObject.transform.position) < 8f)
        {
            switch (random)
            {
                case 0:
                    m_nActionIdx = 1;
                    break;
                case 1:
                    m_nActionIdx = 2;
                    break;
                case 2:
                    m_nActionIdx = 3;
                    break;
                case 3:
                    m_nActionIdx = 4;
                    break;
            }
            SetCharStatus(CharState.Action);

        }
        else
        {

            SetCharStatus(CharState.Move);

        }


    }

    void MoveAlgorithm()
    {

        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, transform.position.y, objTarget.transform.position.z);
        float dis = Vector3.Distance(transform.position, vecEnemyLookingPoint);


        float Range = 6f;


        if (agent.remainingDistance <= Range)
        {
            agent.velocity = Vector3.zero;
            SetCharStatus(CharState.Idle);
            return;
        }


    }

    #endregion


    #region ��ų
    bool m_bAttackCheck;

    void AttackCheck()
    {
        m_bAttackCheck = true;
    }



    void MeleeTargetAttack()//vector ũ�⸸�ٸ���
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);


        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(3, 3, 3),
            Quaternion.Euler(new Vector3(0, GetAngle(gameObject.transform.position, PlayerLookingPoint()), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {

            hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));

        }

    }

    void RangeAngleAttack1()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);//�浹���� ����
        int count = 0;
        //int i = 0;
        while (count < hitcol.Length)
        {

            Vector3 targetDir = hitcol[count].transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
            if (angle <= SkillDB.getSkillRange2())
            {

                hitcol[count].gameObject.GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
            }

            count++;
        }


    }

    void RangeAngleAttack2()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);//�浹���� ����
        int count = 0;
        //int i = 0;
        while (count < hitcol.Length)
        {

            Vector3 targetDir = hitcol[count].transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
            if (angle <= SkillDB.getSkillRange2())
            {

                hitcol[count].gameObject.GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
            }

            count++;
        }


    }

    void RangeAngleAttack3()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[3]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);//�浹���� ����
        int count = 0;
        //int i = 0;
        while (count < hitcol.Length)
        {

            Vector3 targetDir = hitcol[count].transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
            if (angle <= SkillDB.getSkillRange2())
            {

                hitcol[count].gameObject.GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
            }

            count++;
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
