using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MoveManager 
{
    //리스트 이용

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
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        // 타겟과의 거리
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.gameObject.transform.position.y, TargetObj.transform.position.z);
        float dis = Vector3.Distance(PD.gameObject.transform.position, vecEnemyLookingPoint);

        //Debug.Log("PartnerTargetObj : "+ TargetObj + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        // 타겟 체크
        if (PD.getCheck02())//타겟이 적일 때
        {
            if (dis > 20f) //거리가 20보다 멀때
            {
                PD.transform.LookAt(vecEnemyLookingPoint);//타겟 쳐다보기
                PD.transform.position = Vector3.MoveTowards(PD.gameObject.transform.position, vecEnemyLookingPoint, charstatus.m_fPlayerSpeed * Time.deltaTime);
                return true;
            }
            else if (dis < 15)//거리가 15보다 가까울때
            {
                
                if (PD.getCheck01())//역방향 각도 조정 
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
        else // 타겟이 아군일때
        {
            if (dis > 15) //거리가 15보다 멀때
            {
                PD.gameObject.transform.LookAt(vecEnemyLookingPoint);

                PD.gameObject.transform.position = Vector3.MoveTowards(PD.gameObject.transform.position, vecEnemyLookingPoint, charstatus.m_fPlayerSpeed * Time.deltaTime);
                
                return true;
            }
            else //적정 거리
            {
                AlgorithmManager.SetAlgorithm(_id, _PD);
                return false;
            }
        }
        
    }


    static bool ThiefMoving(int _id, Partner_Dynamics _PD)
    {

        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        // 타겟과의 거리
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
