using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlgorithmManager 
{

    public static void SetAlgorithm(int _id, Partner_Dynamics _PD)
    {
        switch (_id)
        {
            case 0:
                HealerPatternSetting(_PD);
                break;
            case 1:
                HealerPatternSetting(_PD);
                break;

        }
    }


    static void HealerPatternSetting(Partner_Dynamics _PD)
    {
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        //Debug.Log("Check");

        // ���� Ž��
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //���� ������ Ÿ��
        PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //�� �Ʊ� Ȯ�� üũ
        PD.setCheck02(false);
        //�ڽ� ü���� ȸ�� ���� Ȯ��
        bool mineCheck=false;
        //Ÿ������
        GameObject Target=null;

        //if (hitcol[0].GetComponent<Enemy_Ctrl>().ES == GameManager.EnemyState.Death && hitcol != null&& hitcol[0].gameObject.layer==8)
        //{
        //    charstatus.CS = GameManager.CharState.Stay;
        //}

        //Ž�� �ݶ��̴� ����
        while (count < hitcol.Length)
        {
            if (hitcol[count].gameObject.layer == 6 || hitcol[count].gameObject.layer == 9)// �÷��̾� �Ǵ� �����϶�
            {
                if (hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHP <= hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHPMax / 2)//ü���� �������϶�
                {
                    if (charstatus.m_nPlayerMP >= 50 && PD.getSkill1On())// ��ų ��밡�� ���� Ȯ��
                    {
                        PD.SetObjTarget(hitcol[count].gameObject);// Ÿ������
                        Target = hitcol[count].gameObject;
                        PD.setCheck02(false);//�Ʊ� üũ
                        break;
                    }

                }
            }
            else //�����϶�
            {
                PD.SetObjTarget(hitcol[count].gameObject);
                Target = hitcol[count].gameObject;
                //PartnerTarget = hitcol[count].gameObject;
                PD.setCheck02(true);
                //setEnemy = true;
            }
            count++;

        }

        // �ڽ��� ü�� ���¿� ���� Ÿ�� ��ȭ
        if (charstatus.m_nPlayerHP <= charstatus.m_nPlayerHPMax / 2 && charstatus.m_nPlayerMP >= 50) // �ڽ��� ȸ���� �ʿ��Ҷ�
        {
            PD.SetObjTarget(PD.gameObject);// Ÿ���� �ڽ�����

            if (PD.getCheck02())// ȸ���� �Ʊ�ĳ���ʹ� ������
            {
                mineCheck = true; //ȸ���� �Ʊ����� ����
                PD.setCheck02(false);// ȸ������ ���� ����
            }
                
        }
        //Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, PD.gameObject.transform.position.y, Target.transform.position.z);
        float dis = Vector3.Distance(PD.gameObject.transform.position, vecEnemyLookingPoint);
        
        // �ൿ ����
        if (PD.getObjTarget() != null)// Ÿ���� �����Ҷ�
        {
            if (PD.getCheck02())//TargetEnemyCheck // ���϶�
            {
                if (dis > 20f)//�Ÿ� 20 ���� �ֶ�
                {
                    PD.setCheck01(false);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else if (dis < 15f) //�Ÿ� 15���� ����ﶧ
                {
                    PD.setCheck01(true);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else //�����Ÿ�
                {
                    PD.SetPartnerStatus(GameManager.CharState.Attack);
                }
            }
            else //�Ʊ��϶�
            {
                if (dis > 15f) //�Ÿ� 15���� �ֶ�(�� �Ÿ��� �ȵ� ��)
                {
                    PD.setCheck01(false);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else// ��ų ��Ÿ� �϶�
                {
                    if (PD.getObjTarget() == PD.gameObject)// Ÿ���� �ڽ��϶�
                    {
                        if(mineCheck) // ġ���ؾ��� ���ᰡ ������
                        {
                           PD.SetPartnerStatus(GameManager.CharState.Skill1);
                        }
                        else //ġ���ؾ��� ���ᰡ ������
                        {
                            if (charstatus.m_nPlayerMP >= 100 && PD.getSkill2On())//��ü�� ��� ����
                            {
                                PD.SetPartnerStatus(GameManager.CharState.Skill2);
                            }
                            else
                            {
                                PD.SetPartnerStatus(GameManager.CharState.Skill1);
                            }
                        }
                    }
                    else
                    {
                        PD.SetPartnerStatus(GameManager.CharState.Skill1);
                    }
                }
            }
        }


    }

    static void ThiefPatternSetting(Partner_Dynamics _PD)
    {
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();


         // ���� Ž��
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //���� ������ Ÿ��
        PD.setAttackDelayTimer(PD.getAttackDelayTime());



        //if (hitcol[0].GetComponent<Enemy_Ctrl>().ES == GameManager.EnemyState.Death && hitcol != null&& hitcol[0].gameObject.layer==8)
        //{
        //    charstatus.CS = GameManager.CharState.Stay;
        //}

        if (hitcol != null)
        {
            TargetObj = hitcol[0].gameObject;
        }


        if (Vector3.Distance(TargetObj.transform.position, PD.transform.position) > 3.5f)
        {
            PD.SetPartnerStatus(GameManager.CharState.Move);
        }
        else
        {
            if (charstatus.m_nPlayerMP >= 50 && PD.getCheck01() && PD.getSkill2On())
            {
                PD.SetPartnerStatus(GameManager.CharState.Skill2);
            }
            else if (charstatus.m_nPlayerMP >= 40 && !PD.getCheck01() && PD.getSkill1On())
            {
                PD.SetPartnerStatus(GameManager.CharState.Skill1);
            }
            else
            {
                PD.SetPartnerStatus(GameManager.CharState.Attack);
            }

        }

        if (TargetObj == null || TargetObj.activeSelf == false)
        {
            PD.SetPartnerStatus(GameManager.CharState.Idle);
        }
    }


}
