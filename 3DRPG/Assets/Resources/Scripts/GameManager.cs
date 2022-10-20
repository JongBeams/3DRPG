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
        Skill0,
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Hit,
        Death,
        Stay,
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


    //Mouse
    public GameObject MBTarget;
    public Vector3 MBPoint;

    //Keyboard
    public bool m_bKeySpaceOn = false;

    public GameObject Enemy;

    public Slider EnemyHPBar;

    public Text Target;


    //gameend
    public GameObject objGameEnd;
    public Text objGameEndMessage;
    bool m_bGameEnd = false;

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



    void ClickActive()
    {
        if (objPlayer != null||objPlayer.activeSelf!=false||!m_bGameEnd)
        {
            Player_Ctrl PC = objPlayer.GetComponent<Player_Ctrl>();
            Char_Status CS = objPlayer.GetComponent<Char_Status>();
            if (CS.CS == CharState.Idle || CS.CS == CharState.Move || CS.CS == CharState.Skill0)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (PC.m_nPlayerShieldPoint>0)
                    {
                        m_bKeySpaceOn = true;
                        if (CS.CS == CharState.Move)
                        {
                            PC.m_bGuardMoving = true;
                        }
                        //PC.PS = PlayerState.Idle;

                        //PC.GetPlayerLP(PlayerLookingPoint);
                        PC.gameObject.transform.LookAt(MBPoint);

                        CS.CS = CharState.Skill0;
                    }
                    

                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    m_bKeySpaceOn = false;
                    CS.CS = CharState.Idle;
                }


                if (Input.GetMouseButtonDown(1))
                {
                    if (CS.CS == CharState.Skill0 && PC.m_nPlayerShieldPoint != 0)
                    {
                        //MouseMovingPointRay();

                        PC.GetPlayerLP(MBPoint);
                        PC.m_bGuardMoving = true;
                        //PC.PS = PlayerState.OnShield;
                    }
                    else
                    {
                        //MouseMovingPointRay();
                        CS.CS = CharState.Idle;
                       
                        PC.GetPlayerLP(MBPoint);
                        CS.CS = CharState.Move;
                    }

                }

                if (!m_bKeySpaceOn)
                {


                    if (Input.GetMouseButtonDown(0))
                    {
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {

                            CS.CS = CharState.Idle;
                            //Debug.Log("Check");
                           
                            PC.GetPlayerLP(MBPoint);

                            CS.CS = CharState.Attack;
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {

                            CS.CS = CharState.Idle;
                            //Debug.Log("Check");
                           
                            PC.GetPlayerLP(MBPoint);
                           
                            if(CS.m_nPlayerMP >= 20&&PC.m_bQSkillOn)
                            {
                                CS.UseMana(20);
                                CS.CS = CharState.Skill1;
                            }
                            
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {

                            CS.CS = CharState.Idle;
                            //Debug.Log("Check");
                           
                            PC.GetPlayerLP(MBPoint);

                            
                            if (CS.m_nPlayerMP>=30 && PC.m_bWSkillOn)
                            {
                                CS.UseMana(30);
                                CS.CS = CharState.Skill2;
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {

                            CS.CS = CharState.Idle;
                            //Debug.Log("Check");
                            
                            PC.GetPlayerLP(MBPoint);

                            
                            if (CS.m_nPlayerMP >= 40 && PC.m_bESkillOn)
                            {
                                CS.UseMana(40);
                                CS.CS = CharState.Skill3;
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {

                            CS.CS = CharState.Idle;
                            //Debug.Log("Check");
                            
                            PC.GetPlayerLP(MBPoint);

                            
                            if (CS.m_nPlayerMP >= 10 && PC.m_bRSkillOn)
                            {
                                CS.UseMana(10);
                                CS.CS = CharState.Skill4;
                            }
                        }
                    }


                }

            }

            



        }
    }


    void HPMPBar()
    {

            //Debug.Log("check");
            PlayerHPBar.value = objPlayer.GetComponent<Char_Status>().m_nPlayerHP;
            PlayerMPBar.value = objPlayer.GetComponent<Char_Status>().m_nPlayerMP;
            PlayerShieldBar.value = objPlayer.GetComponent<Player_Ctrl>().m_nPlayerShieldPoint;
            PlayerHPBar.maxValue = objPlayer.GetComponent<Char_Status>().m_nPlayerHPMax;
            PlayerMPBar.maxValue = objPlayer.GetComponent<Char_Status>().m_nPlayerMPMax;
            PlayerShieldBar.maxValue = objPlayer.GetComponent<Player_Ctrl>().m_nPlayerShieldPointMax;

            PlayerSkillQCoolTime.value = 100 - objPlayer.GetComponent<Player_Ctrl>().m_fQSkillCoolTimer / 5 * 100;
            PlayerSkillWCoolTime.value = 100 - objPlayer.GetComponent<Player_Ctrl>().m_fWSkillCoolTimer / 7 * 100;
            PlayerSkillECoolTime.value = 100 - objPlayer.GetComponent<Player_Ctrl>().m_fESkillCoolTimer / 15 * 100;
            PlayerSkillRCoolTime.value = 100 - objPlayer.GetComponent<Player_Ctrl>().m_fRSkillCoolTimer / 5 * 100;

            PlayerSkillQCoolTime.maxValue = 100;
            PlayerSkillWCoolTime.maxValue = 100;
            PlayerSkillECoolTime.maxValue = 100;
            PlayerSkillRCoolTime.maxValue = 100;

            //Debug.Log("check");
            HealerHPBar.value = objHealer.GetComponent<Char_Status>().m_nPlayerHP;
            HealerMPBar.value = objHealer.GetComponent<Char_Status>().m_nPlayerMP;
            HealerHPBar.maxValue = objHealer.GetComponent<Char_Status>().m_nPlayerHPMax;
            HealerMPBar.maxValue = objHealer.GetComponent<Char_Status>().m_nPlayerMPMax;

   
            //Debug.Log("check");
            ThiefHPBar.value = objThief.GetComponent<Char_Status>().m_nPlayerHP;
            ThiefMPBar.value = objThief.GetComponent<Char_Status>().m_nPlayerMP;
            ThiefHPBar.maxValue = objThief.GetComponent<Char_Status>().m_nPlayerHPMax;
            ThiefMPBar.maxValue = objThief.GetComponent<Char_Status>().m_nPlayerMPMax;

            EnemyHPBar.value = Enemy.GetComponent<Enemy_Ctrl>().m_nEnemy_HP;
            Target.text = "Target : " + Enemy.GetComponent<Enemy_Ctrl>().objTarget.name + "\n NextPattern : " + Enemy.GetComponent<Enemy_Ctrl>().EA;
    }




    private void Awake()
    {
        if (GameManager.instance == null)
            GameManager.instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        EnemyHPBar.maxValue = Enemy.GetComponent<Enemy_Ctrl>().m_nEnemy_HPMax;
        

    }

    // Update is called once per frame
    void Update()
    {
        HPMPBar();

        MouseTargetRay();

        ClickActive();

        if (objPlayer.GetComponent<Char_Status>().CS == CharState.Death &&
            objHealer.GetComponent<Char_Status>().CS == CharState.Death &&
            objThief.GetComponent<Char_Status>().CS == CharState.Death)
        {
            m_bGameEnd = true;
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Over";
        }
        if (Enemy.GetComponent<Enemy_Ctrl>().ES==EnemyState.Death)
        {
            m_bGameEnd = true;
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Clear";
        }

        
        

        Debug.DrawLine(objPlayer.transform.position, objPlayer.GetComponent<Player_Ctrl>().PlayerLookingPoint) ;


    }
}
