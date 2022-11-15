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
            case 0:
                HealerBaseAttack(_CS);
                break;
            case 1:
                Healing(_CS);
                break;
            case 2:
                AllHealing(_CS);
                break;
            case 3:
                ThiefBaseAttack(_CS);
                break;
            case 4:
                ThiefFlipOver(_CS);
                break;
            case 5:
                ThiefBackStep(_CS);
                break;
            case 6:
                KnightBaseAttack(_CS);
                break;
            case 7:
                KnightShieldBash(_CS);
                break;
            case 8:
                KnightShieldRush(_CS);
                break;
            case 9:
                ProtectZone(_CS);
                break;
            case 10:
                Taunt(_CS);
                break;
        }
    }



    //힐러
    void HealerBaseAttack(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Attack", true);

        //원거리 공격 투사체 생성
        GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), AttackPos.position, Quaternion.identity);
        objFireBall.GetComponent<HealerBullet>().SelectTarget(TargetObj);
        objFireBall.GetComponent<HealerBullet>().SetDamage(CS.getATK());

        //애니메이션이 끝날때까지 기다리기
        //PD.setDelayAniName("Attack");
        //PD.SetPartnerStatus(GameManager.CharState.Delay);
        //charstatus.CS = GameManager.CharState.Delay;


        //PD.SetPartnerStatus(GameManager.CharState.Idle);
        //charstatus.CS = GameManager.CharState.Idle;

        //Debug.Log("check");
    }



    void Healing(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill1", true);


        CS.UseMana(50);
        TargetObj.GetComponent<Char_Status>().HealingHP(10);

        CS.setSkill1CoolTimer(3f);
        CS.setSkill1On(false);

    }

    void AllHealing(Char_Status _CS)
    {

        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill1", true);

        CS.UseMana(100);
        TargetObj.GetComponent<Char_Status>().HealingHP(10);

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        Collider[] hitcol = Physics.OverlapSphere(CS.transform.position, 30f, m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().HealingHP(10);
            count++;

        }

        CS.setSkill2CoolTimer(10f);
        CS.setSkill2On(false);

    }


    //도적
    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    void ThiefBaseAttack(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Attack", true);

        //공격 범위
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= 45)
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CS.getATK() * 2);
                CS.setCheck01(true);
                //Debug.Log("BackHit");
            }
            else
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CS.getATK());
                CS.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        

    }


    void ThiefFlipOver(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();


        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        

        animator.SetBool("Skill1", true);
        CS.UseMana(40);

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(CS.transform.position, 10.0f, m_nMask);//충돌감지 저장
                                                                                           //int count = 0;

        if (hitcol[0] != null)
        {
            CS.transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4;
        }

        // 타깃 바라보기
        CS.transform.LookAt(vecEnemyLookingPoint);


        CS.setSkill1CoolTimer(5f);
        CS.setSkill1On(false);

    }


    void ThiefBackStep(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        animator.SetBool("Skill2", true);
        CS.UseMana(50);


        //공격 범위
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= 45)
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CS.getATK() * 4);
                CS.setCheck01(true);
                //Debug.Log("BackHit");
            }
            else
            {
                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CS.getATK()*2);
                CS.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        CS.setSkill1CoolTimer(10f);
        CS.setSkill1On(false);


    }


    void KnightBaseAttack(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Attack", true);

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CS.getATK());
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


    }


    void KnightShieldBash(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill1", true);


        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getIdentityPoint() / 3));
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }

        CS.setSkill1CoolTimer(5f);
        CS.setSkill1On(false);

        

    }


    void KnightShieldRush(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

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
            //공격 애니메이션 조건 활성
            animator.SetBool("Skill2", true);


            float m_fRustDist = Vector3.Distance(CS.gameObject.transform.position, CD.getStartPos());


            if (m_fRustDist < 30)
            {

                CS.gameObject.transform.Translate(Vector3.forward * CS.getSpeed() * 3.5f * Time.deltaTime);

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
                Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 1f, m_nMask);

                if (hitcol.Length != 0)
                {
                    hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(CS.getIdentityPoint() * ((m_fRustDist / 30) / 2)));
                    CS.setSkill2Using(false);
                    CS.setSkill2CoolTimer(7f);
                    CS.setSkill2On(false);
                    CD.SetCharStatus(GameManager.CharState.Idle);
                }


            }
            else
            {
                CS.setSkill2Using(false);
                CS.setSkill2CoolTimer(7f);
                CS.setSkill2On(false);
                CD.SetCharStatus(GameManager.CharState.Idle);
            }

        }









    }

    void ProtectZone(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill3", true);

        GameObject objProtectZone = Instantiate(Resources.Load<GameObject>("Prefabs/Protectzone"), CS.gameObject.transform.position, Quaternion.identity);
        objProtectZone.transform.parent = CS.gameObject.transform;
        objProtectZone.GetComponent<ParticleSystem>().Play();
        Destroy(objProtectZone,13f);

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 10f, m_nMask);
        int count = 0;

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().onProtectBuff();
            count++;
        }


        CS.setSkill3CoolTimer(15f);
        CS.setSkill3On(false);

    }


    void Taunt(Char_Status _CS)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        CS.transform.LookAt(vecEnemyLookingPoint);

        //공격 애니메이션 조건 활성
        animator.SetBool("Skill4", true);

        if (GameManager.instance.MBTarget.gameObject.layer == 8)
        {
            GameManager.instance.MBTarget.gameObject.GetComponent<Enemy_Ctrl>().objTarget = CS.gameObject;
            //Debug.Log("TauntTarget : " + GameManager.instance.MBTarget);
            CS.setSkill4CoolTimer(5f);
            CS.setSkill4On(false);
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
