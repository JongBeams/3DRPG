using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Partner_Dynamics : MonoBehaviour
{
    public int m_nCharID=0;

    Char_Status CharStatus;

    GameObject objTarget;

    Transform AttackPos;

    Animator animator;


    //bool Check : 캐릭터 별 체크 사함
    bool m_bCheck01 = false; //Healer = RunAway Dist Check //Thief = BackPos
    bool m_bCheck02 = false; //Healer = Target Enemy Check

    float m_fDelayTimer = 0;
    float m_fDelayTime = 2.5f;


    float m_fSkill1CoolTimer = 3;
    float m_fSkill2CoolTimer = 10;

    //Skill 사용 가능 여부
    bool m_bSkill1On = false;
    bool m_bSkill2On = false;

    bool m_bPartnerDeath = false;

    //string m_sDelayAniName = "";

    //get
    public Animator getAnimator()
    {
        return animator;
    }

    public Char_Status getCharStatus()
    {
        return CharStatus;
    }

    public GameObject getObjTarget()
    {
        return objTarget;
    }

    public Transform getAttackPos()
    {
        return AttackPos;
    }

    //get float,int
    public float getAttackDelayTimer()
    {
        return m_fDelayTimer;
    }
    public float getAttackDelayTime()
    {
        return m_fDelayTime;
    }

    //get bool
    public bool getCheck01()
    {
        return m_bCheck01;
    }
    public bool getCheck02()
    {
        return m_bCheck02;
    }

    public bool getSkill1On()
    {
        return m_bSkill1On;
    }
    public bool getSkill2On()
    {
        return m_bSkill2On;
    }


    //set
    public void SetObjTarget(GameObject _objTarget)
    {
       if(CharStatus.CS==GameManager.CharState.Idle)
            objTarget = _objTarget;
    }

    //set float, int, string
    public void setAttackDelayTimer(float _Timer)
    {
        m_fDelayTimer = _Timer;
    }

    public void setSkill1CoolTimer(float _Timer)
    {
        m_fSkill1CoolTimer = _Timer;
    }
    public void setSkill2CoolTimer(float _Timer)
    {
        m_fSkill2CoolTimer = _Timer;
    }

    //public void setDelayAniName(string _DelayAniName)
    //{
    //    m_sDelayAniName = _DelayAniName;
    //}

    //set bool
    public void setCheck01(bool _check)
    {
        m_bCheck01 = _check;
    }
    public void setCheck02(bool _check)
    {
        if (CharStatus.CS == GameManager.CharState.Idle)
            m_bCheck02 = _check;
    }

    public void setSkill1On(bool _check)
    {
        m_bSkill1On = _check;
    }
    public void setSkill2On(bool _check)
    {
        m_bSkill2On = _check;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 20);


        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, 15);
    }

    // Start is called before the first frame update
    void Start()
    {
        CharStatus = this.GetComponent<Char_Status>();
        CharStatus.CS = GameManager.CharState.Idle;
        animator = this.GetComponent<Animator>();
        objTarget = null;
        AttackPos = this.transform.GetChild(0);

        m_fDelayTimer = m_fDelayTime;
        //Debug.Log(this.transform.position);

       
    }


    public void SetPartnerStatus(GameManager.CharState _CS)// 한번 실행
    {

        Partner_Dynamics PD = this.GetComponent<Partner_Dynamics>();

        switch (_CS)
        {
            case GameManager.CharState.Idle:
                animator.SetBool("Move", false);
                animator.SetBool("Skill0", false);
                animator.SetBool("Skill1", false);
                animator.SetBool("Hit", false);
                break;
            case GameManager.CharState.Move:
                animator.SetBool("Move", true);
                break;
            case GameManager.CharState.Attack:
                GameManager.instance.getSM().SetSkill(m_nCharID, 0, PD);
                break;
            case GameManager.CharState.Skill1:
                GameManager.instance.getSM().SetSkill(m_nCharID, 1, PD);
                break;
            case GameManager.CharState.Skill2:
                GameManager.instance.getSM().SetSkill(m_nCharID, 2, PD);
                
                break;
            case GameManager.CharState.Hit:
                if (CharStatus.m_nPlayerHP <= 0)
                {
                    SetPartnerStatus(GameManager.CharState.Death);
                }
                else
                {
                    animator.SetBool("Hit", true);
                    animator.SetBool("Move", false);
                    animator.SetBool("Skill0", false);
                    animator.SetBool("Skill1", false);
                    //animator.Play("Hit");
                }
                break;
            case GameManager.CharState.Death:
                animator.SetBool("Death", true);
                break;
            case GameManager.CharState.Stay:
                
                break;
            case GameManager.CharState.Delay:
                animator.SetBool("Move", false);
                
                break;

        }

        
        CharStatus.CS = _CS;


    }

   

    void UpdatePartnerStatus()// 지속 실행
    {
        Partner_Dynamics PD = this.GetComponent<Partner_Dynamics>();


        switch (CharStatus.CS)
        {
            case GameManager.CharState.Idle:
                if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                    getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    AlgorithmManager.SetAlgorithm(m_nCharID, PD);
                }
                break;
            case GameManager.CharState.Move:
                MoveManager.SetMove(m_nCharID, PD);
                break;
            case GameManager.CharState.Attack:
                if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill0") &&
                    getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill1:
                if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill2:
                if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Hit:
                Hit();
                break;
            case GameManager.CharState.Death:
                
                break;
            case GameManager.CharState.Stay:
                break;
            case GameManager.CharState.Delay:
                //getDelay();
                break;

        }


    }


    void Hit()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f&& CharStatus.m_nPlayerHP > 0)
        {
            
            SetPartnerStatus(GameManager.CharState.Idle);
        }

    }

    //void getDelay()
    //{
    //    Debug.Log("check");
    //    //if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName(m_sDelayAniName) &&
    //    //            getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
    //    //{
    //    //    SetPartnerStatus(GameManager.CharState.Idle);
    //    //}
    //}

    //void StayEndAni()
    //{
    //    if (getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill0") &&
    //                getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
    //    {
    //        SetPartnerStatus(GameManager.CharState.Idle);
    //    }
    //}

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

        //PartnerPattern(m_nCharID);

        SkillCoolTimer();


    }

    private void FixedUpdate()
    {
        UpdatePartnerStatus();
    }
}
