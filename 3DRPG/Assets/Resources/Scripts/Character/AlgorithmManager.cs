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
            case 3:
                MagicianPatternSetting(_CS);
                break;
            case 10:
                EnemyPatternSetting(_CS);
                break;
            case 11:
                Enemy2PatternSetting(_CS);
                break;

        }
    }


    static void HealerPatternSetting(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //캐릭터 상태 머신
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();

        

        //Debug.Log("Check");

        // 범위 탐색
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //공격 딜레이 타임
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //적 아군 확인 체크
        CS.setCheck02(false);
        //자신 체력의 회복 여부 확인
        bool mineCheck=false;
        //타겟지정
        GameObject Target=null;


        if (InGameSceneManager.Instance.m_bGameEnd)
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {

            if (!CS.getTaunt())
            {
                //탐색 콜라이더 선별
                while (count < hitcol.Length)
                {
                    if (CS.getTYP() == LayerMask.NameToLayer("Partner"))// 캐릭터가 동료 일때
                    {
                        if (hitcol[count].gameObject.layer == LayerMask.NameToLayer("Player") || hitcol[count].gameObject.layer == LayerMask.NameToLayer("Partner"))// 플레이어 또는 동료일때
                        {
                            Char_Status cs = hitcol[count].gameObject.GetComponent<Char_Status>();
                            if (cs.getHP() <= cs.getHPMax() / 2 && cs.getCS() != GameManager.CharState.Death)//체력이 반이하일때
                            {
                                if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana() && CS.getSkill1On())// 스킬 사용가능 여부 확인
                                {
                                    CS.SetObjTarget(hitcol[count].gameObject);// 타겟지정
                                    Target = hitcol[count].gameObject;
                                    CS.setCheck02(false);//아군 체크 (적이 아니다)
                                    break;
                                }

                            }
                        }
                        else //몬스터일때
                        {
                            CS.SetObjTarget(hitcol[count].gameObject);
                            Target = hitcol[count].gameObject;
                            //PartnerTarget = hitcol[count].gameObject;
                            CS.setCheck02(true);// 적 체크
                                                //setEnemy = true;
                        }
                    }
                    else
                    {
                        if (hitcol[count].gameObject.layer == LayerMask.NameToLayer("Enemy"))// 플레이어 또는 동료일때
                        {
                            Char_Status cs = hitcol[count].gameObject.GetComponent<Char_Status>();
                            if (cs.getHP() <= cs.getHPMax() / 2 && cs.getCS() != GameManager.CharState.Death)//체력이 반이하일때
                            {
                                if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana() && CS.getSkill1On())// 스킬 사용가능 여부 확인
                                {
                                    CS.SetObjTarget(hitcol[count].gameObject);// 타겟지정
                                    Target = hitcol[count].gameObject;
                                    CS.setCheck02(false);//아군 체크 (적이 아니다)
                                    break;
                                }

                            }
                        }
                        else //몬스터일때
                        {
                            CS.SetObjTarget(hitcol[count].gameObject);
                            Target = hitcol[count].gameObject;
                            //PartnerTarget = hitcol[count].gameObject;
                            CS.setCheck02(true);// 적 체크
                                                //setEnemy = true;
                        }
                    }

                    count++;

                }

                // 자신의 체력 상태에 때른 타겟 변화
                if (CS.getHP() <= CS.getHPMax() / 2 && CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana()) // 자신이 회복이 필요할때
                {
                    CS.SetObjTarget(CS.gameObject);// 타겟을 자신으로

                    if (CS.getCheck02())// 회복할 아군캐릭터는 없을때
                    {
                        mineCheck = true; //회복할 아군없음 구분
                        CS.setCheck02(false);// 회복으로 조건 진행
                    }

                }
            }
            

            if (Target!=null)
            {
                //타겟과의 거리
                Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, CS.gameObject.transform.position.y, Target.transform.position.z);
                float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);

                // 행동 설정
                if (CS.getObjTarget() != null)// 타겟이 존재할때
                {
                    if (CS.getCheck02())//TargetEnemyCheck // 적일때
                    {
                        if (dis > 20f)//거리 20 보다 멀때
                        {
                            CS.setCheck01(false);// RunAway Dist Check
                            CD.SetCharStatus(GameManager.CharState.Move);
                        }
                        else if (dis < 15f) //거리 15보다 가까울때
                        {
                            CS.setCheck01(true);// RunAway Dist Check
                            CD.SetCharStatus(GameManager.CharState.Move);
                        }
                        else //적정거리
                        {
                            CD.SetCharStatus(GameManager.CharState.Attack);
                        }
                    }
                    else //아군일때
                    {
                        if (dis > 15f) //거리 15보다 멀때(힐 거리가 안될 때)
                        {
                            CS.setCheck01(false);// RunAway Dist Check
                            CD.SetCharStatus(GameManager.CharState.Move);
                        }
                        else// 스킬 사거리 일때
                        {
                            if (CS.getObjTarget() == CS.gameObject)// 타겟이 자신일때
                            {
                                if (mineCheck) // 치료해야할 동료가 없을때
                                {
                                    CD.SetCharStatus(GameManager.CharState.Skill1);
                                }
                                else //치료해야할 동료가 있을때
                                {
                                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[1]].getSkillUsingMana() && CS.getSkill2On())//전체힐 사용 조건
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

    }

    static void ThiefPatternSetting(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //캐릭터 상태 머신
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();


        // 범위 탐색
        int m_nMask = 0;
        if (CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        }
        else
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        }
        
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);


        //공격 딜레이 타임
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //타겟지정
        GameObject Target = null;

        if (InGameSceneManager.Instance.m_bGameEnd)
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {



            if (hitcol != null)
            {

                if (!CS.getTaunt())
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
                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[1]].getSkillUsingMana() && CS.getCheck01() && CS.getSkill2On())
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill2);
                    }
                    else if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana() && !CS.getCheck01() && CS.getSkill1On())
                    {
                        CD.SetCharStatus(GameManager.CharState.Skill1);
                    }
                    else
                    {
                        CD.SetCharStatus(GameManager.CharState.Attack);
                    }

                }


            }


            
        }
        //if (Target == null || Target.activeSelf == false)
        //{
        //    PD.SetPartnerStatus(GameManager.CharState.Idle);
        //}
    }


    static void MagicianPatternSetting(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //캐릭터 상태 머신
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();



        //Debug.Log("Check");

        // 범위 탐색
        int m_nMask = 0;
        if (CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        }
        else
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        }
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        //공격 딜레이 타임
        //PD.setAttackDelayTimer(PD.getAttackDelayTime());
        //타겟지정
        GameObject Target = null;


        if (InGameSceneManager.Instance.m_bGameEnd)
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {

            if (hitcol != null)
            {
                if (!CS.getTaunt())
                {
                    CS.SetObjTarget(hitcol[0].gameObject);
                    Target = hitcol[0].gameObject;
                }


                //타겟과의 거리
                Vector3 vecEnemyLookingPoint = new Vector3(Target.transform.position.x, CS.gameObject.transform.position.y, Target.transform.position.z);
                float dis = Vector3.Distance(CS.gameObject.transform.position, vecEnemyLookingPoint);

                // 행동 설정
                if (CS.getObjTarget() != null)// 타겟이 존재할때
                {

                    if (dis > 20f)//거리 20 보다 멀때
                    {
                        CS.setCheck01(false);// RunAway Dist Check
                        CD.SetCharStatus(GameManager.CharState.Move);
                    }
                    else if (dis < 15f) //거리 15보다 가까울때
                    {
                        CS.setCheck01(true);// RunAway Dist Check
                        CD.SetCharStatus(GameManager.CharState.Move);
                    }
                    else //적정거리
                    {
                        int ran = Random.Range(0, 3);
                        if (ran == 2)
                        {
                            if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[1]].getSkillUsingMana() && CS.getSkill2On())
                            {
                                CD.SetCharStatus(GameManager.CharState.Skill2);
                            }
                            else
                            {
                                CD.SetCharStatus(GameManager.CharState.Attack);
                            }
                        }
                        else if (ran == 1)
                        {
                            if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana() && CS.getSkill1On())
                            {
                                CD.SetCharStatus(GameManager.CharState.Skill1);
                            }
                            else
                            {
                                CD.SetCharStatus(GameManager.CharState.Attack);
                            }
                        }
                        else
                        {
                            CD.SetCharStatus(GameManager.CharState.Attack);
                        }


                    }
                }

            }
        }

    }




    static void EnemyPatternSetting(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //캐릭터 상태 머신
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();


        int m_nMask = 0;
        if (CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        }
        else
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        }
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;

        if (!CS.getTaunt())
        {
            int TargetRan = Random.Range(0, hitcol.Length);
            while (count < hitcol.Length)
            {
                if (hitcol[TargetRan].GetComponent<Char_Status>().getCS() != GameManager.CharState.Death)
                {
                    CS.SetObjTarget(hitcol[TargetRan].gameObject);
                    break;
                }
                else
                {
                    TargetRan = Random.Range(0, hitcol.Length);

                }
                count++;
            }
        }

        

        //Debug.Log(i+","+count + "," + hitcol.Length);
        if (InGameSceneManager.Instance.CharAllDeathCheck())
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {
            //int TargetRan = 0;
            ////CS.SetObjTarget(hitcol[Random.Range(0, hitcol.Length)].gameObject);
            //while (TargetRan<hitcol.Length)
            //{
            //    CS.SetObjTarget(hitcol[Random.Range(0, hitcol.Length)].gameObject);
            //    //TargetObj = hitcol[Random.Range(0, hitcol.Length)].gameObject;
            //    if (TargetObj != null && TargetObj.GetComponent<Char_Status>().getCS() != GameManager.CharState.Death)
            //    {
            //        break;
            //    }
            //    TargetRan++;
            //}


            int random = Random.Range(0, 100);
            if (Vector3.Distance(CS.getObjTarget().transform.position, CS.gameObject.transform.position) < 8f)
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
            else if (Vector3.Distance(CS.getObjTarget().transform.position, CS.gameObject.transform.position) >= 8f && Vector3.Distance(CS.getObjTarget().transform.position, CS.gameObject.transform.position) < 20f)
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

    static void Enemy2PatternSetting(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //캐릭터 상태 머신
        Char_Dynamics CD = _CS.GetComponent<Char_Dynamics>();


        int m_nMask = 0;
        if (CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        }
        else
        {
            m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        }
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 30f, m_nMask);
        int count = 0;


        if (!CS.getTaunt())
        {
            int TargetRan = Random.Range(0, hitcol.Length);
            while (count < hitcol.Length)
            {
                if (hitcol[TargetRan].GetComponent<Char_Status>().getCS() != GameManager.CharState.Death)
                {
                    CS.SetObjTarget(hitcol[TargetRan].gameObject);
                    break;
                }
                else
                {
                    TargetRan = Random.Range(0, hitcol.Length);

                }
                count++;
            }
        }


        if (InGameSceneManager.Instance.CharAllDeathCheck())
        {
            CD.SetCharStatus(GameManager.CharState.Stay);
        }
        else
        {

            int random = Random.Range(0, 4);
            if (Vector3.Distance(CS.getObjTarget().transform.position, CS.gameObject.transform.position) < 8f)
            {
                switch (random)
                {
                    case 0:
                        CD.SetCharStatus(GameManager.CharState.Skill1);
                        break;
                    case 1:
                        CD.SetCharStatus(GameManager.CharState.Skill2);
                        break;
                    case 2:
                        CD.SetCharStatus(GameManager.CharState.Skill3);
                        break;
                    case 3:
                        CD.SetCharStatus(GameManager.CharState.Skill4);
                        break;
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
