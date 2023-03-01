using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoSingleton<GameManager>
{
   

    

    [Header("Player")]
    public GameObject objPlayer;
    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;
    public Slider[] PlayerSkillCoolTime;


    [Header("Mouse,Button")]
    public GameObject MBTarget;
    public Vector3 MBPoint;
    public bool m_bKeySpaceOn = false;

    GraphicRaycaster GR;
    PointerEventData ped;
    public GameObject UIObj;
    public Vector3 UIMBPoint;
    public ItemData ClickItem = new ItemData();
    public GameObject DragingItem = null;
    public GameObject DragingItemSprite = null;

    [Header("UI")]
    public int m_nGold = 0;
    public GameObject objCanvas;
    public bool OnUIScreen = false;
    public List<GameObject> m_lOnUI = new List<GameObject>();

    [Header("Scenes")]
    public int m_nScreenIdx;


    public void CallGameManager()
    {
        Debug.Log("CallGM");
    }


    // Start is called before the first frame update
    void Start()
    {

        m_nScreenIdx = SceneManager.GetActiveScene().buildIndex;
    }


    public void SetCanvas(GameObject _objCanvas)
    {
        objCanvas = _objCanvas;
        GR = objCanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);//초기화

        //켜진 UI 초기화
        m_lOnUI.Clear();
    }

    public void GetInstancePlayerChar(Vector3 Pos)
    {
        GameObject PlayerObj = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[0].PFL), Pos, Quaternion.identity);


        objPlayer = PlayerObj;


        objPlayer.GetComponent<Char_Base>().CharStatus=(DBManager.PlayerData[PlayerPrefs.GetInt("Player")]);


        objPlayer.GetComponent<Char_Base>().itemSlots=new ItemSlot[Player_Inventory.Instance.getPlayerSlot()];


        objPlayer.GetComponent<Char_Base>().itemSlots[0] =Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor)];
        objPlayer.GetComponent<Char_Base>().itemSlots[1]= Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + 1];

        Camera.main.GetComponent<CameraPos>().Target = objPlayer;

    }


    public void SetPlayerUI(GameObject _objPlayer,Slider _PlayerHPBar, Slider _PlayerMPBar,Slider _PlayerShieldBar,Slider[] _PlayerSKillCoolTime)
    {
        objPlayer=_objPlayer;
        PlayerHPBar=_PlayerHPBar;
        PlayerMPBar=_PlayerMPBar;
        PlayerShieldBar=_PlayerShieldBar;
        PlayerSkillCoolTime=_PlayerSKillCoolTime;
    }

    //게임 오브젝트 레이케스트
    void MouseTargetRay()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        //int m_nLayerMask = 1 << LayerMask.NameToLayer("Player");
        //m_nLayerMask = ~m_nLayerMask;
        int m_nLayerMask = 1 << LayerMask.NameToLayer("Floor") | 1 << LayerMask.NameToLayer("NPC");

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_nLayerMask = 1 << LayerMask.NameToLayer("Player")| 1 << LayerMask.NameToLayer("Floor");
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



    void UpdateUI()
    {

        //Gold
        if (Player_Inventory.Instance.objInventory.activeSelf == true)
        {
            //Debug.LogError(Player_Inventory.Instance.objInventory.transform.GetChild(3).gameObject);
            //Debug.Log(Player_Inventory.Instance.objInventory.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text);
            //Debug.Log("Check");
            Player_Inventory.Instance.objInventory.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Gold : " + GameManager.Instance.m_nGold;
        }
    }


    void CtrlPlayer()
    {
        if (UIObj == null)
        {

            if ( objPlayer != null || objPlayer.activeSelf != false)
            {
                Char_Base CS = objPlayer.GetComponent<Char_Base>();

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

                if (CS.CS == CharState.Idle || CS.CS == CharState.Move || (CS.CS == CharState.Action&&CS.m_nActionIdx==5))
                {

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (CS.m_nIdentityPoint > 0&& !CS.m_bSkillUsing[5])
                        {
                            //CS.vecMovePoint =MBPoint;
                            CS.m_nActionIdx = 5;
                            CS.SetCharStatus(CharState.Action);
                        }

                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (CS.m_bSkillUsing[5])
                        {
                            //CS.delGetDamage = CS.GetDamage;
                            //CS.SetCharStatus(CharState.Idle);
                            CS.m_bSkillUsing[5]=false;
                        }
                    }


                    if (Input.GetMouseButtonDown(1) && MBTarget.layer == LayerMask.NameToLayer("Floor"))
                    {
                        CS.vecMovePoint = MBPoint;
                        if (CS.CS == CharState.Idle || CS.CS == CharState.Move)
                        {
                            CS.SetCharStatus(CharState.Move);
                        }


                    }


                    if (Input.GetMouseButtonDown(0))
                    {

                        CS.vecMovePoint = MBPoint;
                        CS.objTarget=MBTarget;
                        CS.m_nActionIdx = 0;
                        CS.SetCharStatus(CharState.Action);

                    }



                    if (Input.GetKeyDown(KeyCode.Q))
                    {

                        if (CS.CharStatus.MP >= DBManager.SkillData[CS.CharStatus.SID[1]].getSkillUsingMana() && CS.m_bSkillOn[1])
                        {
                            CS.vecMovePoint = MBPoint;
                            CS.objTarget = MBTarget;
                            CS.m_nActionIdx = 1;
                            CS.SetCharStatus(CharState.Action);
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (CS.CharStatus.MP >= DBManager.SkillData[CS.CharStatus.SID[2]].getSkillUsingMana() && CS.m_bSkillOn[2])
                        {
                            CS.vecMovePoint = MBPoint;
                            CS.objTarget = MBTarget;
                            CS.m_nActionIdx = 2;
                            CS.SetCharStatus(CharState.Action);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (CS.CharStatus.MP >= DBManager.SkillData[CS.CharStatus.SID[3]].getSkillUsingMana() && CS.m_bSkillOn[3])
                        {
                            CS.vecMovePoint = MBPoint;
                            CS.objTarget = MBTarget;
                            CS.m_nActionIdx = 3;
                            CS.SetCharStatus(CharState.Action);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (CS.CharStatus.MP >= DBManager.SkillData[CS.CharStatus.SID[4]].getSkillUsingMana() && CS.m_bSkillOn[4])
                        {
                            CS.vecMovePoint = MBPoint;
                            CS.objTarget = MBTarget;
                            CS.m_nActionIdx = 4;
                            CS.SetCharStatus(CharState.Action);
                        }
                    }




                }

            }
        }
    }


    void CtrlObjEvnet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MBTarget.layer ==LayerMask.NameToLayer("NPC"))
            {
                MBTarget.GetComponent<NpcEvent>().NS.ClickEvent();

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
                if (UIObj == null || UIObj.tag != "ItemSlot")
                {
                    DragingItemSprite.transform.parent = DragingItem.transform;
                    DragingItemSprite.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    if (UIObj == null)
                    {
                        Player_Inventory.Instance.RemoveItem(DragingItem.GetComponent<ItemSlot>().m_nSlotNum);
                    }
                    DragingItemSprite = null;
                    DragingItem = null;
                    ClickItem = null;
                    
                }
                else
                {
                    DragingItemSprite.transform.parent = DragingItem.transform;
                    DragingItemSprite.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    Player_Inventory.Instance.ChangeItemSlot(DragingItem.GetComponent<ItemSlot>().m_nSlotNum, UIObj.GetComponent<ItemSlot>().m_nSlotNum);
                    DragingItemSprite = null;
                    DragingItem = null;
                    ClickItem = null;
                }
            }

        }

        if (Input.GetMouseButtonDown(1))
        {

            if (UIObj != null && UIObj.tag == "ItemSlot"&& UIObj.GetComponent<ItemSlot>().item.TYP==2)
            {
                objPlayer.GetComponent<Char_Base>().HealingHP(UIObj.GetComponent<ItemSlot>().item.HP);
                Player_Inventory.Instance.RemoveItem(UIObj.GetComponent<ItemSlot>().m_nSlotNum);
            }

        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Player_Inventory.Instance.objInventory.activeSelf == true)
            {
                Player_Inventory.Instance.objInventory.SetActive(false);
                
            }
            else
            {
                Player_Inventory.Instance.objInventory.SetActive(true);
            }
            
        }


    }

    


    // Update is called once per frame
    void Update()
    {
        

        MouseTargetRay();
        UIMouseRay();


        CtrlPlayer();
        CtrlUI();
        CtrlObjEvnet();

        UpdateUI();

        //Debug.DrawLine(objPlayer.transform.position, objPlayer.GetComponent<Char_Dynamics>().getStartPos());

    }
}
