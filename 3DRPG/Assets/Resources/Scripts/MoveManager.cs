using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MoveManager 
{
    //����Ʈ �̿�

    public static void SetMove(int _id, Partner_Dynamics _PD)
    {
        switch (_id)
        {
            case 0:
                HealerMoving(_id,_PD);
                break;
            case 1:
                ThiefMoving(_id, _PD);
                break;
        }
    }


    static float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }


    static bool HealerMoving(int _id,Partner_Dynamics _PD)
    {
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.gameObject.transform.position.y, TargetObj.transform.position.z);
        float dis = Vector3.Distance(PD.gameObject.transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // Ÿ�� üũ
        if (PD.getCheck02())//Ÿ���� ���� ��
        {
            if (dis > 20f) //�Ÿ��� 20���� �ֶ�
            {
                PD.transform.LookAt(vecEnemyLookingPoint);//Ÿ�� �Ĵٺ���
                PD.transform.position = Vector3.MoveTowards(PD.gameObject.transform.position, vecEnemyLookingPoint, charstatus.m_fPlayerSpeed * Time.deltaTime);
                return true;
            }
            else if (dis < 15)//�Ÿ��� 15���� ����ﶧ
            {
                
                if (PD.getCheck01())//������ ���� ���� 
                {
                    if (GetAngle(PD.transform.position, vecEnemyLookingPoint) < 0)
                    {
                        PD.gameObject.transform.rotation = Quaternion.Euler(0, 180 + GetAngle(PD.gameObject.transform.position, vecEnemyLookingPoint), 0);
                    }
                    else
                    {
                        PD.gameObject.transform.rotation = Quaternion.Euler(0, GetAngle(PD.gameObject.transform.position, vecEnemyLookingPoint) - 180, 0);
                    }
                    PD.setCheck01(false);
                }
                
                PD.gameObject.transform.Translate(Vector3.forward * charstatus.m_fPlayerSpeed * Time.deltaTime);
                return true;
            }
            else
            {
                PD.SetPartnerStatus(GameManager.CharState.Attack);
                //AlgorithmManager.SetAlgorithm(_id,_PD);
                return false;
            }
        }
        else // Ÿ���� �Ʊ��϶�
        {
            if (dis > 15) //�Ÿ��� 15���� �ֶ�
            {
                PD.gameObject.transform.LookAt(vecEnemyLookingPoint);

                PD.gameObject.transform.position = Vector3.MoveTowards(PD.gameObject.transform.position, vecEnemyLookingPoint, charstatus.m_fPlayerSpeed * Time.deltaTime);
                
                return true;
            }
            else //���� �Ÿ�
            {
                AlgorithmManager.SetAlgorithm(_id, _PD);
                return false;
            }
        }
        
    }


    static bool ThiefMoving(int _id, Partner_Dynamics _PD)
    {

        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.gameObject.transform.position.y, TargetObj.transform.position.z);
        float dis = Vector3.Distance(PD.gameObject.transform.position, vecEnemyLookingPoint);


        if (dis > 3.5f)
        {
            animator.Play("Run");
            PD.transform.LookAt(vecEnemyLookingPoint);

            PD.transform.position = Vector3.MoveTowards(PD.transform.position, vecEnemyLookingPoint, charstatus.m_fPlayerSpeed * Time.deltaTime);
            //Debug.Log(dis);
            return true;
        }
        else
        {
            AlgorithmManager.SetAlgorithm(_id, _PD);
            return false;
        }

    }



}
