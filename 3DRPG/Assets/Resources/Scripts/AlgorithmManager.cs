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
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();

        //Debug.Log("Check");

        // 범위 탐색
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //공격 딜레이 타임
        PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //적 아군 확인 체크
        PD.setCheck02(false);
        //자신 체력의 회복 여부 확인
        bool mineCheck=false;
        //타겟지정
        GameObject Target=null;

        //if (hitcol[0].GetComponent<Enemy_Ctrl>().ES == GameManager.EnemyState.Death && hitcol != null&& hitcol[0].gameObject.layer==8)
        //{
        //    charstatus.CS = GameManager.CharState.Stay;
        //}

        //탐색 콜라이더 선별
        while (count < hitcol.Length)
        {
            if (hitcol[count].gameObject.layer == 6 || hitcol[count].gameObject.layer == 9)// 플레이어 또는 동료일때
            {
                if (hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHP <= hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHPMax / 2)//체력이 반이하일때
                {
                    if (charstatus.m_nPlayerMP >= 50 && PD.getSkill1On())// 스킬 사용가능 여부 확인
                    {
                        PD.SetObjTarget(hitcol[count].gameObject);// 타겟지정
                        Target = hitcol[count].gameObject;
                        PD.setCheck02(false);//아군 체크
                        break;
                    }

                }
            }
            else //몬스터일때
            {
                PD.SetObjTarget(hitcol[count].gameObject);
                Target = hitcol[count].gameObject;
                //PartnerTarget = hitcol[count].gameObject;
                PD.setCheck02(true);
                //setEnemy = true;
            }
            count++;

        }

        // 자신의 체력 상태에 때른 타겟 변화
        if (charstatus.m_nPlayerHP <= charstatus.m_nPlayerHPMax / 2 && charstatus.m_nPlayerMP >= 50) // 자신이 회복이 필요할때
        {
            PD.SetObjTarget(PD.gameObject);// 타겟을 자신으로

            if (PD.getCheck02())// 회복할 아군캐릭터는 없을때
            {
                mineCheck = true; //회복할 아군없음 구분
                PD.setCheck02(false);// 회복으로 조건 진행
            }
                
        }
        //타겟과의 거리
        Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, PD.gameObject.transform.position.y, Target.transform.position.z);
        float dis = Vector3.Distance(PD.gameObject.transform.position, vecEnemyLookingPoint);
        
        // 행동 설정
        if (PD.getObjTarget() != null)// 타겟이 존재할때
        {
            if (PD.getCheck02())//TargetEnemyCheck // 적일때
            {
                if (dis > 20f)//거리 20 보다 멀때
                {
                    PD.setCheck01(false);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else if (dis < 15f) //거리 15보다 가까울때
                {
                    PD.setCheck01(true);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else //적정거리
                {
                    PD.SetPartnerStatus(GameManager.CharState.Attack);
                }
            }
            else //아군일때
            {
                if (dis > 15f) //거리 15보다 멀때(힐 거리가 안될 때)
                {
                    PD.setCheck01(false);// RunAway Dist Check
                    PD.SetPartnerStatus(GameManager.CharState.Move);
                }
                else// 스킬 사거리 일때
                {
                    if (PD.getObjTarget() == PD.gameObject)// 타겟이 자신일때
                    {
                        if(mineCheck) // 치료해야할 동료가 없을때
                        {
                           PD.SetPartnerStatus(GameManager.CharState.Skill1);
                        }
                        else //치료해야할 동료가 있을때
                        {
                            if (charstatus.m_nPlayerMP >= 100 && PD.getSkill2On())//전체힐 사용 조건
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
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();


         // 범위 탐색
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //공격 딜레이 타임
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
