using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{


    //DeleGate, ��� �������������� ��ü ����
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
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // Ÿ�� �ٶ󺸱�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //���� �ִϸ��̼� ���� Ȱ��
        animator.SetBool("Skill0", true);

        //���Ÿ� ���� ����ü ����
        GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), AttackPos.position, Quaternion.identity);
        objFireBall.GetComponent<HealerBullet>().SelectTarget(TargetObj);
        objFireBall.GetComponent<HealerBullet>().SetDamage(charstatus.Damage);

        //�ִϸ��̼��� ���������� ��ٸ���
        //PD.setDelayAniName("Skill0");
        //PD.SetPartnerStatus(GameManager.CharState.Delay);
        //charstatus.CS = GameManager.CharState.Delay;


        //PD.SetPartnerStatus(GameManager.CharState.Idle);
        //charstatus.CS = GameManager.CharState.Idle;

        //Debug.Log("check");
    }


    // ����
    IEnumerator HealerBaseAttackAni(Partner_Dynamics _PD)
    {
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        
        // Ÿ�� �ٶ󺸱�
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


        //���� ����
        /*
        1. ���� �ִϸ��̼ǿ� ������ �Ķ���ͷ� ����ȴ�.
        2. �Ķ���ͷ� ����ʿ����� �ﰢ���� �ִϸ��̼��� ��ȯ�� ������� �ʴ´�.
        3. ���ѻ��¸ӽ��� �̿��Ͽ� 1ȸ������ �⺻������ ����Ѵ�.
        4. ���� �� �� ������ �ð��� �ִϸ��̼��� ������������ �����̽ð��� ��� ���� �ڷ�ƾ�� ����Ͽ���.
        5. while ���� �̿��Ͽ� �ִϸ��̼��� ������������ �ð��� ����Ų��.
        6. while ���� ���� ���������� �Ķ���͸� ��ȯ �Ǿ��ְ�, �ִϸ��̼��� ������� ���� �����̴�.
        7. ������ �ִϸ��̼��� normalizedTime�� �̿��Ͽ� �����Ȳ�� �������� �����Ű���ϸ� ���� �ִϸ��̼��� ����Ǳ� �� �ִϸ��̼���
        ���̳����� ������ �ɷ��� ����ȴ�.
        8. and(&&)�� �̿��Ͽ� �ִϸ��̼��� �̸��� �������� �ְԵ� �� ���� �ִϸ��̼� ���� �� ����Ǿ� while���� ���� �ݺ��� �ɸ���.
        9. �ڷ�ƾ�� Update�ʹ� ������ ������ ������ �ڷ�ƾ �� while���� Update���� �ִϸ��̼��� �ٲ�µ����� �����ϰ� �ɸ���.
         */

        //�ذ� ����
        /*
         1. while �� ���� �� ���� �ִϸ��̼��� ����ǵ��� �Ѵ�.
         2. while ���� ������ �Ǿ����� ���� �ִϸ��̼��� �ƴҰ�� ���ѹݺ��� �ɸ��� �ʰ� �Ѵ�.
         */

        //�ذ� ���
        /*
         1. while �� ���� �� �ణ�� �����̸� �־� �ִϸ��̼��� ����� �ð��� ��´�.
         2. Has Exit Time�� ���
         */



    }



    void Healing(Partner_Dynamics _PD)
    {
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // Ÿ�� �ٶ󺸱�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //���� �ִϸ��̼� ���� Ȱ��
        animator.SetBool("Skill1", true);


        charstatus.UseMana(50);
        TargetObj.GetComponent<Char_Status>().m_nPlayerHP += 7;

        PD.setSkill1CoolTimer(3f);
        PD.setSkill1On(false);

    }

    void AllHealing(Partner_Dynamics _PD)
    {

        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // Ÿ�� �ٶ󺸱�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //���� �ִϸ��̼� ���� Ȱ��
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


    //����
    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    void ThiefBaseAttack(Partner_Dynamics _PD)
    {
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // Ÿ�� �ٶ󺸱�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);

        //���� �ִϸ��̼� ���� Ȱ��
        animator.SetBool("Skill0", true);

        //���� ����
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
        //��Ʈ�� ����
        Partner_Dynamics PD = _PD;
        Animator animator = PD.getAnimator();
        Char_Status charstatus = PD.getCharStatus();
        GameObject TargetObj = PD.getObjTarget();
        Transform AttackPos = PD.getAttackPos();

        // Ÿ�� �ٶ󺸱�
        Vector3 vecEnemyLookingPoint = new Vector3(TargetObj.transform.position.x, PD.transform.position.y, TargetObj.transform.position.z);
        PD.transform.LookAt(vecEnemyLookingPoint);


        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(PD.transform.position, 10.0f, m_nMask);//�浹���� ����
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
