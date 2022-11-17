using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{


    //DeleGate, 상속, 전략적패턴으로 대체 가능
    public void SetPartnerSkill(int _id, Char_Status _CS)
    {
        switch (_id)
        {
            case 0: // HealerBaseAttack
                SingleTargetBullet(_CS, _id);
                break;
            case 1://Healing
                TargetHealing(_CS,_id);
                break;
            case 2:
                AllHealing(_CS, _id);
                break;
            case 3:
                MeleeAttack_BackAttackOn(_CS, _id);
                break;
            case 4:
                FlipOver(_CS, _id);
                break;
            case 5:
                BackStep(_CS, _id);
                break;
            case 6:
                MeleeAttack(_CS, _id);
                break;
            case 7:
                ShieldBash(_CS, _id);
                break;
            case 8:
                ShieldRush(_CS, _id);
                break;
            case 9:
                ProtectZone(_CS, _id);
                break;
            case 10:
                Taunt(_CS, _id);
                break;
        }
    }


    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }


    void SingleTargetBullet(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Attack", true);

        //원거리 공격 투사체 생성
        //GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), AttackPos.position, Quaternion.identity);
        GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
        if (CS.gameObject.layer ==6 || CS.gameObject.layer == 9)
        {
            objFireBall.GetComponent<HealerBullet>().Setting(TargetObj, (int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(),false, SkillDB.getSkillUsingTime());
        }
        else
        {
            objFireBall.GetComponent<HealerBullet>().Setting(TargetObj, (int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(), true, SkillDB.getSkillUsingTime());
        }
           

        
    }



    void TargetHealing(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);




        TargetObj.GetComponent<Char_Status>().HealingHP((int)(CS.getMPMax() * SkillDB.getSkillCeofficientPer1()));


    }

    void AllHealing(Char_Status _CS, int SkillID)
    {

        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();


        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);



        TargetObj.GetComponent<Char_Status>().HealingHP((int)(CS.getMPMax() * SkillDB.getSkillCeofficientPer1()));

        int m_nMask = 0;
        //m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapSphere(CS.transform.position, SkillDB.getSkillRange1(), m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().HealingHP((int)(CS.getMPMax() * SkillDB.getSkillCeofficientPer1()));
            count++;

        }


    }


   

    void MeleeAttack_BackAttackOn(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);


        //공격 범위
        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= 45)
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()  *SkillDB.getSkillCeofficientPer2()));
                CS.setCheck01(true);// 백어택 판정으로 사용
                //Debug.Log("BackHit");
            }
            else
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                CS.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        

    }


    void FlipOver(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();


        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        


        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapSphere(CS.transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
                                                                                           //int count = 0;

        if (hitcol[0] != null)
        {
            CS.transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4.5f;
        }

        // 타깃 바라보기
        CS.transform.LookAt(vecEnemyLookingPoint);


    }


    void BackStep(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);



        //공격 범위
        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= 45)
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                CS.setCheck01(true);
                //Debug.Log("BackHit");
            }
            else
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                CS.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }




    }


    void MeleeAttack(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);



        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


    }


    void ShieldBash(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);




        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getDEF() * SkillDB.getSkillCeofficientPer1()));
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        

    }


    void ShieldRush(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        

        if (!CS.getSkill2Using())
        {
            // 타깃 바라보기
            Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
            CS.transform.LookAt(vecEnemyLookingPoint);

            CD.setStartPos();
            CS.setSkill2Using(true);
        }
        else
        {


            float m_fRustDist = Vector3.Distance(CS.gameObject.transform.position, CD.getStartPos());


            if (m_fRustDist < 30)
            {

                CS.gameObject.transform.Translate(Vector3.forward * CS.getSpeed() * 3.5f * Time.deltaTime);

                int m_nMask = 0;
                m_nMask = SkillDB.getTargetSelect();
                Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 1f, m_nMask);

                if (hitcol.Length != 0)
                {
                    hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getDEF() * ((m_fRustDist / 30) * SkillDB.getSkillCeofficientPer1())));
                    CS.setSkill2Using(false);

                    //CD.SetCharStatus(GameManager.CharState.Idle);
                }


            }
            else
            {
                CS.setSkill2Using(false);

                //CD.SetCharStatus(GameManager.CharState.Idle);
            }

        }









    }

    void ProtectZone(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);



        GameObject objProtectZone = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), CS.gameObject.transform.position, Quaternion.identity);
        objProtectZone.transform.parent = CS.gameObject.transform;
        objProtectZone.GetComponent<ParticleSystem>().Play();
        Destroy(objProtectZone, SkillDB.getSkillUsingTime()+3f);

        int m_nMask = 0;
        m_nMask = SkillDB.getTargetSelect();
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().onProtectBuff((int)SkillDB.getSkillCeofficientPer1(), SkillDB.getSkillUsingTime());
            count++;
        }


    }


    void Taunt(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        //스킬 정보
        SkillData SkillDB = CharDataBase.instance.m_lSkillDB[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);



        if (GameManager.instance.MBTarget.gameObject.layer == 8)
        {
            GameManager.instance.MBTarget.gameObject.GetComponent<Enemy_Ctrl>().objTarget = CS.gameObject;
            //Debug.Log("TauntTarget : " + GameManager.instance.MBTarget);
        }


    }



    //bool OnShield()
    //{
    //    animator.Play("Walk_SwordShield");
    //    if (m_bGuardMoving)
    //    {
    //        float dis = Vector3.Distance(transform.position, PlayerLookingPoint);
    //        if (dis >= 0.02f)
    //        {

    //            transform.position = Vector3.MoveTowards(transform.position, PlayerLookingPoint, CharStatus.getSpeed() / 2 * Time.deltaTime);

    //            return true;
    //        }
    //        m_bGuardMoving = false;
    //        return true;
    //        //animator.Play("Idle_SwordShield");
    //    }
    //    else
    //    {
    //        return true;
    //    }


    //    return false;
    //}







}
