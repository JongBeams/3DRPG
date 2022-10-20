using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parter_Dynamic : MonoBehaviour
{
    //public GameManager.PartnerState PS;


    Char_Status CharStatus;

    Animator animator;

    public GameObject PartnerTarget;

    bool setEnemy=false;

    public float AttackDelayTimer = 0;
    float AttackDelayTime = 2.5f;

    public GameObject objShootingPoint;

    bool m_bPartnerDeath = false;

    bool m_bRunAngle = false;


    float m_fSkill1CoolTimer =3;
    float m_fSkill2CoolTimer =10;

    bool m_bSkill1On =false;
    bool m_bSkill2On =false;

    // Start is called before the first frame update
    void Start()
    {
        
        CharStatus = GetComponent<Char_Status>();
        CharStatus.CS = GameManager.CharState.Idle;
        animator = this.GetComponent<Animator>();
        PartnerTarget = null;
        objShootingPoint = this.transform.GetChild(2).gameObject;
        CharStatus.setPlayerStatus(20,150,3,2,0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position,20);


        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, 15);
    }

    void PatternSetting()
    {
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")) | 1 << (LayerMask.NameToLayer("Enemy"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;
        //int i = 0;
        //PartnerTarget = null;
        AttackDelayTimer = AttackDelayTime;
        setEnemy = false;
        //healcheck = false;

        if (hitcol[0].GetComponent<Enemy_Ctrl>().ES == GameManager.EnemyState.Death)
        {
            CharStatus.CS = GameManager.CharState.Stay;
        }

        while (count< hitcol.Length)
        {
            if(hitcol[count].gameObject.layer==6|| hitcol[count].gameObject.layer == 9) {
                if (hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHP <= hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHPMax / 2 )
                {
                    if (CharStatus.m_nPlayerMP >= 50)
                    {
                        PartnerTarget = hitcol[count].gameObject;
                        setEnemy = false;
                        //healcheck = true;
                        break;
                    }
                    
                }
            }
            else
            {
                PartnerTarget = hitcol[count].gameObject;
                setEnemy = true;
            }
            count++;

        }

        if (CharStatus.m_nPlayerHP<= CharStatus.m_nPlayerHPMax/2 && CharStatus.m_nPlayerMP >= 50)
        {
            PartnerTarget = this.gameObject;
        }

        if (PartnerTarget!=null)
        {


            if (setEnemy)
            {
                if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) > 20f)
                {
                    CharStatus.CS = GameManager.CharState.Move;
                    m_bRunAngle = false;

                }
                else if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) < 15f)
                {
                    CharStatus.CS = GameManager.CharState.Move;
                    m_bRunAngle = true;
                }
                else if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) >= 15f || Vector3.Distance(PartnerTarget.transform.position, this.transform.position) < 20f)
                {
                    CharStatus.CS = GameManager.CharState.Attack;
                }
                else
                {
                    CharStatus.CS = GameManager.CharState.Idle;
                }
            }
            else
            {
                if (Vector3.Distance(PartnerTarget.transform.position, this.transform.position) > 15f)
                {
                    CharStatus.CS = GameManager.CharState.Move;
                    m_bRunAngle = false;

                }
                else 
                {
                    if (PartnerTarget == this.gameObject)
                    {
                        if (CharStatus.m_nPlayerMP >= 100&&m_bSkill2On)
                        {
                            CharStatus.CS = GameManager.CharState.Skill2;
                        }
                        else if (CharStatus.m_nPlayerMP >= 50 && m_bSkill1On)
                        {
                            CharStatus.CS = GameManager.CharState.Skill1;
                        }

                    }
                    else
                    {
                        if (CharStatus.m_nPlayerMP >= 50 && m_bSkill1On)
                        {
                            CharStatus.CS = GameManager.CharState.Skill1;
                        }
                        else
                        {
                            CharStatus.CS = GameManager.CharState.Idle;
                        }

                    }
                }
                
            }
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

        //Debug.Log("PartnerTargetObj : "+ PartnerTarget + ", ParterTargetPos : "+vecEnemyLookingPoint+", Dis : "+ dis);
        if (setEnemy)
        {
            if (dis > 20f)
            {
                animator.Play("Run");
                transform.LookAt(vecEnemyLookingPoint);

                transform.position = Vector3.MoveTowards(transform.position, vecEnemyLookingPoint, CharStatus.m_fPlayerSpeed * Time.deltaTime);
                //Debug.Log(dis);
                return true;
            }
            else if (dis < 15)
            {
                //Debug.Log("BackMove");
                animator.Play("Run");
                if (m_bRunAngle)
                {
                    //Debug.Log(GetAngle(transform.position, vecEnemyLookingPoint));
                    //transform.LookAt(vecEnemyLookingPoint);
                    if (GetAngle(transform.position, vecEnemyLookingPoint)<0)
                    {
                        this.transform.rotation = Quaternion.Euler(0,180 + GetAngle(transform.position, vecEnemyLookingPoint), 0);
                    }
                    else
                    {
                        this.transform.rotation = Quaternion.Euler(0,GetAngle(transform.position, vecEnemyLookingPoint)-180, 0);
                    }
                    
                    m_bRunAngle = false;
                }
                //transform.LookAt(vecEnemyLookingPoint);

                //transform.position = Vector3.MoveTowards(transform.position, vecEnemyLookingPoint, CharStatus.m_fPlayerSpeed * Time.deltaTime);
                this.transform.Translate(Vector3.forward * CharStatus.m_fPlayerSpeed * Time.deltaTime);
                return true;
            }
            else if (dis >= 15f && dis <= 20f)
            {
                //Debug.Log("check");
                animator.Play("Idle01");

                CharStatus.CS = GameManager.CharState.Attack;

                return false;
            }
            else
            {
                CharStatus.CS = GameManager.CharState.Idle;
                return false;
            }
        }
        else
        {
            if (dis > 15)
            {
                animator.Play("Run");
                transform.LookAt(vecEnemyLookingPoint);

                transform.position = Vector3.MoveTowards(transform.position, vecEnemyLookingPoint, CharStatus.m_fPlayerSpeed * Time.deltaTime);
                //Debug.Log(dis);
                return true;
            }
            else
            {
                animator.Play("Idle01");
                if (PartnerTarget == this.gameObject)
                    {
                        if (CharStatus.m_nPlayerMP >= 100 && m_bSkill2On)
                        {

                        CharStatus.CS = GameManager.CharState.Skill2;
                        }
                        else if (CharStatus.m_nPlayerMP >= 50 && m_bSkill1On)
                        {

                        CharStatus.CS = GameManager.CharState.Skill1;
                        }
                        else
                        {
                        CharStatus.CS = GameManager.CharState.Idle;
                        }
                    }
                    else
                    {
                        if (CharStatus.m_nPlayerMP >= 50 && m_bSkill1On)
                        {

                        CharStatus.CS = GameManager.CharState.Skill1;
                        }
                        else
                        {
                        CharStatus.CS = GameManager.CharState.Idle;
                        }
                    }
                    
                
                return false;
            }
        }


        CharStatus.CS = GameManager.CharState.Idle;
        //animator.Play("Idle01");
        return false;
    }



    bool Attack()
    {


        if (AttackDelayTimer > 1)
        {

            AttackDelayTimer -= Time.deltaTime;
            Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= 1 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01")|| animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {

                animator.Play("Attack01");

                GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/HealerBullet"), objShootingPoint.transform.position, Quaternion.identity);
                objFireBall.GetComponent<HealerBullet>().SelectTarget(PartnerTarget);
                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
        }

        CharStatus.CS = GameManager.CharState.Idle;
        return false;

    }


    bool Healing()
    {
        if (AttackDelayTimer > 1)
        {

            AttackDelayTimer -= Time.deltaTime;
            Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= 1 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {

                animator.Play("Attack03");
                CharStatus.UseMana(50);
                PartnerTarget.GetComponent<Char_Status>().m_nPlayerHP += 7;


                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03LoopEnd") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                m_fSkill1CoolTimer = 3f;
                m_bSkill1On = false;

                CharStatus.CS = GameManager.CharState.Idle;
                return false;
            }
        }
        return false;
        
    }

    bool AllHealing()
    {
        if (AttackDelayTimer > 1)
        {

            AttackDelayTimer -= Time.deltaTime;
            Vector3 vecEnemyLookingPoint = new Vector3(PartnerTarget.transform.position.x, this.transform.position.y, PartnerTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= 1 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {

                animator.Play("Attack03");
                CharStatus.UseMana(100);
                PartnerTarget.GetComponent<Char_Status>().m_nPlayerHP += 7;

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
                Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
                int count = 0;

                while (count < hitcol.Length)
                {
                    hitcol[count].gameObject.GetComponent<Char_Status>().m_nPlayerHP += 7;
                    count++;

                }



                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03LoopEnd") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                m_fSkill2CoolTimer = 10f;
                m_bSkill2On = false;

                CharStatus.CS = GameManager.CharState.Idle;
                return false;
            }
        }

        
        return false;

    }



    bool Hit()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
        {

            animator.Play("GetHit");

            return true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit") &&
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

        

        switch (CharStatus.CS)
        {
            case GameManager.CharState.Idle:
                animator.Play("Idle01");
                PatternSetting();
                break;
            case GameManager.CharState.Move:
                Moving();
                break;
            case GameManager.CharState.Attack:
                Attack();
                break;
            case GameManager.CharState.Skill1:
                Healing();
                break;
            case GameManager.CharState.Skill2:
                AllHealing();
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

        SkillCoolTimer();

        if (PartnerTarget!=null)
            Debug.DrawLine(this.transform.position,PartnerTarget.transform.position);


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
}
