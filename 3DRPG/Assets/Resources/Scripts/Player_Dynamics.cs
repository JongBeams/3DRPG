using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dynamics : MonoBehaviour
{

    Char_Status CharStatus;

    Vector3 vecMovePoint =Vector3.zero;

    //public Vector3 PlayerLookingPoint;

    bool m_bIdentitySkillOn = false;

    //public bool m_bRushOn = false;
    //Vector3 vecRushStartpos = Vector3.zero;
    //float m_fRustDist;

    //ParticleSystem ProtectZoneEffect;

    //bool m_bPlayerDie = false;


    //get
    public bool getIdentitySkillOn()
    {
        return m_bIdentitySkillOn;
    }

    //set
    public void setMovePoint(Vector3 _MousePoint)
    {
        vecMovePoint = _MousePoint;
    }
    public void setIdentitySkillOn(bool _IdentitySkillOn)
    {
        m_bIdentitySkillOn = _IdentitySkillOn;
    }



    // Start is called before the first frame update
    void Start()
    {
        
        CharStatus = this.GetComponent<Char_Status>();

    }

    Vector3 PlayerLookingPoint()
    {
        return new Vector3(vecMovePoint.x, this.transform.position.y, vecMovePoint.z);
    }

    public void SetPlayerStatus(GameManager.CharState _CS)// 한번 실행
    {
        switch (_CS)
        {
            case GameManager.CharState.Idle:

                break;
            case GameManager.CharState.Move:

                break;
            case GameManager.CharState.Attack:

                break;
            case GameManager.CharState.IdentitySkill:
                transform.LookAt(PlayerLookingPoint());
                break;
            case GameManager.CharState.Skill1:

                break;
            case GameManager.CharState.Skill2:

                break;
            case GameManager.CharState.Skill3:

                break;
            case GameManager.CharState.Skill4:

                break;
            case GameManager.CharState.Hit:

                if (CharStatus.getHP() <= 0)
                {
                    CharStatus.setCS(GameManager.CharState.Death);
                }
                break;
            case GameManager.CharState.Death:

                break;

        }


        CharStatus.setCS(_CS);
    }



    void UpdatePlayerStatus()// 지속 실행
    {

        switch (CharStatus.getCS())
        {
            case GameManager.CharState.Idle:
                
                break;
            case GameManager.CharState.Move:
                transform.LookAt(PlayerLookingPoint());
                Moving();
                break;
            case GameManager.CharState.Attack:
                
                break;
            case GameManager.CharState.IdentitySkill:
                Moving();
                break;
            case GameManager.CharState.Skill1:
                
                break;
            case GameManager.CharState.Skill2:
                
                break;
            case GameManager.CharState.Skill3:
                
                break;
            case GameManager.CharState.Skill4:
                
                break;
            case GameManager.CharState.Hit:
                
                break;
            case GameManager.CharState.Death:
                
                break;

        }


    }



    bool Moving()
    {
        float dis = Vector3.Distance(transform.position, vecMovePoint);

        if (dis >= 0.02f)
        {
            //animator.Play("Move");
            

            transform.position = Vector3.MoveTowards(transform.position, PlayerLookingPoint(), CharStatus.getSpeed() * Time.deltaTime);

            return true;
        }
        // animator.Play("Idle_SwordShield");
        SetPlayerStatus(GameManager.CharState.Idle);
        return false;
    }


    //private void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.red;
    //    Vector3 AttackArea = PlayerLookingPoint() - this.transform.position;

    //    //Gizmos.DrawWireCube(this.transform.position + AttackArea.normalized, new Vector3(1, 1, 1),Quaternion.Euler());

    //}



    //bool Hit()
    //{
    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
    //    {

    //        animator.Play("Hit");

    //        return true;
    //    }
    //    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HIt") &&
    //        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //    {

    //        return true;
    //    }

    //    CharStatus.setCS(GameManager.CharState.Idle);
    //    return false;
    //}



    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatus();


        

        //if (m_bPlayerDie && animator.GetCurrentAnimatorStateInfo(0).IsName("Die_SwordShield") &&
        //    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    Destroy(this.gameObject, 3f);
        //    m_bPlayerDie = false;
        //}



    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            ////animator.Play("Idle_SwordShield");
            //if (CharStatus.CS == GameManager.CharState.Skill2)
            //{
            //    collision.gameObject.GetComponent<Enemy_Ctrl>().GetDamage((int)(m_nIdentityPoint * ((m_fRustDist / 30) / 2)));
            //    Debug.Log("EnemyHP : " + collision.gameObject.GetComponent<Enemy_Ctrl>().m_nEnemy_HP);
            //    Debug.Log("ShieldRush Damage : " + (int)(m_nIdentityPoint * ((m_fRustDist / 30) / 2)) + ", Dist : " + m_fRustDist + "/30");
            //}
            //CharStatus.CS = GameManager.CharState.Idle;
        }
    }
}
