using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlgorithmManager 
{

    public static void SetAlgorithm(int _id, Char_Status _CS)
    {
        switch (_id)
        {
            case 0:
                
                break;
            case 1:
                HealerPatternSetting(_CS);
                break;
            case 2:
                ThiefPatternSetting(_CS);
                break;
            case 10:
                EnemyPatternSetting(_CS);
                break;

        }
    }


    static void HealerPatternSetting(Char_Status _CS)
    {
        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();

        //Debug.Log("Check");

        // ���� Ž��
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //���� ������ Ÿ��
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //�� �Ʊ� Ȯ�� üũ
        CS.setCheck02(false);
        //�ڽ� ü���� ȸ�� ���� Ȯ��
        bool mineCheck=false;
        //Ÿ������
        GameObject Target=null;


        if (GameManager.instance.getGameEnd())
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {




            //Ž�� �ݶ��̴� ����
            while (count < hitcol.Length)
            {
                if (hitcol[count].gameObject.layer == 6 || hitcol[count].gameObject.layer == 9)// �÷��̾� �Ǵ� �����϶�
                {
                    Char_Status cs = hitcol[count].gameObject.GetComponent<Char_Status>();
                    if (cs.getHP() <= cs.getHPMax() / 2 && cs.getCS() != GameManager.CharState.Death)//ü���� �������϶�
                    {
                        if (CS.getMP() >= 50 && CS.getSkill1On())// ��ų ��밡�� ���� Ȯ��
                        {
                            CS.SetObjTarget(hitcol[count].gameObject);// Ÿ������
                            Target = hitcol[count].gameObject;
                            CS.setCheck02(false);//�Ʊ� üũ (���� �ƴϴ�)
                            break;
                        }

                    }
                }
                else //�����϶�
                {
                    CS.SetObjTarget(hitcol[count].gameObject);
                    Target = hitcol[count].gameObject;
                    //PartnerTarget = hitcol[count].gameObject;
                    CS.setCheck02(true);// �� üũ
                                        //setEnemy = true;
                }
                count++;

            }

            // �ڽ��� ü�� ���¿� ���� Ÿ�� ��ȭ
            if (CS.getHP() <= CS.getHPMax() / 2 && CS.getMP() >= 50) // �ڽ��� ȸ���� �ʿ��Ҷ�
            {
                CS.SetObjTarget(CS.gameObject);// Ÿ���� �ڽ�����

                if (CS.getCheck02())// ȸ���� �Ʊ�ĳ���ʹ� ������
                {
                    mineCheck = true; //ȸ���� �Ʊ����� ����
                    CS.setCheck02(false);// ȸ������ ���� ����
                }

            }
            //Ÿ�ٰ��� �Ÿ�
            Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, CS.gameObject.transform.position.y, Target.transform.position.z);
            float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);

            // �ൿ ����
            if (CS.getObjTarget() != null)// Ÿ���� �����Ҷ�
            {
                if (CS.getCheck02())//TargetEnemyCheck // ���϶�
                {
                    if (dis > 20f)//�Ÿ� 20 ���� �ֶ�
                    {
                        CS.setCheck01(false);// RunAway Dist Check
                        CD.SetCharStatus(GameManager.CharState.Move);
                    }
                    else if (dis < 15f) //�Ÿ� 15���� ����ﶧ
                    {
                        CS.setCheck01(true);// RunAway Dist Check
                        CD.SetCharStatus(GameManager.CharState.Move);
                    }
                    else //�����Ÿ�
                    {
                        CD.SetCharStatus(GameManager.CharState.Attack);
                    }
                }
                else //�Ʊ��϶�
                {
                    if (dis > 15f) //�Ÿ� 15���� �ֶ�(�� �Ÿ��� �ȵ� ��)
                    {
                        CS.setCheck01(false);// RunAway Dist Check
                        CD.SetCharStatus(GameManager.CharState.Move);
                    }
                    else// ��ų ��Ÿ� �϶�
                    {
                        if (CS.getObjTarget() == CS.gameObject)// Ÿ���� �ڽ��϶�
                        {
                            if (mineCheck) // ġ���ؾ��� ���ᰡ ������
                            {
                                CD.SetCharStatus(GameManager.CharState.Skill1);
                            }
                            else //ġ���ؾ��� ���ᰡ ������
                            {
                                if (CS.getMP() >= 100 && CS.getSkill2On())//��ü�� ��� ����
                                {
                                    CD.SetCharStatus(GameManager.CharState.Skill2);
                                }
                                else
                                {
                                    CD.SetCharStatus(GameManager.CharState.Skill1);
                                }
                            }
                        }
                        else
                        {
                            CD.SetCharStatus(GameManager.CharState.Skill1);
                        }
                    }
                }
            }
        }

    }

    static void ThiefPatternSetting(Char_Status _CS)
    {
        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();


        // ���� Ž��
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);


        //���� ������ Ÿ��
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //Ÿ������
        GameObject Target = null;

        if (GameManager.instance.getGameEnd())
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {



            if (hitcol != null)
            {
                CS.SetObjTarget(hitcol[0].gameObject);
                Target = hitcol[0].gameObject;
            }


            Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, CS.gameObject.transform.position.y, Target.transform.position.z);
            float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);


            if (dis > 3.5f)
            {
                CD.SetCharStatus(GameManager.CharState.Move);
            }
            else
            {
                if (CS.getMP() >= 50 && CS.getCheck01() && CS.getSkill2On())
                {
                    CD.SetCharStatus(GameManager.CharState.Skill2);
                }
                else if (CS.getMP() >= 40 && !CS.getCheck01() && CS.getSkill1On())
                {
                    CD.SetCharStatus(GameManager.CharState.Skill1);
                }
                else
                {
                    CD.SetCharStatus(GameManager.CharState.Attack);
                }

            }
        }
        //if (Target == null || Target.activeSelf == false)
        //{
        //    PD.SetPartnerStatus(GameManager.CharState.Idle);
        //}
    }




    static void EnemyPatternSetting(Char_Status _CS)
    {
        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();


        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;
        int i = 0;
        //m_fActionDelayTimer = 3f;

        //animator.Play("Idle01");


        while (count < hitcol.Length)
        {
            if (hitcol[count].GetComponent<Char_Status>().getCS() != GameManager.CharState.Death)
            {
                break;
            }
            else
            {
                i++;
            }
            count++;
        }

        //Debug.Log(i+","+count + "," + hitcol.Length);

        if (i == hitcol.Length)
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {
            while (true)
            {
                TargetObj = hitcol[Random.Range(0, hitcol.Length)].gameObject;
                if (TargetObj.GetComponent<Char_Status>().getCS() != GameManager.CharState.Death)
                {
                    break;
                }

            }


            int random = Random.Range(0, 100);

            if (Vector3.Distance(TargetObj.transform.position, CS.gameObject.transform.position) < 8f)
            {
                if (random < 75)
                {

                    if (Random.Range(0, 2) == 0)
                    {
                        //AttackDelayTimer = AttackDelayTime;
                        CD.SetCharStatus(GameManager.CharState.Skill1);
                    }
                    else
                    {
                        //AttackDelayTimer = AttackDelayTime * 2;
                        CD.SetCharStatus(GameManager.CharState.Skill2);
                    }

                }
                else
                {
                    //AttackDelayTimer = AttackDelayTime * 2;
                    if (Random.Range(0, 2) == 0)
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill3);
                    }
                    else
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill4);
                    }

                }

            }
            else if (Vector3.Distance(TargetObj.transform.position, CS.gameObject.transform.position) < 20f)
            {
                if (random < 75)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill3);
                    }
                    else
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill4);
                    }
                }
                else
                {
                    CD.SetCharStatus(GameManager.CharState.Move);
                }
            }
            else
            {

                    CD.SetCharStatus(GameManager.CharState.Move);

            }
            //Debug.Log("Dist : " + Vector3.Distance(objTarget.transform.position, this.transform.position));
            //Debug.Log("AttackPattern : " + EA);
            // Debug.Log("ActionPattern : " + ES);

        }
    }



}
