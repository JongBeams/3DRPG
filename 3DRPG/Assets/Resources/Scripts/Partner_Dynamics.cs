using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Partner_Dynamics : MonoBehaviour
{
    Char_Status CharStatus;

    


    

    float m_fDelayTimer = 0;
    float m_fDelayTime = 2.5f;


   

    bool m_bPartnerDeath = false;

    //string m_sDelayAniName = "";

    //get
    public Char_Status getCharStatus()
    {
        return CharStatus;
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

    



    //set
   

    //set float, int, string
    public void setAttackDelayTimer(float _Timer)
    {
        m_fDelayTimer = _Timer;
    }



    //public void setDelayAniName(string _DelayAniName)
    //{
    //    m_sDelayAniName = _DelayAniName;
    //}

    

    


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
        m_fDelayTimer = m_fDelayTime;
        //Debug.Log(this.transform.position);

       
    }


    public void SetPartnerStatus(GameManager.CharState _CS)// 한번 실행
    {

        switch (_CS)
        {
            case GameManager.CharState.Idle:
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Skill0", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                CharStatus.getAnimator().SetBool("Hit", false);
                break;
            case GameManager.CharState.Move:
                CharStatus.getAnimator().SetBool("Move", true);
                break;
            case GameManager.CharState.Attack:
                GameManager.instance.getSM().SetPartnerSkill(, CharStatus);
                break;
            case GameManager.CharState.Skill1:
                GameManager.instance.getSM().SetPartnerSkill(, CharStatus);
                break;
            case GameManager.CharState.Skill2:
                GameManager.instance.getSM().SetPartnerSkill(, CharStatus);
                break;
            case GameManager.CharState.Hit:
                CharStatus.getAnimator().SetBool("Hit", true);
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Skill0", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                    //animator.Play("Hit");
                break;
            case GameManager.CharState.Death:
                CharStatus.getAnimator().SetBool("Death", true);
                break;
            case GameManager.CharState.Stay:
                
                break;
            case GameManager.CharState.Delay:
                CharStatus.getAnimator().SetBool("Move", false);
                
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
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    AlgorithmManager.SetAlgorithm(CharStatus.getID(), PD);
                }
                break;
            case GameManager.CharState.Move:
                MoveManager.SetMove(CharStatus.getID(), PD);
                break;
            case GameManager.CharState.Attack:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill0") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill1:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill2:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill2") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetPartnerStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Hit:
                Hit();
                if (CharStatus.getHP() <= 0)
                {
                   SetPartnerStatus(GameManager.CharState.Death);
                }
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
        if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f&& CharStatus.getHP() > 0)
        {
            
            SetPartnerStatus(GameManager.CharState.Idle);
        }

    }



    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        UpdatePartnerStatus();
    }
}
