using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player_Ctrl : MonoBehaviour
{
    ////public GameManager.PlayerState PS;

    //Animator animator;

    //public Vector3 PlayerLookingPoint;

    ////float Timer = 0;

    //GameObject objMeleeAttackPoint;


    //public int m_nPlayerShieldPoint = 30;
    //public int m_nPlayerShieldPointMax = 30;
    //public float m_fShieldChargeTimer = 5;
    //public bool m_bGuardMoving = false;

    ////float m_fShieldRushTimer = 1;

    //Char_Status CharStatus;

    //public bool m_bRushOn = false;
    //Vector3 vecRushStartpos = Vector3.zero;
    //float m_fRustDist;

    //ParticleSystem ProtectZoneEffect;

    //bool m_bPlayerDie = false;


    //public float m_fQSkillCoolTimer = 0;
    //public float m_fWSkillCoolTimer = 0;
    //public float m_fESkillCoolTimer = 0;
    //public float m_fRSkillCoolTimer = 0;

    //public bool m_bQSkillOn=false;
    //public bool m_bWSkillOn =false;
    //public bool m_bESkillOn =false;
    //public bool m_bRSkillOn =false;



    //// Start is called before the first frame update
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
        
    //    objMeleeAttackPoint = this.transform.GetChild(2).gameObject;
    //    CharStatus = this.GetComponent<Char_Status>();
    //    CharStatus.CS = GameManager.CharState.Idle;
    //    ProtectZoneEffect = this.gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
    //    CharStatus.m_nPlayerAumor = 5;
    //    CharStatus.setPlayerStatus(25,100,4,4,5);
    //    m_nPlayerShieldPoint = m_nPlayerShieldPointMax;
    //}

    ///*
    //public void GetPlayerLP(Vector3 _MBPoint)
    //{
    //    float Remainder = 0.5f - _MBPoint.y;
    //    PlayerLookingPoint = new Vector3(_MBPoint.x, _MBPoint.y + Remainder, _MBPoint.z);
    //}
    //*/




    //public void GetPlayerLP(Vector3 _MBPoint)
    //{
    //    PlayerLookingPoint = _MBPoint;
    //}

    //bool Moving()
    //{

    //    float dis = Vector3.Distance(transform.position, PlayerLookingPoint);
    //    if (dis >= 0.02f)
    //    {
    //        animator.Play("Run_SwordShield");
    //        transform.LookAt(PlayerLookingPoint);

    //        transform.position = Vector3.MoveTowards(transform.position, PlayerLookingPoint, CharStatus.m_fPlayerSpeed * Time.deltaTime);

    //        return true;
    //    }
    //    // animator.Play("Idle_SwordShield");
    //    CharStatus.CS = GameManager.CharState.Idle;
    //    return false;
    //}


    ////private void OnDrawGizmos()
    ////{
    ////    //Gizmos.color = Color.red;
    ////    Vector3 AttackArea = PlayerLookingPoint - this.transform.position;

    ////    //Gizmos.DrawWireCube(this.transform.position + AttackArea.normalized, new Vector3(1, 1, 1),Quaternion.Euler());
    ////}


    //float GetAngle(Vector3 start, Vector3 end)
    //{
    //    Vector3 v2 = end - start;
    //    return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    //}

    //bool Attack()
    //{

    //    //Debug.Log("Check");
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_SwordShield") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run_SwordShield"))
    //    {
    //        Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
    //        transform.LookAt(PlayerLookingPoint);
    //        animator.Play("NormalAttack01_SwordShield");


    //        int m_nMask = 0;
    //        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
    //        Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position, new Vector3(1, 1, 1),
    //            Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0)), m_nMask);

    //        //Debug.Log(hitcol[0].gameObject);
    //        if (hitcol.Length != 0)
    //        {
    //            hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage);
    //            //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
    //        }

    //        return true;
    //    }
    //    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack01_SwordShield") &&
    //        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //    {
    //        return true;
    //    }

    //    //Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
    //    //AttackRange.transform.position = transform.position + AttackArea.normalized+new Vector3(0,0.5f,0);
    //    //AttackRange.transform.rotation = Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0));


    //    //animator.Play("Idle_SwordShield");
    //    CharStatus.CS = GameManager.CharState.Idle;
    //    return false;

    //}


    //bool ShieldBash()
    //{


       
    //        m_bQSkillOn = false;

    //        //Debug.Log("Check");
    //        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_SwordShield") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run_SwordShield"))
    //        {
    //            // AttackArea = PlayerLookingPoint - this.transform.position;
    //            transform.LookAt(PlayerLookingPoint);
    //            animator.Play("NormalAttack02_SwordShield");



    //            int m_nMask = 0;
    //            m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
    //            Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position, new Vector3(1, 1, 1),
    //                Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0)), m_nMask);

    //            //Debug.Log(hitcol[0].gameObject);
    //            if (hitcol.Length != 0)
    //            {
    //                hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage((int)(m_nPlayerShieldPoint/3));
    //                //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
    //            }

    //            return true;
    //        }
    //        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack02_SwordShield") &&
    //            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //        {
    //            return true;
    //        }


    //        m_fQSkillCoolTimer = 5f;


    //    //Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
    //    //AttackRange.transform.position = transform.position + AttackArea.normalized+new Vector3(0,0.5f,0);
    //    //AttackRange.transform.rotation = Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0));


    //    //animator.Play("Idle_SwordShield");
    //    CharStatus.CS = GameManager.CharState.Idle;
        
    //    return false;

    //}


    //bool ShieldRush()
    //{
        

            
    //        if (!m_bRushOn)
    //        {
    //            this.transform.rotation = Quaternion.Euler(0, GetAngle(transform.position, PlayerLookingPoint), 0);
    //            m_bRushOn = true;
    //            vecRushStartpos = this.transform.position;

    //        }

    //        m_fRustDist = Vector3.Distance(transform.position, vecRushStartpos);
    //        if (m_fRustDist < 30)
    //        {
    //            animator.Play("Sprint_SwordShield");

    //            //transform.LookAt(PlayerLookingPoint);

    //            //m_fShieldRushTimer -= Time.deltaTime;

    //            this.transform.Translate(Vector3.forward * CharStatus.m_fPlayerSpeed * 3.5f * Time.deltaTime);

    //            return true;
    //        }

    //        m_bWSkillOn = false;
    //        m_fWSkillCoolTimer = 7f;



    //    CharStatus.CS = GameManager.CharState.Idle;
    //    //m_fShieldRushTimer = 1f;
        
    //    m_bRushOn = false;
    //    return false;
    //}

    //void ProtectZone()
    //{
        

    //        ProtectZoneEffect.Play();
    //        int m_nMask = 0;
    //        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
    //        Collider[] hitcol = Physics.OverlapSphere(transform.position, 10f, m_nMask);
    //        int count = 0;

    //        while (count < hitcol.Length)
    //        {
    //            hitcol[count].gameObject.GetComponent<Char_Status>().onProtectBuff();
    //            count++;
    //        }



    //        m_fESkillCoolTimer = 15f;
    //        m_bESkillOn = false;

    //    CharStatus.CS = GameManager.CharState.Idle;
        
    //}


    //void Taunt()
    //{
        

    //        if (GameManager.instance.MBTarget.gameObject.layer == 8)
    //        {
    //            GameManager.instance.MBTarget.gameObject.GetComponent<Enemy_Ctrl>().objTarget = this.gameObject;
    //            Debug.Log("TauntTarget : " + GameManager.instance.MBTarget);
    //        }


    //        m_fRSkillCoolTimer = 5f;
    //        m_bRSkillOn = false;

    //    CharStatus.CS = GameManager.CharState.Idle;
       
    //}



    //bool OnShield()
    //{
    //    animator.Play("Walk_SwordShield");
    //    if (m_bGuardMoving)
    //    {
    //        float dis = Vector3.Distance(transform.position, PlayerLookingPoint);
    //        if (dis >= 0.02f)
    //        {

    //            transform.position = Vector3.MoveTowards(transform.position, PlayerLookingPoint, CharStatus.m_fPlayerSpeed / 2 * Time.deltaTime);

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




    //bool Hit()
    //{
    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield"))
    //    {

    //        animator.Play("GetHit_SwordShield");

    //        return true;
    //    }
    //    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit_SwordShield") &&
    //        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //    {

    //        return true;
    //    }

    //    CharStatus.CS = GameManager.CharState.Idle;
    //    return false;
    //}



    //void SkillCoolTimer()
    //{
    //    if (m_fQSkillCoolTimer<=0)
    //    {
    //        m_bQSkillOn = true;
    //    }
    //    else
    //    {
    //        m_fQSkillCoolTimer-=Time.deltaTime;
    //    }

    //    if (m_fWSkillCoolTimer <= 0)
    //    {
    //        m_bWSkillOn = true;
    //    }
    //    else
    //    {
    //        m_fWSkillCoolTimer -= Time.deltaTime;
    //    }

    //    if (m_fESkillCoolTimer <= 0)
    //    {
    //        m_bESkillOn = true;
    //    }
    //    else
    //    {
    //        m_fESkillCoolTimer -= Time.deltaTime;
    //    }

    //    if (m_fRSkillCoolTimer <= 0)
    //    {
    //        m_bRSkillOn = true;
    //    }
    //    else
    //    {
    //        m_fRSkillCoolTimer -= Time.deltaTime;
    //    }
    //}




    //// Update is called once per frame
    //void Update()
    //{

    //    SkillCoolTimer();

    //    switch (CharStatus.CS)
    //    {
    //        case GameManager.CharState.Idle:
    //            animator.Play("Idle_SwordShield");
    //            break;
    //        case GameManager.CharState.Move:
    //            Moving();
    //            break;
    //        case GameManager.CharState.Attack:
    //            Attack();
    //            break;
    //        case GameManager.CharState.Skill0:
    //            OnShield();
    //            break;
    //        case GameManager.CharState.Skill1:
    //            ShieldBash();
    //            break;
    //        case GameManager.CharState.Skill2:
    //            ShieldRush();
    //            break;
    //        case GameManager.CharState.Skill3:
    //            ProtectZone();
    //            break;
    //        case GameManager.CharState.Skill4:
    //            Taunt();
    //            break;
    //        case GameManager.CharState.Hit:
    //            Hit();
    //            break;
    //        case GameManager.CharState.Death:
    //            animator.Play("Die_SwordShield");
    //            this.GetComponent<Rigidbody>().useGravity = false;
    //            this.GetComponent<CapsuleCollider>().isTrigger = true;
    //            m_bPlayerDie = true;
    //            break;

    //    }

    //    if (CharStatus.m_nPlayerHP<=0)
    //    {
    //        CharStatus.CS = GameManager.CharState.Death;
    //    }

    //    if (m_bPlayerDie && animator.GetCurrentAnimatorStateInfo(0).IsName("Die_SwordShield") &&
    //        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
    //    {
    //        //Destroy(this.gameObject, 3f);
    //        m_bPlayerDie = false;
    //    }

        

    //    if (m_nPlayerShieldPoint < m_nPlayerShieldPointMax)
    //    {
    //        if (m_fShieldChargeTimer <= 0)
    //        {
    //            m_nPlayerShieldPoint+=3;
    //            m_fShieldChargeTimer = 5;
    //        }
    //        else
    //        {
    //            m_fShieldChargeTimer -= Time.deltaTime;
    //        }
    //    }


    //}


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 8)
    //    {
    //        //animator.Play("Idle_SwordShield");
    //        if (CharStatus.CS == GameManager.CharState.Skill2)
    //        {
    //            collision.gameObject.GetComponent<Enemy_Ctrl>().GetDamage((int)(m_nPlayerShieldPoint*((m_fRustDist/30)/2)));
    //            Debug.Log("EnemyHP : "+collision.gameObject.GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
    //            Debug.Log("ShieldRush Damage : "+(int)(m_nPlayerShieldPoint * ((m_fRustDist / 30)/2))+", Dist : "+ m_fRustDist+"/30");
    //        }
    //        CharStatus.CS = GameManager.CharState.Idle;
    //    }
    //}
}
