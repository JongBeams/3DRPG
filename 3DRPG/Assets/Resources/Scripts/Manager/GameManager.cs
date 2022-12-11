using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    public enum ItemType
    {
        Empty,
        Wearable,
        Expendalbe
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

    public GameObject objWall;


    //Manager
    SkillManager SM;
    Player_Inventory PI;


    //Mouse
    public GameObject MBTarget;
    public Vector3 MBPoint;


    public bool m_bKeySpaceOn = false;

    public GameObject objEnemy;

    public Slider EnemyHPBar;

    public Text Target;

    public GameObject objCanvas;

    public SLManager slM;

    public DBManager DBM;


    //UIMouse
    GraphicRaycaster GR;
    PointerEventData ped;
    public GameObject UIObj;
    
    public Vector3 UIMBPoint;
    public ItemData ClickItem = new ItemData();
    public GameObject DragingItem=null;
    public GameObject DragingItemSprite = null;


    struct Dropitem
    {
        GameObject ItemObj;
        int ItemID;
    }
    List<Dropitem> m_lDropItem = new List<Dropitem>();
    

    //gameend
    public GameObject objGameEnd;
    public Text objGameEndMessage;
    bool m_bGameEnd = false;

    public int m_nEnemyID = 0;

    //get
    public SkillManager getSM()
    {
        return SM;
    }

    public Player_Inventory getPI()
    {
        return PI;
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
        
        SM = this.GetComponent<SkillManager>();
        PI = this.GetComponent<Player_Inventory>();
        slM = this.GetComponent<SLManager>();
        DBM = this.GetComponent<DBManager>();

        //UI 등록
        objCanvas = GameObject.FindGameObjectWithTag("Canvas");
        GR = objCanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);//초기화

        slM.RemoteStart();

        PI.RemoteStart();


        getInstanceChar();

        if (PlayerPrefs.GetInt("EnemyID") <= 0)
        {
            PI.InventorySave();
        }
        else
        {
            PI.InventoryLoad();
        }
        
        
    }


    void getInstanceChar()
    {
        m_nEnemyID = PlayerPrefs.GetInt("EnemyID");
        //
        GameObject PlayerObj = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[PlayerPrefs.GetInt("Player")].getObjPrefab()), new Vector3(0,0,-15), Quaternion.identity);
        GameObject Partner1Obj = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[PlayerPrefs.GetInt("Partner1")].getObjPrefab()), new Vector3(5, 0, -17), Quaternion.identity);
        GameObject Partner2Obj = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[PlayerPrefs.GetInt("Partner2")].getObjPrefab()), new Vector3(-5, 0, -17), Quaternion.identity);
        GameObject EnemyObj = Instantiate(Resources.Load<GameObject>(DBManager.EnemyData[m_nEnemyID].getObjPrefab()), new Vector3(0, 0, 15), Quaternion.identity);

        objPlayer = PlayerObj;
        objHealer = Partner1Obj;
        objThief = Partner2Obj;
        objEnemy = EnemyObj;

        objPlayer.GetComponent<Char_Status>().CharStatusSetting(DBManager.PlayerData[PlayerPrefs.GetInt("Player")]);
        objHealer.GetComponent<Char_Status>().CharStatusSetting(DBManager.PartnerData[PlayerPrefs.GetInt("Partner1")]);
        objThief.GetComponent<Char_Status>().CharStatusSetting(DBManager.PartnerData[PlayerPrefs.GetInt("Partner2")]);

        objPlayer.GetComponent<Char_Status>().setItemSlotsNum(PI.getPlayerSlot());
        objHealer.GetComponent<Char_Status>().setItemSlotsNum(PI.getPartner1Slot());
        objThief.GetComponent<Char_Status>().setItemSlotsNum(PI.getPartner2Slot());

        objPlayer.GetComponent<Char_Status>().setItemSlots(0,PI.m_lSlot[(PI.ver * PI.hor)]);
        objPlayer.GetComponent<Char_Status>().setItemSlots(1,PI.m_lSlot[(PI.ver * PI.hor)+1]);

        objHealer.GetComponent<Char_Status>().setItemSlots(0, PI.m_lSlot[(PI.ver * PI.hor)+PI.getPlayerSlot()]);
        objHealer.GetComponent<Char_Status>().setItemSlots(1, PI.m_lSlot[(PI.ver * PI.hor)+PI.getPlayerSlot()+1]);

        objThief.GetComponent<Char_Status>().setItemSlots(0, PI.m_lSlot[(PI.ver * PI.hor)+PI.getPlayerSlot()+PI.getPartner1Slot()]);
        objThief.GetComponent<Char_Status>().setItemSlots(1, PI.m_lSlot[(PI.ver * PI.hor)+PI.getPlayerSlot() + PI.getPartner1Slot() + 1]);

        objEnemy.GetComponent<Char_Status>().CharStatusSetting(DBManager.EnemyData[m_nEnemyID]);
        objEnemy.GetComponent<Char_Status>().SetSuperArmor(true);
    }



    //게임 오브젝트 레이케스트
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

    void UIMouseRay()
    {
        ped.position = Input.mousePosition;//마우스 위치의 이벤트 실행
        List<RaycastResult> rayResults = new List<RaycastResult>();
        GR.Raycast(ped, rayResults);

        //UIPos = ped.position;
        //UIMBPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UIMBPoint = new Vector3(
            (Camera.main.ScreenToViewportPoint(Input.mousePosition).x*Screen.width)- (Screen.width/2),
            (Camera.main.ScreenToViewportPoint(Input.mousePosition).y * Screen.height) - (Screen.height / 2), 0);


        if (rayResults.Count > 0)
        {
            UIObj = rayResults[0].gameObject;
        }
        else
        {
            UIObj = null;
        }
    }

    public void GoMain()
    {
        SceneManager.LoadScene("MainScene");

    }

    public void NextStage()
    {
        PI.InventorySave();
        if (m_nEnemyID == 1)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("InGmaeScene");
            PlayerPrefs.SetInt("EnemyID", m_nEnemyID+1);
        }
        
    }




    void CtrlPlayer()
    {
        if (objPlayer != null || objPlayer.activeSelf != false)
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

            if (CS.getCS() == CharState.Idle || CS.getCS() == CharState.Move || CS.getCS() == CharState.IdentitySkill&&UIObj==null)
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
                    if (CS.getCS()==CharState.IdentitySkill)
                    {
                        CS.delGetDamae = CS.GetDamage;
                        CD.SetCharStatus(CharState.Idle);
                        CS.setIdentitySkillUsing(false);
                    }
                }


                if (Input.GetMouseButtonDown(1))
                {
                    CD.setMovePoint(MBPoint);
                    if (CS.getCS() == CharState.Idle || CS.getCS() == CharState.Move && !CS.getIdentitySkillUsing())
                    {
                        CD.SetCharStatus(CharState.Move);
                    }

                }


                if (Input.GetMouseButtonDown(0))
                {
                    
                    CD.setMovePoint(MBPoint);
                    CS.SetObjTarget(MBTarget);
                    CD.SetCharStatus(CharState.Attack);

                }

                if (Input.GetKeyDown(KeyCode.Q))
                {

                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[0]].getSkillUsingMana() && CS.getSkill1On())
                    {
                        CD.setMovePoint(MBPoint);
                        CS.SetObjTarget(MBTarget);
                        CD.SetCharStatus(CharState.Skill1);
                    }
                    
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[1]].getSkillUsingMana() && CS.getSkill2On())
                    {
                        CD.setMovePoint(MBPoint);
                        CS.SetObjTarget(MBTarget);
                        CD.SetCharStatus(CharState.Skill2);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[2]].getSkillUsingMana() && CS.getSkill3On())
                    {
                        CD.setMovePoint(MBPoint);
                        CS.SetObjTarget(MBTarget);
                        CD.SetCharStatus(CharState.Skill3);
                    }
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (CS.getMP() >= DBManager.SkillData[CS.getSkillID()[3]].getSkillUsingMana() && CS.getSkill4On())
                    {
                        CD.setMovePoint(MBPoint);
                        CS.SetObjTarget(MBTarget);
                        CD.SetCharStatus(CharState.Skill4);
                    }
                }




            }

        }
    }


    void CtrlUI()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (UIObj != null && UIObj.tag == "ItemSlot")
            {
                ClickItem = UIObj.GetComponent<ItemSlot>().item;
                DragingItem = UIObj;
                DragingItemSprite = UIObj.transform.GetChild(0).gameObject;
                DragingItemSprite.transform.parent = objCanvas.transform;
            }

        }
        if (Input.GetMouseButton(0))
        {
            if(DragingItemSprite!=null)
                DragingItemSprite.GetComponent<RectTransform>().anchoredPosition = UIMBPoint;

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (DragingItem != null)
            {
                if (UIObj.tag != "ItemSlot")
                {
                    DragingItemSprite.transform.parent = DragingItem.transform;
                    DragingItemSprite.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    if (UIObj == null)
                    {
                        PI.RemoveItem(DragingItem.GetComponent<ItemSlot>().m_nSlotNum);
                    }
                    DragingItemSprite = null;
                    DragingItem = null;
                    ClickItem = null;
                    
                }
                else
                {
                    DragingItemSprite.transform.parent = DragingItem.transform;
                    DragingItemSprite.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    PI.ChangeItemSlot(DragingItem.GetComponent<ItemSlot>().m_nSlotNum, UIObj.GetComponent<ItemSlot>().m_nSlotNum);
                    DragingItemSprite = null;
                    DragingItem = null;
                    ClickItem = null;
                }
            }

        }

        if (Input.GetMouseButtonDown(1))
        {



        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (PI.objInventory.activeSelf == true)
            {
                PI.objInventory.SetActive(false);
            }
            else
            {
                PI.objInventory.SetActive(true);
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

        EnemyHPBar.value = objEnemy.GetComponent<Char_Status>().getHP();
        EnemyHPBar.maxValue = objEnemy.GetComponent<Char_Status>().getHPMax();
        if(objEnemy.GetComponent<Char_Status>().getObjTarget()!=null)
            Target.text = "Target : " + objEnemy.GetComponent<Char_Status>().objTarget.GetComponent<Char_Status>().getName() 
                + "\n NextPattern : " + objEnemy.GetComponent<Char_Status>().getCS();
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
        if (objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death)
        {
            m_bGameEnd = true;
            objWall.SetActive(false);
        }
        if ((objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death && objPlayer.GetComponent<Char_Status>().getCS() == CharState.Death)|| (objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death && m_nEnemyID == 1))
        {
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Clear";
        }
        
    }






    // Update is called once per frame
    void Update()
    {
        HPMPBar();

        MouseTargetRay();
        UIMouseRay();


        CtrlPlayer();
        CtrlUI();

            

        GameEndText();



        //Debug.DrawLine(objPlayer.transform.position, objPlayer.GetComponent<Char_Dynamics>().getStartPos());



    }
}
