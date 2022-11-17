using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Dynamics : MonoBehaviour
{
    Char_Status CharStatus;

    Vector3 vecMovePoint = Vector3.zero;

    Vector3 vecStartPos = Vector3.zero;


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
    public Vector3 getStartPos()
    {
        return vecStartPos;
    }

    //set
    public void setMovePoint(Vector3 _MousePoint)
    {
        vecMovePoint = _MousePoint;
    }
    public void setStartPos()
    {
        vecStartPos = this.transform.position;
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
                CharStatus.SetObjTarget(null);
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                CharStatus.getAnimator().SetBool("Skill2", false);
                CharStatus.getAnimator().SetBool("Skill3", false);
                CharStatus.getAnimator().SetBool("Skill4", false);
                CharStatus.getAnimator().SetBool("Hit", false);
                break;
            case GameManager.CharState.Move:
                CharStatus.getAnimator().SetBool("Move", true);
                break;
            case GameManager.CharState.Attack:
                CharStatus.getAnimator().SetBool("Attack", true);
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getAttackID(), CharStatus);
                break;
            case GameManager.CharState.IdentitySkill:
                transform.LookAt(PlayerLookingPoint());
                break;
            case GameManager.CharState.Skill1:
                CharStatus.getAnimator().SetBool("Skill1", true);
                CharStatus.UseMana(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill1ID()].getSkillUsingMana());
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill1ID(), CharStatus);
                break;
            case GameManager.CharState.Skill2:
                CharStatus.getAnimator().SetBool("Skill2", true);
                CharStatus.UseMana(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill2ID()].getSkillUsingMana());
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill2ID(), CharStatus);
                break;
            case GameManager.CharState.Skill3:
                CharStatus.getAnimator().SetBool("Skill3", true);
                CharStatus.UseMana(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill3ID()].getSkillUsingMana());
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill3ID(), CharStatus);
                break;
            case GameManager.CharState.Skill4:
                CharStatus.getAnimator().SetBool("Skill4", true);
                CharStatus.UseMana(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill4ID()].getSkillUsingMana());
                GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill4ID(), CharStatus);
                break;
            case GameManager.CharState.Hit:
                CharStatus.getAnimator().SetBool("Hit", true);
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                if (CharStatus.getHP() <= 0)
                {
                    SetCharStatus(GameManager.CharState.Death);
                }
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


        switch (CharStatus.getCS())
        {
            case GameManager.CharState.Idle:
                //if (this.gameObject.layer ==8 || this.gameObject.layer == 9)
                //{
                    
                //}
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    AlgorithmManager.SetAlgorithm(CharStatus.getID(), CharStatus);
                    //Debug.Log(this.gameObject.name+","+this.gameObject.layer);
                }
                break;
            case GameManager.CharState.Move:
                MoveManager.SetMove(CharStatus.getID(), CharStatus);
                break;
            case GameManager.CharState.Attack:
                if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SetCharStatus(GameManager.CharState.Idle);
                }
                break;
            case GameManager.CharState.IdentitySkill:
                MoveManager.SetMove(CharStatus.getID(), CharStatus);
                break;
            case GameManager.CharState.Skill1:
                if (CharStatus.getSkill1Using())
                {
                    GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill1ID(), CharStatus);
                }
                else {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill1CoolTimer(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill1ID()].getSkillCoolTime());
                        CharStatus.setSkill1On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                
                break;
            case GameManager.CharState.Skill2:
                if (CharStatus.getSkill2Using())
                {
                    GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill2ID(), CharStatus);
                }
                else
                {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill2") &&
                       CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill2CoolTimer(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill1ID()].getSkillCoolTime());
                        CharStatus.setSkill2On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }

                }
                
                break;
            case GameManager.CharState.Skill3:
                if (CharStatus.getSkill3Using())
                {
                    GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill3ID(), CharStatus);
                }
                else
                {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill3") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill3CoolTimer(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill1ID()].getSkillCoolTime());
                        CharStatus.setSkill3On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                
                break;
            case GameManager.CharState.Skill4:
                if (CharStatus.getSkill4Using())
                {
                    GameManager.instance.getSM().SetPartnerSkill(CharStatus.getSkill4ID(), CharStatus);
                }
                else
                {

                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill4") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                       
                        CharStatus.setSkill4CoolTimer(CharDataBase.instance.m_lSkillDB[CharStatus.getSkill1ID()].getSkillCoolTime());
                        CharStatus.setSkill4On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                break;
            case GameManager.CharState.Hit:
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
                        CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && CharStatus.getHP() > 0)
                    {
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                break;
            case GameManager.CharState.Death:

                break;
            case GameManager.CharState.Stay:
                break;

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
