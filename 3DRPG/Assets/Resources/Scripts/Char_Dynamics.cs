using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Dynamics : MonoBehaviour
{
    Char_Status CharStatus;

    Vector3 vecMovePoint = Vector3.zero;




    private void OnDrawGizmos()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        CharStatus = this.GetComponent<Char_Status>();
    }


    //get
    public Vector3 getMovePoint()
    {
        return vecMovePoint;
    }
    //set
    public void setMovePoint(Vector3 _MousePoint)
    {
        vecMovePoint = _MousePoint;
    }

    public Vector3 PlayerLookingPoint()
    {
        return new Vector3(vecMovePoint.x, this.transform.position.y, vecMovePoint.z);
    }

    public void SetCharStatus(GameManager.CharState _CS)// 한번 실행
    {

        switch (_CS)
        {
            case GameManager.CharState.Idle:
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                CharStatus.getAnimator().SetBool("Hit", false);
                break;
            case GameManager.CharState.Move:
                CharStatus.getAnimator().SetBool("Move", true);
                break;
            case GameManager.CharState.Attack:
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getAttackID(), CharStatus);
                break;
            case GameManager.CharState.IdentitySkill:
                transform.LookAt(PlayerLookingPoint());
                break;
            case GameManager.CharState.Skill1:
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill1ID(), CharStatus);
                break;
            case GameManager.CharState.Skill2:
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill2ID(), CharStatus);
                break;
            case GameManager.CharState.Skill3:
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill3ID(), CharStatus);
                break;
            case GameManager.CharState.Skill4:
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill4ID(), CharStatus);
                break;
            case GameManager.CharState.Hit:
                CharStatus.getAnimator().SetBool("Hit", true);
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                //animator.Play("Hit");
                break;
            case GameManager.CharState.Death:
                CharStatus.getAnimator().SetBool("Death", true);
                break;
            case GameManager.CharState.Stay:
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                CharStatus.getAnimator().SetBool("Hit", false);
                break;

        }


        CharStatus.setCS(_CS);


    }


    void UpdateCharStatus()// 지속 실행
    {
        //Partner_Dynamics PD = this.GetComponent<Partner_Dynamics>();
        Char_Status CS = this.GetComponent<Char_Status>();


        switch (CharStatus.getCS())
        {
            case GameManager.CharState.Idle:
                //if (this.gameObject.layer ==8 || this.gameObject.layer == 9)
                //{
                    
                //}
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    AlgorithmManager.SetAlgorithm(CharStatus.getID(), CS);
                    //Debug.Log(this.gameObject.name+","+this.gameObject.layer);
                }
                break;
            case GameManager.CharState.Move:
                MoveManager.SetMove(CharStatus.getID(), CS);
                break;
            case GameManager.CharState.Attack:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.IdentitySkill:
                MoveManager.SetMove(CharStatus.getID(), CS);
                break;
            case GameManager.CharState.Skill1:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill2:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill2") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill3:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill3") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Skill4:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill4") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.Hit:
                Hit();
                if (CharStatus.getHP() <= 0)
                {
                    SetCharStatus(GameManager.CharState.Death);
                }
                break;
            case GameManager.CharState.Death:

                break;
            case GameManager.CharState.Stay:
                break;

        }


    }


    void Hit()
    {
        if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && CharStatus.getHP() > 0)
        {

            SetCharStatus(GameManager.CharState.Idle);
        }

    }



    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        UpdateCharStatus();
    }
}
