using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{


    //DeleGate, 상속 전략적패턴으로 대체 가능
    public void SetSkill(int _id,int _skillNum, Partner_Dynamics _PD)
    {
        switch ((_id*3)+_skillNum)
        {
            case 0:
                //StartCoroutine(HealerBaseAttackAni(_PD));
                HealerBaseAttack(_PD);
                break;
            case 1:
                Healing(_PD);
                break;
            case 2:
                AllHealing(_PD);
                break;
            case 3:
                HealerBaseAttack(_PD);
                break;
            case 4:
                Healing(_PD);
                break;
            case 5:
                AllHealing(_PD);
                break;

        }
    }


    void HealerBaseAttack(Partner_Dynamics _PD)
    {
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill0", true);

        //원거리 공격 투사체 생성
        GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), AttackPos.position, Quaternion.identity);
        objFireBall.GetComponent<HealerBullet>().SelectTarget(TargetObj);
        objFireBall.GetComponent<HealerBullet>().SetDamage(charstatus.Damage);

        //애니메이션이 끝날때까지 기다리기
        //PD.setDelayAniName("Skill0");
        //PD.SetPartnerStatus(GameManager.CharState.Delay);
        //charstatus.CS = GameManager.CharState.Delay;


        //PD.SetPartnerStatus(GameManager.CharState.Idle);
        //charstatus.CS = GameManager.CharState.Idle;

        //Debug.Log("check");
    }


    // 힐러
    IEnumerator HealerBaseAttackAni(Partner_Dynamics _PD)
    {
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        
        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        animator.SetBool("Move", false);
        

        yield return new WaitForSeconds(PD.getAttackDelayTime());

        animator.SetBool("Skill0", true);
        //Debug.Log("check");

        GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), AttackPos.position, Quaternion.identity);
        objFireBall.GetComponent<HealerBullet>().SelectTarget(TargetObj);
        objFireBall.GetComponent<HealerBullet>().SetDamage(charstatus.Damage);


        //if (PD.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill0")&&
        //    PD.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        //    Debug.Log("check");

        //yield return new WaitForSeconds(PD.getAttackDelayTime());
        //PD.SetPartnerStatus(GameManager.CharState.Idle);


        //버그 원인
        /*
        1. 공격 애니메이션에 시작은 파라미터로 진행된다.
        2. 파라미터로 진행됨에따라 즉각적인 애니메이션의 변환이 진행되지 않는다.
        3. 유한상태머신을 이용하여 1회성으로 기본공격을 사용한다.
        4. 공격 전 선 딜레이 시간과 애니메이션이 끝날때까지의 딜레이시간을 얻기 위해 코루틴을 사용하였다.
        5. while 문을 이용하여 애니메이션이 끝날때까지의 시간을 대기시킨다.
        6. while 문의 실행 시점에서는 파라미터만 변환 되어있고, 애니메이션은 변경되지 않은 상태이다.
        7. 때문에 애니메이션의 normalizedTime을 이용하여 진행상황이 끝났을때 종료시키려하면 공격 애니메이션이 실행되기 전 애니메이션이
        끝이났을때 조건이 걸려서 종료된다.
        8. and(&&)을 이용하여 애니메이션의 이름을 조건으로 넣게될 시 공격 애니메이션 시작 전 실행되어 while문이 무한 반복에 걸린다.
        9. 코루틴은 Update와는 별개의 진행을 하지만 코루틴 속 while문이 Update에서 애니메이션이 바뀌는동안의 과부하가 걸린다.
         */

        //해결 요점
        /*
         1. while 문 실행 시 공격 애니메이션이 실행되도록 한다.
         2. while 문이 실행이 되었을때 공격 애니메이션이 아닐경우 무한반복이 걸리지 않게 한다.
         */

        //해결 방안
        /*
         1. while 문 시작 전 약간의 딜레이를 주어 애니메이션이 변경될 시간을 얻는다.
         2. Has Exit Time의 사용
         */



    }



    void Healing(Partner_Dynamics _PD)
    {
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill1", true);


        charstatus.UseMana(50);
        TargetObj.GetComponent<Char_Status>().m_nPlayerHP += 7;

        PD.setSkill1CoolTimer(3f);
        PD.setSkill1On(false);

    }

    void AllHealing(Partner_Dynamics _PD)
    {

        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill1", true);

        charstatus.UseMana(100);
        TargetObj.GetComponent<Char_Status>().m_nPlayerHP += 7;

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        Collider[] hitcol = Physics.OverlapSphere(PD.transform.position, 30f, m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHP += 7;
            count++;

        }

        PD.setSkill2CoolTimer(10f);
        PD.setSkill2On(false);

    }


    //도적
    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    void ThiefBaseAttack(Partner_Dynamics _PD)
    {
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill0", true);

        //공격 범위
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapBox(AttackPos.transform.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = this.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= 45)
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(charstatus.Damage * 2);
                PD.setCheck01(true);
                //Debug.Log("BackHit");
            }
            else
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(charstatus.Damage);
                PD.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        

    }


    void ThiefFlipOver(Partner_Dynamics _PD)
    {
        //파트너 정보
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);


        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.transform.position, 10.0f, m_nMask);//충돌감지 저장
                                                                                           //int count = 0;

        if (hitcol[0] != null)
        {
            PD.transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4;
        }

        PD.setSkill1CoolTimer(5f);
        PD.setSkill1On(false);

    }


    //void ThiefBackStep()
    //{


    //    if (AttackDelayTimer > AttackDelayTime / 2)
    //    {

    //        AttackDelayTimer -= Time.deltaTime;
    //        Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
    //        transform.LookAt(vecEnemyLookingPoint);
    //        return true;
    //    }
    //    else if (AttackDelayTimer <= AttackDelayTime / 2 && AttackDelayTimer > 0)
    //    {
    //        AttackDelayTimer -= Time.deltaTime;
    //        return true;
    //    }
    //    else
    //    {
    //        //Debug.Log("Check");
    //        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
    //        {
    //            Vector3 PlayerLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
    //            animator.Play("AttackA3");
    //            Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
    //            transform.LookAt(PlayerLookingPoint);

    //            CharStatus.UseMana(50);

    //            int m_nMask = 0;
    //            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
    //            Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position, new Vector3(1, 1, 1),
    //                Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0)), m_nMask);


    //            //Vector3 vecHitCol = new Vector3(hitcol[0].transform.position.x, this.transform.position.y, hitcol[0].transform.position.z);

    //            //Debug.Log(angle);


    //            //Debug.Log(hitcol[0].gameObject);
    //            if (hitcol.Length != 0)
    //            {
    //                Vector3 targetDir = this.transform.position - hitcol[0].transform.position;
    //                float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);
    //                if (angle <= 45)
    //                {
    //                    hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage * 4);
    //                    m_bBackPos = true;
    //                    //Debug.Log("BackHit");
    //                }
    //                else
    //                {
    //                    hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage * 2);
    //                    m_bBackPos = false;
    //                }


    //                //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
    //            }





    //            return true;
    //        }
    //        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA3") &&
    //            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //        {
    //            return true;
    //        }
    //    }

    //    PD.setSkill1CoolTimer(10f);
    //    PD.setSkill1On(false);



    //}



}
