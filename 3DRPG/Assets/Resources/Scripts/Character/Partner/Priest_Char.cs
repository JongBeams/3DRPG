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
        //�� �Ʊ� Ȯ�� üũ
        m_bCheck[1] = false;
        //�ڽ� ü���� ȸ�� ���� Ȯ��
        bool mineCheck = false;
        //Ÿ������
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
            //Ž�� �ݶ��̴� ����
            while (count < hitcol.Length)
            {
                if (hitcol[count].gameObject.layer == m_nTargetLayer[1])// �÷��̾� �Ǵ� �����϶�
                {
                    Char_Base cs = hitcol[count].gameObject.GetComponent<Char_Base>();
                    if (cs.m_nPlayerHP <= cs.CharStatus.HP / 2 && cs.CS != CharState.Death)//ü���� �������϶�
                    {
                        if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana() && m_bSkillOn[1])// ��ų ��밡�� ���� Ȯ��
                        {
                            objTarget = hitcol[count].gameObject;// Ÿ������
                            Target = hitcol[count].gameObject;
                            m_bCheck[1] = false;//�Ʊ� üũ (���� �ƴϴ�)
                            break;
                        }

                    }
                }
                else //�����϶�
                {
                    objTarget = hitcol[count].gameObject;
                    Target = hitcol[count].gameObject;
                    //PartnerTarget = hitcol[count].gameObject;
                    m_bCheck[1] = true;// �� üũ
                                       //setEnemy = true;
                }

                count++;

            }

            // �ڽ��� ü�� ���¿� ���� Ÿ�� ��ȭ
            if (m_nPlayerHP <= CharStatus.HP / 2 && m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana()) // �ڽ��� ȸ���� �ʿ��Ҷ�
            {
                objTarget = this.gameObject;// Ÿ���� �ڽ�����

                if (m_bCheck[1])// ȸ���� �Ʊ�ĳ���ʹ� ������
                {
                    mineCheck = true; //ȸ���� �Ʊ����� ����
                    m_bCheck[1] = false;// ȸ������ ���� ����
                }

            }
        }


        if (Target != null)
        {
            //Ÿ�ٰ��� �Ÿ�
            Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            float dis = Vector3.Distance(transform.position, vecEnemyLookingPoint);

            // �ൿ ����
            if (objTarget != null)// Ÿ���� �����Ҷ�
            {
                if (m_bCheck[1])//TargetEnemyCheck // ���϶�
                {
                    if (dis > 20f)//�Ÿ� 20 ���� �ֶ�
                    {
                        m_bCheck[0] = false;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else if (dis < 15f) //�Ÿ� 15���� ����ﶧ
                    {
                        m_bCheck[0] = true;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else //�����Ÿ�
                    {
                        m_nActionIdx = 0;
                        SetCharStatus(CharState.Action);
                    }
                }
                else //�Ʊ��϶�
                {
                    if (dis > 15f) //�Ÿ� 15���� �ֶ�(�� �Ÿ��� �ȵ� ��)
                    {
                        m_bCheck[0] = false;// RunAway Dist Check
                        SetCharStatus(CharState.Move);
                    }
                    else// ��ų ��Ÿ� �϶�
                    {
                        if (objTarget == gameObject)// Ÿ���� �ڽ��϶�
                        {
                            if (mineCheck) // ġ���ؾ��� ���ᰡ ������
                            {
                                m_nActionIdx = 1;
                                SetCharStatus(CharState.Action);
                            }
                            else //ġ���ؾ��� ���ᰡ ������
                            {
                                if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[2]].getSkillUsingMana() && m_bSkillOn[2])//��ü�� ��� ����
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
                            if (m_nPlayerMP >= DBManager.SkillData[CharStatus.SID[1]].getSkillUsingMana() && m_bSkillOn[1])//��ü�� ��� ����
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


        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, gameObject.transform.position.y, objTarget.transform.position.z);
        float dis = Vector3.Distance(gameObject.transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // Ÿ�� üũ
        if (m_bCheck[1])//Ÿ���� ���� ��
        {
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
                agent.SetDestination(this.transform.position);
                agent.velocity = Vector3.zero;
                SetAlgorithm();
                //AlgorithmManager.SetAlgorithm(_id,_PD);
                return;
            }
        }
        else // Ÿ���� �Ʊ��϶�
        {
            if (dis > 15) //�Ÿ��� 15���� �ֶ�
            {
                transform.LookAt(vecEnemyLookingPoint);

                agent.SetDestination(vecEnemyLookingPoint);

                return;
            }
            else //���� �Ÿ�
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



    void TargetHealing()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];


        objTarget.GetComponent<Char_Base>().HealingHP((int)(CharStatus.MP * SkillDB.getSkillCeofficientPer1()));


    }

    void AllHealing()
    {


        //��ų ����
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


    #region ����



    #endregion


}
