using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Enemy_Ctrl : MonoBehaviour
{

    public GameManager.EnemyState ES;
    public GameManager.EnemyAttack EA;

    public GameObject objMeleeAttackPoint;

    public int m_nEnemy_HP = 100;
    public int m_nEnemy_HPMax = 100;

    float m_fEnemySpeed = 8;

    Animator animator;

    bool m_bEnemyDie = false;

    //float m_fActionDelayTimer = 0;

    public GameObject objTarget;


    Vector3 vecEnemyLookingPoint;

    public float AttackDelayTimer = 0;
    float AttackDelayTime = 1.5f;


    MeshRenderer MeleeAttackRange;

    public ParticleSystem FireBreathEffect;

    public void GetDamage(int _Damage)
    {
        if (ES != GameManager.EnemyState.Death)
        {
            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
            m_nEnemy_HP -= _Damage;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ES = GameManager.EnemyState.Idle;
        EA = GameManager.EnemyAttack.MeleeTargetAttack;
        objMeleeAttackPoint = this.transform.GetChild(2).gameObject;
        m_nEnemy_HP = m_nEnemy_HPMax;
        MeleeAttackRange = this.transform.GetChild(3).GetComponent<MeshRenderer>();
    }





    void PatternSetting()
    {
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 30f, m_nMask);
        int count = 0;
        int i = 0;
        //m_fActionDelayTimer = 3f;

       

        //animator.Play("Idle01");


        while (count<hitcol.Length)
        {
            if (hitcol[count].GetComponent<Char_Status>().CS != GameManager.CharState.Death)
            {
                break;
            }
            else
            {
                i++;
            }
            count++;
        }

        Debug.Log(i+","+count + "," + hitcol.Length);

        if (i == hitcol.Length)
        {
            ES = GameManager.EnemyState.Stay;
        }
        else
        {
            while (true)
            {
                objTarget = hitcol[Random.Range(0, hitcol.Length)].gameObject;
                if (objTarget.GetComponent<Char_Status>().CS != GameManager.CharState.Death)
                {
                    break;
                }

            }









            int random = Random.Range(0, 100);

            if (Vector3.Distance(objTarget.transform.position, this.transform.position) < 8f)
            {
                if (random < 75)
                {

                    if (Random.Range(0, 2) == 0)
                    {
                        AttackDelayTimer = AttackDelayTime;
                        EA = GameManager.EnemyAttack.MeleeTargetAttack;
                    }
                    else
                    {
                        AttackDelayTimer = AttackDelayTime * 2;
                        EA = GameManager.EnemyAttack.MeleeRangeAttack;
                    }

                }
                else
                {
                    AttackDelayTimer = AttackDelayTime * 2;
                    if (Random.Range(0, 2) == 0)
                    {
                        EA = GameManager.EnemyAttack.FireBall;
                    }
                    else
                    {
                        EA = GameManager.EnemyAttack.FireBreath;
                    }

                }
                ES = GameManager.EnemyState.Attack;

            }
            else if (Vector3.Distance(objTarget.transform.position, this.transform.position) < 20f)
            {
                if (random < 75)
                {
                    AttackDelayTimer = AttackDelayTime * 2;
                    if (Random.Range(0, 2) == 0)
                    {
                        EA = GameManager.EnemyAttack.FireBall;
                    }
                    else
                    {
                        EA = GameManager.EnemyAttack.FireBreath;
                    }
                    ES = GameManager.EnemyState.Attack;
                }
                else
                {

                    if (Random.Range(0, 2) == 0)
                    {
                        AttackDelayTimer = AttackDelayTime;
                        EA = GameManager.EnemyAttack.MeleeTargetAttack;
                    }
                    else
                    {
                        AttackDelayTimer = 2 * AttackDelayTime;
                        EA = GameManager.EnemyAttack.MeleeRangeAttack;
                    }
                    ES = GameManager.EnemyState.Move;
                }
            }
            else
            {
                if (random < 50)
                {
                    AttackDelayTimer = AttackDelayTime * 2;
                    if (Random.Range(0, 2) == 0)
                    {
                        EA = GameManager.EnemyAttack.FireBall;
                    }
                    else
                    {
                        EA = GameManager.EnemyAttack.FireBreath;
                    }
                    ES = GameManager.EnemyState.Move;
                }
                else
                {

                    if (Random.Range(0, 2) == 0)
                    {
                        AttackDelayTimer = AttackDelayTime;
                        EA = GameManager.EnemyAttack.MeleeTargetAttack;
                    }
                    else
                    {
                        AttackDelayTimer = 2 * AttackDelayTime;
                        EA = GameManager.EnemyAttack.MeleeRangeAttack;
                    }
                    ES = GameManager.EnemyState.Move;
                }
            }
            //Debug.Log("Dist : " + Vector3.Distance(objTarget.transform.position, this.transform.position));
            //Debug.Log("AttackPattern : " + EA);
           // Debug.Log("ActionPattern : " + ES);

        }
    }


    void AttackSetting()
    {
        switch (EA)
        {
            case GameManager.EnemyAttack.MeleeTargetAttack:
                MeleeTargetAttack();
                break;
            case GameManager.EnemyAttack.MeleeRangeAttack:
                MeleeRangeAttack();
                //Debug.Log("MeleeRangeAttack");
                //FireBall();
                break;
            case GameManager.EnemyAttack.FireBall:
                //Debug.Log("FireBall");
                FireBall();
                //ES = GameManager.EnemyState.Idle;
                break;
            case GameManager.EnemyAttack.FireBreath:
                FireBreath();
                //Debug.Log("FireBreath");
                //FireBall();
                break;
        }
    }




    bool Moving()
    {
        vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, this.transform.position.y, objTarget.transform.position.z);
        float dis = Vector3.Distance(transform.position, vecEnemyLookingPoint);
        float Range = 8f;
        if (EA == GameManager.EnemyAttack.FireBall || EA == GameManager.EnemyAttack.FireBreath)
        {
            Range = 15f;
        }

        animator.Play("Walk");

        transform.LookAt(vecEnemyLookingPoint);

        
        if (dis > Range)
        {
            transform.position = Vector3.MoveTowards(transform.position, vecEnemyLookingPoint, m_fEnemySpeed * Time.deltaTime);
            //Debug.Log(dis);
            return true;
        }
        else
        {
            animator.Play("Idle01");
            ES = GameManager.EnemyState.Attack;
            return false;
        }

        //ES = GameManager.EnemyState.Attack;
        //animator.Play("Idle01");
        //return false;
    }


    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }


    bool MeleeTargetAttack()
    {

        if (AttackDelayTimer > AttackDelayTime/2)
        {
            MeleeAttackRange.enabled = true;
            AttackDelayTimer -= Time.deltaTime;
            vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, this.transform.position.y, objTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= AttackDelayTime/2 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle02"))
            {

                animator.Play("BasicAttack");

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
                Collider[] hitcol = Physics.OverlapBox(objMeleeAttackPoint.transform.position,
                    new Vector3(3, 3, 3), Quaternion.Euler(new Vector3(0, GetAngle(this.transform.position, vecEnemyLookingPoint), 0)), m_nMask);

                //Debug.Log(hitcol[0].gameObject);
                if (hitcol.Length != 0)
                {
                    Debug.Log(hitcol[0].gameObject);
                    hitcol[0].GetComponent<Char_Status>().GetDamage(9);

                }

                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }

            //Debug.Log("Check");
        }



        
        ES = GameManager.EnemyState.Idle;
        return false;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 20f);


        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(this.transform.position, 10f);
    }


    bool FireBall()
    {

        if (AttackDelayTimer > AttackDelayTime/2)
        {
            AttackDelayTimer -= Time.deltaTime;
            vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, this.transform.position.y, objTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= AttackDelayTime/2 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            if (objTarget==null || objTarget.activeSelf==false)
            {
                ES = GameManager.EnemyState.Idle;
                return false;
            }
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle02"))
            {
                animator.Play("FlameAttack");

                if(objTarget!=null|| objTarget.activeSelf == false)
                {
                    GameObject objFireBall = Instantiate(Resources.Load<GameObject>("Prefabs/Fireball"), objMeleeAttackPoint.transform.position, Quaternion.identity);
                    objFireBall.GetComponent<FireBall>().SelectTarget(objTarget);
                }
                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("FlameAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
        }

        ES = GameManager.EnemyState.Idle;
        return false;

    }

    bool MeleeRangeAttack()
    {

        bool AttackTiming = true;
        if (AttackDelayTimer > AttackDelayTime / 2)
        {
            AttackDelayTimer -= Time.deltaTime;
            vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, this.transform.position.y, objTarget.transform.position.z);
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle02"))
            {
                AttackTiming = false;
                animator.Play("ClawAttack");

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
                Collider[] hitcol = Physics.OverlapSphere(this.transform.position, 10f, m_nMask);//충돌감지 저장
                int count = 0;
                //int i = 0;
                while (count < hitcol.Length)
                {

                    Vector3 targetDir = hitcol[count].transform.position - this.transform.position;
                    float angle = Vector3.Angle(targetDir, this.transform.forward);
                    if (angle <= 35)
                    {
                        Debug.Log(angle+""+ hitcol[count].gameObject.name);
                        hitcol[count].gameObject.GetComponent<Char_Status>().GetDamage(10);
                    }

                    count++;
                }
                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ClawAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("ClawAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.80f&&!AttackTiming)
            {
                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
                Collider[] hitcol = Physics.OverlapSphere(this.transform.position, 10f, m_nMask);//충돌감지 저장
                int count = 0;
                //int i = 0;
                while (count < hitcol.Length)
                {

                    Vector3 targetDir = hitcol[count].transform.position - this.transform.position;
                    float angle = Vector3.Angle(targetDir, this.transform.forward);
                    if (angle <= 45)
                    {
                        Debug.Log(angle + "" + hitcol[count].gameObject.name);
                        hitcol[count].gameObject.GetComponent<Char_Status>().GetDamage(10);
                    }

                    count++;
                }
                AttackTiming=true;
                return true;
            }


        }


        ES = GameManager.EnemyState.Idle;
        return false;

    }



    bool FireBreath()
    {



        if (AttackDelayTimer > AttackDelayTime/2)
        {
            AttackDelayTimer -= Time.deltaTime;
            vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, this.transform.position.y, objTarget.transform.position.z);
            transform.LookAt(vecEnemyLookingPoint);
            return true;
        }
        else if (AttackDelayTimer <= AttackDelayTime/2 && AttackDelayTimer > 0)
        {
            AttackDelayTimer -= Time.deltaTime;
            return true;
        }
        else
        {
            
            //Debug.Log("Check");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle02"))
            {

                animator.Play("FlameAttack");
                FireBreathEffect.Play();

                int m_nMask = 0;
                m_nMask = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
                Collider[] hitcol = Physics.OverlapSphere(this.transform.position, 20.0f, m_nMask);//충돌감지 저장
                int count = 0;
                //int i = 0;
                while (count < hitcol.Length)
                {
                    
                        Vector3 targetDir = hitcol[count].transform.position - this.transform.position;
                        float angle = Vector3.Angle(targetDir, this.transform.forward);
                    
                        if (angle <= 20)
                        {
                            Debug.Log(angle + "" + hitcol[count].gameObject.name);
                            hitcol[count].gameObject.GetComponent<Char_Status>().GetDamage(10);
                        }

                    count++;
                }
                return true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("FlameAttack") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return true;
            }
        }

        ES = GameManager.EnemyState.Idle;
        return false;

    }


    // Update is called once per frame
    void Update()
    {

        switch (ES)
        {
            case GameManager.EnemyState.Idle:
                MeleeAttackRange.enabled = false;
                animator.Play("Idle01");
                PatternSetting();
                break;
            case GameManager.EnemyState.Move:
                Moving();
                break;
            case GameManager.EnemyState.Attack:
                AttackSetting();
                break;
            case GameManager.EnemyState.Death:
                animator.Play("Die");
                m_bEnemyDie = true;
                break;
            case GameManager.EnemyState.Stay:
                MeleeAttackRange.enabled = false;
                animator.Play("Idle01");
                break;
        }



        if (m_nEnemy_HP <= 0 && !m_bEnemyDie)
        {
            m_nEnemy_HP = 0;
            ES = GameManager.EnemyState.Death;
        }

        if (m_bEnemyDie && animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //Destroy(this.gameObject, 3f);
            m_bEnemyDie = false;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9 && ES == GameManager.EnemyState.Move)
        //{

        //    ES = GameManager.EnemyState.Attack;
        //}
    }



}
