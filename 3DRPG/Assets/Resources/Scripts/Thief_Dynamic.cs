using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief_Dynamic : MonoBehaviour
{
    //public GameManager.PartnerState PS;


    Char_Status CharStatus;

    Animator animator;

    public GameObject PartnerTarget;

    bool setEnemy = false;

    public float AttackDelayTimer = 0;
    float AttackDelayTime = 2.0f;

    public GameObject objMeleeAttackPoint;

    bool m_bPartnerDeath = false;

    bool m_bRunAngle = false;

    bool m_bBackPos = false;



    float m_fSkill1CoolTimer = 0;
    float m_fSkill2CoolTimer = 0;

    bool m_bSkill1On = false;
    bool m_bSkill2On = false;

    // Start is called before the first frame update
    void Start()
    {
        
        CharStatus = GetComponent<Char_Status>();
        CharStatus.CS = GameManager.CharState.Idle;
        animator = this.GetComponent<Animator>();
        PartnerTarget = null;
        objMeleeAttackPoint = this.transform.GetChild(3).gameObject;
        CharStatus.setPlayerStatus(20,100,5,2,1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 3.5f);


        //Gizmos.color = Color.yellow;

        //Gizmos.DrawWireSphere(this.transform.position, 15);
    }

    void PatternSetting()
    {
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;
        //int i = 0;
        //PartnerTarget = null;
        AttackDelayTimer = AttackDelayTime;

        if (hitcol[0].GetComponent<Enemy_Ctrl>().ES==GameManager.EnemyState.Death)
        {
            CharStatus.CS = GameManager.CharState.Stay;
        }

        if (hitcol != null)
        {
            PartnerTarget = hitcol[0].gameObject;
        }

        //if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) > 10f)
        //{
        //    PS = GameManager.PartnerState.Skill1;
        //}
        //else
        if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) > 3.5f)
        {
            CharStatus.CS = GameManager.CharState.Move;
        }
        else
        {
            if (CharStatus.m_nPlayerMP >= 50&& m_bBackPos&&m_bSkill2On)
            {

                CharStatus.CS = GameManager.CharState.Skill2;
            }
            else if(CharStatus.m_nPlayerMP >= 40 && !m_bBackPos&&m_bSkill1On)
            {

                CharStatus.CS = GameManager.CharState.Skill1;
            }
            else
            {
                CharStatus.CS = GameManager.CharState.Attack;
            }
           
        }

        if (PartnerTarget == null || PartnerTarget.activeSelf==false)
        {
            CharStatus.CS = GameManager.CharState.Idle;
        }


    }


    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }


    bool Moving()
    {
        Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
        float dis = Vector3.Distance(this.transform.position, vecEnemyLookingPoint);


        if (dis > 3.5f)
        {
                animator.Play("Run");
                transform.LookAt(vecEnemyLookingPoint);

                transform.position = Vector3.MoveTowards(transform.position, vecEnemyLookingPoint, CharStatus.m_fPlayerSpeed * Time.deltaTime);
                //Debug.Log(dis);
                return true;
        }
        else
        {
            animator.Play("Idle");
            CharStatus.CS = GameManager.CharState.Attack;
            return false;
        }



        CharStatus.CS = GameManager.CharState.Idle;
        //animator.Play("Idle01");
        return false;
    }



    bool Attack()
    {


        if (AttackDelayTimer > AttackDelayTime/2)
        {

            AttackDelayTimer -= Time.deltaTime;
            Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= AttackDelayTime / 2 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                Vector3 PlayerLookingPoint = new Vector3(PartnerTarget.transform.position.x,this.transform.position.y, PartnerTarget.transform.position.z);
                animator.Play("AttackA1");
                Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
                transform.LookAt(PlayerLookingPoint);


                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
                Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position, new Vector3(1, 1, 1),
                    Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0)), m_nMask);

                //Vector3 vecHitCol = new Vector3(hitcol[0].transform.position.x, this.transform.position.y, hitcol[0].transform.position.z);
                
                //Debug.Log(angle);
                

                //Debug.Log(hitcol[0].gameObject);
                if (hitcol.Length != 0)
                {
                    Vector3 targetDir = this.transform.position - hitcol[0].transform.position;
                    float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);

                    if (angle <= 45)
                    {
                        hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage*2);
                        m_bBackPos = true;
                        //Debug.Log("BackHit");
                    }
                    else
                    {
                        hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage);
                        m_bBackPos = false;
                    }

                    
                    //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
                }





                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA1") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
        }

        CharStatus.CS = GameManager.CharState.Idle;
        return false;

    }



    void FlipOver()
    {
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(this.transform.position, 10.0f, m_nMask);//충돌감지 저장
                                                                                           //int count = 0;
                                                                                           //int i = 0;

        if (hitcol[0] != null)
        {
            this.transform.position = hitcol[0].transform.position - hitcol[0].transform.forward * 4;
        }

        Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
        transform.LookAt(vecEnemyLookingPoint);


        m_fSkill1CoolTimer = 5f;
        m_bSkill1On = false;

        CharStatus.CS = GameManager.CharState.Idle;
    }


    bool BackStep()
    {


        if (AttackDelayTimer > AttackDelayTime / 2)
        {

            AttackDelayTimer -= Time.deltaTime;
            Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= AttackDelayTime / 2 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                Vector3 PlayerLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
                animator.Play("AttackA3");
                Vector3 AttackArea = PlayerLookingPoint - this.transform.position;
                transform.LookAt(PlayerLookingPoint);

                CharStatus.UseMana(50);

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Enemy"));
                Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position, new Vector3(1, 1, 1),
                    Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, PlayerLookingPoint), 0)), m_nMask);


                //Vector3 vecHitCol = new Vector3(hitcol[0].transform.position.x, this.transform.position.y, hitcol[0].transform.position.z);
                
                //Debug.Log(angle);


                //Debug.Log(hitcol[0].gameObject);
                if (hitcol.Length != 0)
                {
                    Vector3 targetDir = this.transform.position - hitcol[0].transform.position;
                    float angle = Vector3.Angle(targetDir, -hitcol[0].transform.forward);
                    if (angle <= 45)
                    {
                        hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage * 4);
                        m_bBackPos = true;
                        //Debug.Log("BackHit");
                    }
                    else
                    {
                        hitcol[0].GetComponent<Enemy_Ctrl>().GetDamage(CharStatus.Damage*2);
                        m_bBackPos = false;
                    }


                    //Debug.Log(hitcol[0].GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
                }





                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA3") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
        }

        m_fSkill2CoolTimer = 10f;
        m_bSkill2On = false;

        CharStatus.CS = GameManager.CharState.Idle;
        return false;

    }



    bool Hit()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA2"))
        {

            animator.Play("AttackA2");

            return true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA2") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {

            return true;
        }

        CharStatus.CS = GameManager.CharState.Idle;
        return false;
    }

    void SkillCoolTimer()
    {
        if (m_fSkill1CoolTimer <= 0)
        {
            m_bSkill1On = true;
        }
        else
        {
            m_fSkill1CoolTimer -= Time.deltaTime;
        }

        if (m_fSkill2CoolTimer <= 0)
        {
            m_bSkill2On = true;
        }
        else
        {
            m_fSkill2CoolTimer -= Time.deltaTime;
        }


    }


    // Update is called once per frame
    void Update()
    {
        SkillCoolTimer();

        switch (CharStatus.CS)
        {
            case GameManager.CharState.Idle:
                animator.Play("Idle");
                PatternSetting();
                break;
            case GameManager.CharState.Move:
                Moving();
                break;
            case GameManager.CharState.Attack:
                Attack();
                break;
            case GameManager.CharState.Skill1:
                FlipOver();
                break;
            case GameManager.CharState.Skill2:
                BackStep();
                break;
            case GameManager.CharState.Hit:
                Hit();
                break;
            case GameManager.CharState.Death:
                animator.Play("Die");
                m_bPartnerDeath = true;
                break;
            case GameManager.CharState.Stay:
                animator.Play("Idle01");
                break;
        }

        if (PartnerTarget != null)
            Debug.DrawLine(this.transform.position, PartnerTarget.transform.position);


        if (CharStatus.m_nPlayerHP <= 0 && !m_bPartnerDeath)
        {
            CharStatus.CS = GameManager.CharState.Death;
        }

        if (m_bPartnerDeath && animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //Destroy(this.gameObject, 3f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && CharStatus.CS == GameManager.CharState.Move)
        {
            animator.Play("Idle");
            CharStatus.CS = GameManager.CharState.Attack;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8 && CharStatus.CS == GameManager.CharState.Move)
        {
            CharStatus.CS = GameManager.CharState.Idle;
        }
    }


}
