using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    int SetTargetLayerMask(string _Target)
    {
        switch (_Target)
        {
            case "Ally":
                return 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")); 
                break;
            case "Enemy":
                return 1 << (LayerMask.NameToLayer("Enemy"));
                break;
            default:
                return 0;
                break;
        }
    }


    //DeleGate, 상속, 전략적패턴으로 대체 가능
    public void SetCharSkill(int _id, Char_Status _CS)
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
                StartCoroutine(MeleeAttack(_CS, _id));
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
                TargetTaunt(_CS, _id);
                break;
            case 11:
                StartCoroutine(MeleeTargetAttack(_CS, _id));
                break;
            case 12:
                StartCoroutine(RangeAngleAttack(_CS, _id));
                break;
            case 13:
                SingleTargetBullet(_CS, _id);
                break;
            case 14:
                FireBreath(_CS, _id);
                break;
            case 15:
                OnShield(_CS, _id);
                break;
            case 16:
                SingleTargetBullet(_CS, _id);
                break;
            case 17:
                FireBreath(_CS, _id);
                break;
            case 18:
                StartCoroutine(RangeAngleAttack(_CS, _id));
                break;
            case 19:
                StartCoroutine(RangeAngleAttack(_CS, _id));
                break;
            case 20:
                StartCoroutine(RangeAngleAttack(_CS, _id));
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
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);

        bool enemyCheck = false;
        if (CS.getTYP() == LayerMask.NameToLayer("Player") || CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            enemyCheck = false;
        }
        else
        {
            enemyCheck = true;
        }

        if (CS.getObjTarget() != null || CS.getObjTarget().activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<Bullet>().Setting(TargetObj, (int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(), enemyCheck, SkillDB.getSkillUsingTime());
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
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
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
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);



        TargetObj.GetComponent<Char_Status>().HealingHP((int)(CS.getMPMax() * SkillDB.getSkillCeofficientPer1()));

        int m_nMask = 0;
        //m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
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
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);


        //공격 범위
        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= SkillDB.getSkillRange1())
            {
                //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()  *SkillDB.getSkillCeofficientPer2()));
                hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                CS.setCheck01(true);// 백어택 판정으로 사용
                //Debug.Log("BackHit");
            }
            else
            {
                //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
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
        SkillData SkillDB = DBManager.SkillData[SkillID];

        
        


        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapSphere(CS.transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
                                                                                           //int count = 0;

        if (hitcol[0] != null)
        {
            CS.transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4.5f;
        }

        // 타깃 바라보기
        CS.transform.LookAt(new Vector3(hitcol[0].transform.position.x,CS.transform.position.y,hitcol[0].transform.position.z));


    }


    void BackStep(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        GameObject TargetObj = CS.getObjTarget();
        Transform AttackPos = CS.getAttackPos();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);



        //공격 범위
        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        if (hitcol.Length != 0)
        {
            Vector3 targetDir = CS.transform.position - hitcol[0].transform.position;
            float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

            if (angle <= SkillDB.getSkillRange1())
            {
                //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1() * SkillDB.getSkillCeofficientPer2()));
                CS.setCheck01(true);
                //Debug.Log("BackHit");
            }
            else
            {
                //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                CS.setCheck01(false);
            }


            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }




    }


    IEnumerator MeleeAttack(Char_Status _CS, int SkillID)
    {

        
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];




        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);

        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
            hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


    }


    void ShieldBash(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);




        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(1, 1, 1),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getDEF() * SkillDB.getSkillCeofficientPer1()));
            hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getDEF() * SkillDB.getSkillCeofficientPer1()));
            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
        }


        

    }


    void ShieldRush(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();



        int i = 0;
        while (i < CS.getSkillID().Length)
        {
            if (SkillID == CS.getSkillID()[i])
            {
                break;
            }
            i++;
        }
        

        if (!CS.getSkillUsing()[i])
        {
            // 타깃 바라보기
            Vector3 vecEnemyLookingPoint;
            
            if (CS.getTYP() == LayerMask.NameToLayer("Player"))
            {
                vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
            }
            else
            {
                vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
            }
            CS.transform.LookAt(vecEnemyLookingPoint);

            CS.SetSuperArmor(true);
            CD.setStartPos();
            CS.setSkillUsing(i,true);
        }
        else
        {


            float m_fRustDist = Vector3.Distance(CS.gameObject.transform.position, CD.getStartPos());


            if (m_fRustDist < 30)
            {

                CS.gameObject.transform.Translate(Vector3.forward * CS.getSpeed() * 3.5f * Time.deltaTime);

                int m_nMask = 0;
                m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
                Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, 1f, m_nMask);

                if (hitcol.Length != 0)
                {
                    //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getDEF() * ((m_fRustDist / 30) * SkillDB.getSkillCeofficientPer1())));
                    hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getDEF() * ((m_fRustDist / 30) * SkillDB.getSkillCeofficientPer1())));
                    CS.setSkillUsing(i, false);
                    CS.SetSuperArmor(false);
                    //CD.SetCharStatus(GameManager.CharState.Idle);
                }


            }
            else
            {
                CS.setSkillUsing(i, false);
                CS.SetSuperArmor(false);
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
        GameObject TargetObj = CS.getObjTarget();


        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);



        GameObject objProtectZone = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), CS.gameObject.transform.position, Quaternion.identity);
        objProtectZone.transform.parent = CS.gameObject.transform;
        objProtectZone.GetComponent<ParticleSystem>().Play();
        Destroy(objProtectZone, SkillDB.getSkillUsingTime()+3f);

        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, SkillDB.getSkillRange1(), m_nMask);
        int count = 0;


        CS.OnDrBuff(SkillDB.getSkillUsingTime(), SkillDB.getSkillCeofficientPer1());

        while (count < hitcol.Length)
        {
            hitcol[count].gameObject.GetComponent<Char_Status>().OnDrBuff(SkillDB.getSkillUsingTime(), SkillDB.getSkillCeofficientPer1());

            count++;
        }


    }


    void TargetTaunt(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();


        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);

        if (CS.getObjTarget()!=null)
        {
            if (CS.getTYP() == LayerMask.NameToLayer("Player") || CS.getTYP() == LayerMask.NameToLayer("Partner"))
            {
                if (CS.getObjTarget().layer == 8)
                {
                    CS.getObjTarget().GetComponent<Char_Status>().OnTaunt(5f, CS.gameObject);
                    //Debug.Log("TauntTarget : " + GameManager.instance.MBTarget);
                }
            }
            else
            {
                if (CS.getObjTarget().layer == 6 || CS.getObjTarget().layer == 9)
                {
                    CS.getObjTarget().GetComponent<Char_Status>().OnTaunt(5f, CS.gameObject);
                    //Debug.Log("TauntTarget : " + GameManager.instance.MBTarget);
                }
            }
        }
        

        


    }



    void OnShield(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();


        float dis = Vector3.Distance(CS.transform.position, CD.getMovePoint());
        

        if (!CS.getIdentitySkillUsing())
        {

            // 타깃 바라보기
            Vector3 vecEnemyLookingPoint;

            if (CS.getTYP() == LayerMask.NameToLayer("Player"))
            {
                vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
            }
            else
            {
                vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
            }
            CS.transform.LookAt(vecEnemyLookingPoint);


            if (CS.getIdentityPoint() > 0)
            {
                CS.delGetDamae = CS.UseIdentitiy;
                CS.setIdentitySkillUsing(true);
                //변동확인hp = 지금HP
            }
            else
            {
                CS.delGetDamae = CS.GetDamage; 
                CS.setIdentitySkillUsing(false);
                CD.SetCharStatus(GameManager.CharState.Idle);
            }
            
        }
        else
        {
            dis = Vector3.Distance(CS.transform.position, CD.getMovePoint());

            //if(변동확인hp < 지금HP)

            //체력이 맞아서 줄고 그체력을 다시 회복시키고 고유자원을 깐다

            if (CS.getIdentityPoint() <= 0)
            {
                CS.delGetDamae = CS.GetDamage;
                CS.setIdentitySkillUsing(false);
            }
            

            if (dis >= 0.02f)
            {

                CS.transform.position = Vector3.MoveTowards(CS.transform.position, CD.PlayerLookingPoint(), CS.getSpeed()*SkillDB.getSkillCeofficientPer1() * Time.deltaTime);

            }

            

            
        }
    }



    IEnumerator MeleeTargetAttack(Char_Status _CS, int SkillID)//vector 크기만다르다
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();


        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position,new Vector3(3, 3, 3),
            Quaternion.Euler(new Vector3(0, GetAngle(CS.gameObject.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {
            //Debug.Log(hitcol[0].gameObject);
            //hitcol[0].GetComponent<Char_Status>().GetDamage((int)(CS.getATK()*SkillDB.getSkillCeofficientPer1()));
            hitcol[0].GetComponent<Char_Status>().delGetDamae((int)(CS.getATK()*SkillDB.getSkillCeofficientPer1()));

        }

    }





    IEnumerator RangeAngleAttack(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();


        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);


        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        int m_nMask = 0;
        m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
        Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
        int count = 0;
        //int i = 0;
        while (count < hitcol.Length)
        {

            Vector3 targetDir = hitcol[count].transform.position - CS.gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, CS.gameObject.transform.forward);
            if (angle <= SkillDB.getSkillRange2())
            {
                //Debug.Log(angle+""+ hitcol[count].gameObject.name);
                //hitcol[count].gameObject.GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
                hitcol[count].gameObject.GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
            }

            count++;
        }


    }

    void FireBall(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);

        bool enemyCheck = false;
        if (CS.getTYP() == LayerMask.NameToLayer("Player") || CS.getTYP() == LayerMask.NameToLayer("Partner"))
        {
            enemyCheck = false;
        }
        else
        {
            enemyCheck = true;
        }

        if (CS.getObjTarget() != null || CS.getObjTarget().activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<FireBall>().Setting(TargetObj, (int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(), enemyCheck, SkillDB.getSkillUsingTime());
        }


    }


    //IEnumerator FireBreath(Char_Status _CS, int SkillID)
    //{
    //    //파트너 정보
    //    Char_Status CS = _CS;
    //    Animator animator = CS.getAnimator();
    //    Transform AttackPos = CS.getAttackPos();
    //    GameObject TargetObj = CS.getObjTarget();


    //    //스킬 정보
    //    SkillData SkillDB = DBManager.SkillData[SkillID];

    //    // 타깃 바라보기
    //    Vector3 vecEnemyLookingPoint;
    //    Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
    //    Vector3 MousePoint = CD.getMovePoint();
    //    if (CS.getTYP() == 6)
    //    {
    //        vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
    //    }
    //    else
    //    {
    //        vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
    //    }
    //    CS.transform.LookAt(vecEnemyLookingPoint);



    //    GameObject FireBreathEffect = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity, AttackPos);
    //    FireBreathEffect.transform.parent = AttackPos;
    //    FireBreathEffect.transform.localPosition = Vector3.zero;
    //    FireBreathEffect.transform.localRotation = Quaternion.identity;
        

    //    FireBreathEffect.GetComponent<ParticleSystem>().Play();
    //    Destroy(FireBreathEffect, SkillDB.getSkillUsingTime() + 3f);

    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

    //    int m_nMask = 0;
    //    m_nMask = SetTargetLayerMask(SkillDB.getTargetSelect());
    //    Collider[] hitcol = Physics.OverlapSphere(CS.gameObject.transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
    //    int count = 0;
    //    //int i = 0;
    //    while (count < hitcol.Length)
    //    {

    //        Vector3 targetDir = hitcol[count].transform.position - CS.gameObject.transform.position;
    //        float angle = Vector3.Angle(targetDir, CS.gameObject.transform.forward);

    //        if (angle <= SkillDB.getSkillRange2())
    //        {
    //            //Debug.Log(angle + "" + hitcol[count].gameObject.name);
    //            //hitcol[count].gameObject.GetComponent<Char_Status>().GetDamage((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
    //            hitcol[count].gameObject.GetComponent<Char_Status>().delGetDamae((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()));
    //        }

    //        count++;
    //    }

       
    //}

    void FireBreath(Char_Status _CS, int SkillID)
    {
        //파트너 정보
        Char_Status CS = _CS;
        Animator animator = CS.getAnimator();
        Transform AttackPos = CS.getAttackPos();
        GameObject TargetObj = CS.getObjTarget();

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[SkillID];

        // 타깃 바라보기
        Vector3 vecEnemyLookingPoint;
        Char_Dynamics CD = CS.gameObject.GetComponent<Char_Dynamics>();
        Vector3 MousePoint = CD.getMovePoint();
        if (CS.getTYP() == LayerMask.NameToLayer("Player"))
        {
            vecEnemyLookingPoint = new Vector3(MousePoint.x, CS.transform.position.y, MousePoint.z);
        }
        else
        {
            vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, CS.transform.position.y, TargetObj.transform.position.z);
        }
        CS.transform.LookAt(vecEnemyLookingPoint);


        if (CS.getObjTarget() != null || CS.getObjTarget().activeSelf == false)
        {

            GameObject FireBreathEffect = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity, AttackPos);
            FireBreathEffect.transform.parent = AttackPos;
            FireBreathEffect.transform.localPosition = Vector3.zero;
            FireBreathEffect.transform.localRotation = Quaternion.identity;
            FireBreathEffect.GetComponent<ParticleSystem>().Play();
            FireBreathEffect.GetComponent<FireBreath>().Setting((int)(CS.getATK() * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(),
                SkillDB.getSkillRange1(),SetTargetLayerMask(SkillDB.getTargetSelect()),SkillDB.getSkillRange2());

        }

    }




}
