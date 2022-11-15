using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum CharState
    {
        Idle,
        Move,
        Attack,
        IdentitySkill,
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Hit,
        Death,
        Stay,
        Delay,
    }

   

    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Death,
        Stay,
    }

    public enum EnemyAttack
    {
        MeleeTargetAttack,
        MeleeRangeAttack,
        FireBall,
        FireBreath,
    }


    public static GameManager instance;

    public GameObject objPlayer;
    public GameObject objHealer;
    public GameObject objThief;

    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;


    public Slider HealerHPBar;
    public Slider HealerMPBar;

    public Slider ThiefHPBar;
    public Slider ThiefMPBar;

    public Slider PlayerSkillQCoolTime;
    public Slider PlayerSkillWCoolTime;
    public Slider PlayerSkillECoolTime;
    public Slider PlayerSkillRCoolTime;


    //Manager
    SkillManager SM;


    //Mouse
    public GameObject MBTarget;
    public Vector3 MBPoint;

    //Keyboard
    public bool m_bKeySpaceOn = false;

    public GameObject objEnemy;

    public Slider EnemyHPBar;

    public Text Target;


    //gameend
    public GameObject objGameEnd;
    public Text objGameEndMessage;
    bool m_bGameEnd = false;


    //get
    public SkillManager getSM()
    {
        return SM;
    }
    public bool getGameEnd()
    {
        return m_bGameEnd;
    }



    private void Awake()
    {
        if (GameManager.instance == null)
            GameManager.instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyHPBar.maxValue = objEnemy.GetComponent<Enemy_Ctrl>().m_nEnemy_HPMax;
        SM = this.GetComponent<SkillManager>();

        objPlayer.GetComponent<Char_Status>().CharStatusSetting(CharDataBase.instance.m_lPlayerDB[0]);
        objHealer.GetComponent<Char_Status>().CharStatusSetting(CharDataBase.instance.m_lPartnerDB[0]);
        objThief.GetComponent<Char_Status>().CharStatusSetting(CharDataBase.instance.m_lPartnerDB[1]);

    }


    void MouseTargetRay()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        //int m_nLayerMask = 1 << LayerMask.NameToLayer("Player");
        //m_nLayerMask = ~m_nLayerMask;
        int m_nLayerMask = 1 << LayerMask.NameToLayer("Floor");

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_nLayerMask = 1 << LayerMask.NameToLayer("Player");
            m_nLayerMask = ~m_nLayerMask;
        }


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_nLayerMask))
        {
            //Curser.transform.position = new Vector3(hit.point.x,hit.point.y+1f, hit.point.z);

            MBTarget = hit.collider.gameObject;
            MBPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        }

    }

    //void MouseMovingPointRay()
    //{
    //    RaycastHit hit;
    //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


    //    int m_nLayerMask = 1 << LayerMask.NameToLayer("Floor");
    //    //m_nLayerMask = ~m_nLayerMask;


    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_nLayerMask))
    //    {
    //        //Curser.transform.position = new Vector3(hit.point.x,hit.point.y+1f, hit.point.z);

    //        MBTarget = hit.collider.gameObject;
    //        MBPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

    //    }

    //}

    public void GoMain()
    {
        SceneManager.LoadScene("MainScence");
        


    }

    void CtrlPlayer()
    {
        if (objPlayer != null || objPlayer.activeSelf != false && !m_bGameEnd)
        {
            Char_Status CS = objPlayer.GetComponent<Char_Status>();
            Char_Dynamics CD = objPlayer.GetComponent<Char_Dynamics>();

            //CS.SetObjTarget(objEnemy);
            // 키 조작 조건
            /*
            이동 : 대기, 이동, 고유
            공격 : 대기, 이동
            고유 : 대기, 이동, 고유
            스킬1 : 대기, 이동 
            스킬2 : 대기, 이동
            스킬3 : 대기, 이동
            스킬4 : 대기, 이동
            
            이동 = 고유 같은 취급
             */

            if (CS.getCS() == CharState.Idle || CS.getCS() == CharState.Move || CS.getCS() == CharState.IdentitySkill)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (CS.getIdentityPoint() > 0)
                    {
                        CD.setMovePoint(MBPoint);
                        CD.SetCharStatus(CharState.IdentitySkill);
                    }

                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    CD.SetCharStatus(CharState.Idle);
                }


                if (Input.GetMouseButtonDown(1))
                {
                    CD.setMovePoint(MBPoint);
                    if (CS.getCS() == CharState.Idle || CS.getCS() == CharState.Move || CS.getIdentityPoint() == 0)
                    {
                        CD.SetCharStatus(CharState.Move);
                    }

                }


                if (Input.GetMouseButtonDown(0))
                {
                    
                    CD.setMovePoint(MBPoint);
                    CD.SetCharStatus(CharState.Attack);

                }

                if (Input.GetKeyDown(KeyCode.Q))
                {

                    if (CS.getMP() >= 20 && CS.getSkill1On())
                    {
                        CS.UseMana(20);
                        CD.setMovePoint(MBPoint);
                        CD.SetCharStatus(CharState.Skill1);
                    }
                    
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (CS.getMP() >= 30 && CS.getSkill2On())
                    {
                        CS.UseMana(30);
                        CD.setMovePoint(MBPoint);
                        CD.SetCharStatus(CharState.Skill2);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (CS.getMP() >= 40 && CS.getSkill3On())
                    {
                        CS.UseMana(40);
                        CD.setMovePoint(MBPoint);
                        CD.SetCharStatus(CharState.Skill3);
                    }
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (CS.getMP() >= 10 && CS.getSkill4On())
                    {
                        CS.UseMana(10);
                        CD.setMovePoint(MBPoint);
                        CD.SetCharStatus(CharState.Skill4);
                    }
                }




            }

        }
    }

    void HPMPBar()
    {
        Char_Status CS = objPlayer.GetComponent<Char_Status>();

        //Debug.Log("check");
        PlayerHPBar.value = CS.getHP();
        PlayerMPBar.value = CS.getMP();
        PlayerShieldBar.value = CS.getIdentityPoint();
        PlayerHPBar.maxValue = CS.getHPMax();
        PlayerMPBar.maxValue = CS.getMPMax();
        PlayerShieldBar.maxValue = CS.getIdentityPointMax();

        PlayerSkillQCoolTime.value = 100 - CS.getSkill1CoolTimer() / 5 * 100;
        PlayerSkillWCoolTime.value = 100 - CS.getSkill2CoolTimer() / 7 * 100;
        PlayerSkillECoolTime.value = 100 - CS.getSkill3CoolTimer() / 15 * 100;
        PlayerSkillRCoolTime.value = 100 - CS.getSkill4CoolTimer() / 5 * 100;

        PlayerSkillQCoolTime.maxValue = 100;
        PlayerSkillWCoolTime.maxValue = 100;
        PlayerSkillECoolTime.maxValue = 100;
        PlayerSkillRCoolTime.maxValue = 100;

        //Debug.Log("check");
        HealerHPBar.value = objHealer.GetComponent<Char_Status>().getHP();
        HealerMPBar.value = objHealer.GetComponent<Char_Status>().getMP();
        HealerHPBar.maxValue = objHealer.GetComponent<Char_Status>().getHPMax();
        HealerMPBar.maxValue = objHealer.GetComponent<Char_Status>().getMPMax();


        //Debug.Log("check");
        ThiefHPBar.value = objThief.GetComponent<Char_Status>().getHP();
        ThiefMPBar.value = objThief.GetComponent<Char_Status>().getMP();
        ThiefHPBar.maxValue = objThief.GetComponent<Char_Status>().getHPMax();
        ThiefMPBar.maxValue = objThief.GetComponent<Char_Status>().getMPMax();

        EnemyHPBar.value = objEnemy.GetComponent<Enemy_Ctrl>().m_nEnemy_HP;
            Target.text = "Target : " + objEnemy.GetComponent<Enemy_Ctrl>().objTarget.name + "\n NextPattern : " + objEnemy.GetComponent<Enemy_Ctrl>().EA;
    }


     void GameEndText()
    {
        if (objPlayer.GetComponent<Char_Status>().getCS() == CharState.Death &&
            objHealer.GetComponent<Char_Status>().getCS() == CharState.Death &&
            objThief.GetComponent<Char_Status>().getCS() == CharState.Death)
        {
            m_bGameEnd = true;
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Over";
        }
        if (objEnemy.GetComponent<Enemy_Ctrl>().ES == EnemyState.Death)
        {
            m_bGameEnd = true;
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Clear";
        }
    }






    // Update is called once per frame
    void Update()
    {
        HPMPBar();

        MouseTargetRay();

        CtrlPlayer();

        GameEndText();



        //Debug.DrawLine(objPlayer.transform.position, objPlayer.GetComponent<Player_Ctrl>().PlayerLookingPoint) ;


    }
}
