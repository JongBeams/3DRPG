using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MoveManager 
{
    //����Ʈ �̿�

    public static void SetMove(int _id, Char_Status _CS)
    {
        switch (_id)
        {
            case 0:
                PlayerMoving(_id, _CS);
                break;
            case 1:
                HealerMoving(_id, _CS);
                break;
            case 2:
                ThiefMoving(_id, _CS);
                break;
                
        }
    }


    static float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }


    static bool HealerMoving(int _id,Char_Status _CS)
    {
        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.gameObject.GetComponent<Char_Dynamics>();

        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.gameObject.transform.position.y, TargetObj.transform.position.z);
        float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // Ÿ�� üũ
        if (CS.getCheck02())//Ÿ���� ���� ��
        {
            if (dis > 20f) //�Ÿ��� 20���� �ֶ�
            {
                CS.transform.LookAt(vecEnemyLookingPoint);//Ÿ�� �Ĵٺ���
                CS.transform.position = Vector3.MoveTowards(CS.gameObject.transform.position, vecEnemyLookingPoint, CS.getSpeed() * Time.deltaTime);
                return true;
            }
            else if (dis < 15)//�Ÿ��� 15���� ����ﶧ
            {
                
                if (CS.getCheck01())//������ ���� ���� 
                {
                    if (GetAngle(CS.transform.position, vecEnemyLookingPoint) < 0)
                    {
                        CS.gameObject.transform.rotation = Quaternion.Euler(0, 180 + GetAngle(CS.gameObject.transform.position, vecEnemyLookingPoint), 0);
                    }
                    else
                    {
                        CS.gameObject.transform.rotation = Quaternion.Euler(0, GetAngle(CS.gameObject.transform.position, vecEnemyLookingPoint) - 180, 0);
                    }
                    CS.setCheck01(false);
                }
                
                CS.gameObject.transform.Translate(Vector3.forward * CS.getSpeed() * Time.deltaTime);
                return true;
            }
            else
            {
                CD.SetCharStatus(GameManager.CharState.Attack);
                //AlgorithmManager.SetAlgorithm(_id,_PD);
                return false;
            }
        }
        else // Ÿ���� �Ʊ��϶�
        {
            if (dis > 15) //�Ÿ��� 15���� �ֶ�
            {
                CS.gameObject.transform.LookAt(vecEnemyLookingPoint);

                CS.gameObject.transform.position = Vector3.MoveTowards(CS.gameObject.transform.position, vecEnemyLookingPoint, CS.getSpeed() * Time.deltaTime);
                
                return true;
            }
            else //���� �Ÿ�
            {
                AlgorithmManager.SetAlgorithm(_id, _CS);
                return false;
            }
        }
        
    }


    static bool ThiefMoving(int _id, Char_Status _CS)
    {

        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();

        // Ÿ�ٰ��� �Ÿ�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.gameObject.transform.position.y, TargetObj.transform.position.z);
        float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);


        if (dis > 3.5f)
        {
            animator.Play("Run");
            CS.transform.LookAt(vecEnemyLookingPoint);

            CS.transform.position = Vector3.MoveTowards(CS.transform.position, vecEnemyLookingPoint, CS.getSpeed() * Time.deltaTime);
            //Debug.Log(dis);
            return true;
        }
        else
        {
            AlgorithmManager.SetAlgorithm(_id, _CS);
            return false;
        }

    }

    static bool PlayerMoving(int _id, Char_Status _CS)
    {
        //��Ʈ�� ����
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //ĳ���� ���� �ӽ�
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();

        float dis = Vector3.Distance(CS.transform.position, CD.getMovePoint());

        if (dis >= 0.02f)
        {
            //animator.Play("Move");
            CS.transform.LookAt(CD.PlayerLookingPoint());

            CS.transform.position = Vector3.MoveTowards(CS.transform.position, CD.PlayerLookingPoint(), CS.getSpeed() * Time.deltaTime);

            return true;
        }
        // animator.Play("Idle_SwordShield");
        CD.SetCharStatus(GameManager.CharState.Idle);
        return false;
    }

}
