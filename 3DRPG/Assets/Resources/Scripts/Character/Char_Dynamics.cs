using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Dynamics : MonoBehaviour
{
    Char_Status CharStatus;

    Vector3 vecMovePoint = Vector3.zero;

    Vector3 vecStartPos = Vector3.zero;



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

    void ItemDrop()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            ItemDropData IDD = DBManager.GetItemDropDataByIdx(CharStatus.getID());
            GameManager.Instance.m_nGold += 100;
            //Debug.Log(IDD.IDP.Count);
            for (int i = 0; i < IDD.IDP.Count; i++)
            {
                int ran = Random.Range(0, 100);
                //Debug.Log(i+"번째 아이템 드랍 "+(ran+1)+"/100");
                if (ran < IDD.IDP[i])
                {
                    GameObject Item = Instantiate(Resources.Load<GameObject>("Prefabs/Item/DropItem"),this.transform.position,Quaternion.identity);
                    //Debug.Log("드랍성공");
                    //Debug.Log("ItemID : "+ IDD.IDT[i]);
                    //Debug.Log("ItemMesh : "+ DBManager.GetItemStatusByIdx(IDD.IDT[i]).Mesh);
                    //Debug.Log("ItemMaterial : "+ DBManager.GetItemStatusByIdx(IDD.IDT[i]).Material);
                    Item.GetComponent<DropItemInfo>().SetItem(IDD.IDT[i], DBManager.GetItemStatusByIdx(IDD.IDT[i]).Mesh, DBManager.GetItemStatusByIdx(IDD.IDT[i]).Material);
                }
                else
                {
                    //Debug.Log("드랍 실패!");
                }
            }
        }
        
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
                CharStatus.getAnimator().SetBool("Identity", false);
                break;
            case GameManager.CharState.Move:
                CharStatus.getAnimator().SetBool("Move", true);
                break;
            case GameManager.CharState.Attack:
                CharStatus.getAnimator().SetBool("Attack", true);
                SkillManager.Instance.SetCharSkill(CharStatus.getAttackID(), CharStatus);
                break;
            case GameManager.CharState.IdentitySkill:
                CharStatus.getAnimator().SetBool("Identity", true);
                SkillManager.Instance.SetCharSkill(CharStatus.getIdentitySkillID(), CharStatus);
                break;
            case GameManager.CharState.Skill1:
                CharStatus.getAnimator().SetBool("Skill1", true);
                CharStatus.UseMana(DBManager.SkillData[CharStatus.getSkillID()[0]].getSkillUsingMana());
                SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[0], CharStatus);
                break;
            case GameManager.CharState.Skill2:
                CharStatus.getAnimator().SetBool("Skill2", true);
                CharStatus.UseMana(DBManager.SkillData[CharStatus.getSkillID()[1]].getSkillUsingMana());
                SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[1], CharStatus);
                break;
            case GameManager.CharState.Skill3:
                CharStatus.getAnimator().SetBool("Skill3", true);
                CharStatus.UseMana(DBManager.SkillData[CharStatus.getSkillID()[2]].getSkillUsingMana());
                SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[2], CharStatus);
                break;
            case GameManager.CharState.Skill4:
                CharStatus.getAnimator().SetBool("Skill4", true);
                CharStatus.UseMana(DBManager.SkillData[CharStatus.getSkillID()[3]].getSkillUsingMana());
                SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[3], CharStatus);
                break;
            case GameManager.CharState.Hit:
                CharStatus.getAnimator().SetBool("Hit", true);
                CharStatus.getAnimator().SetBool("Move", false);
                CharStatus.getAnimator().SetBool("Attack", false);
                CharStatus.getAnimator().SetBool("Skill1", false);
                
                break;
            case GameManager.CharState.Death:
                CharStatus.getAnimator().SetBool("Death", true);
                ItemDrop();
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().isTrigger = true;
                //Destroy(this.gameObject, 5f);
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
                if (CharStatus.getAnimator()!=null && CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
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
                if (CharStatus.getIdentitySkillUsing())
                {
                    SkillManager.Instance.SetCharSkill(CharStatus.getIdentitySkillID(), CharStatus);
                }
                else
                {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Identity") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                break;
            case GameManager.CharState.Skill1:
                if (CharStatus.getSkillUsing()[0])
                {
                    SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[0], CharStatus);
                }
                else {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill1CoolTimer(DBManager.SkillData[CharStatus.getSkillID()[0]].getSkillCoolTime());
                        CharStatus.setSkill1On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                
                break;
            case GameManager.CharState.Skill2:
                if (CharStatus.getSkillUsing()[1])
                {
                    SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[1], CharStatus);
                }
                else
                {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill2") &&
                       CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill2CoolTimer(DBManager.SkillData[CharStatus.getSkillID()[1]].getSkillCoolTime());
                        CharStatus.setSkill2On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }

                }
                
                break;
            case GameManager.CharState.Skill3:
                if (CharStatus.getSkillUsing()[2])
                {
                    SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[2], CharStatus);
                }
                else
                {
                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill3") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        
                        CharStatus.setSkill3CoolTimer(DBManager.SkillData[CharStatus.getSkillID()[2]].getSkillCoolTime());
                        CharStatus.setSkill3On(false);
                        SetCharStatus(GameManager.CharState.Idle);
                    }
                }
                
                break;
            case GameManager.CharState.Skill4:
                if (CharStatus.getSkillUsing()[3])
                {
                    SkillManager.Instance.SetCharSkill(CharStatus.getSkillID()[3], CharStatus);
                }
                else
                {

                    if (CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Skill4") &&
                    CharStatus.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                       
                        CharStatus.setSkill4CoolTimer(DBManager.SkillData[CharStatus.getSkillID()[3]].getSkillCoolTime());
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

        if (CharStatus.getHP() <= 0 && CharStatus.CS!=GameManager.CharState.Death)
        {
            SetCharStatus(GameManager.CharState.Death);
        }
    }
}
