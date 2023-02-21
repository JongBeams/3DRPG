using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class InGameSceneManager : MonoBehaviour
{
    public static InGameSceneManager Instance;

    [Serializable]
    public struct PartnerInfo {

        public GameObject objPartner;
        public Slider PartnerHPBar;
        public Slider PartnerMPBar;
        public Camera PartnerCam;
    }

    [Header("PlayerUI")]
    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;
    public Slider[] PlayerSkillCoolTime=new Slider[4];

    [Header("Partner")]
    public PartnerInfo[] PInfo = new PartnerInfo[2];

    [Header("Enemy")]
    public GameObject objEnemy;
    public Slider EnemyHPBar;
    public Text Target;

    [Header("UI")]
    public GameObject objCanvas;
    GraphicRaycaster GR;
    PointerEventData ped;
    public GameObject objGameEnd;
    public Text objGameEndMessage;
    public bool m_bGameEnd = false;
    public int m_nEnemyID = 0;


    public bool CharAllDeathCheck()
    {
        if(GameManager.Instance.objPlayer.GetComponent<Char_Status>().getCS() == GameManager.CharState.Death &&
            PInfo[0].objPartner.GetComponent<Char_Status>().getCS() == GameManager.CharState.Death &&
            PInfo[1].objPartner.GetComponent<Char_Status>().getCS() == GameManager.CharState.Death)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //UI 등록
        objCanvas = GameObject.FindGameObjectWithTag("Canvas");
        GR = objCanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);//초기화

        GameManager.Instance.SetCanvas(objCanvas);

        

        SLManager.Instance.RemoteStart();

        Player_Inventory.Instance.RemoteStart();

        Player_Inventory.Instance.InventoryLoad();

        getInstanceChar();
    }


    void getInstanceChar()
    {
        m_nEnemyID = PlayerPrefs.GetInt("Enemy");
        //
        GameObject PlayerObj = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[PlayerPrefs.GetInt("Player")].getObjPrefab()), new Vector3(0, 0, -15), Quaternion.identity);
        GameObject Partner1Obj = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[PlayerPrefs.GetInt("Partner1")].getObjPrefab()), new Vector3(5, 0, -17), Quaternion.identity);
        GameObject Partner2Obj = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[PlayerPrefs.GetInt("Partner2")].getObjPrefab()), new Vector3(-5, 0, -17), Quaternion.identity);
        GameObject EnemyObj = Instantiate(Resources.Load<GameObject>(DBManager.EnemyData[m_nEnemyID].getObjPrefab()), new Vector3(0, 0, 15), Quaternion.identity);

        GameManager.Instance.objPlayer = PlayerObj;
        PInfo[0].objPartner = Partner1Obj;
        PInfo[1].objPartner = Partner2Obj;
        objEnemy = EnemyObj;

        GameManager.Instance.objPlayer.GetComponent<Char_Status>().CharStatusSetting(DBManager.PlayerData[PlayerPrefs.GetInt("Player")]);
        PInfo[0].objPartner.GetComponent<Char_Status>().CharStatusSetting(DBManager.PartnerData[PlayerPrefs.GetInt("Partner1")]);
        PInfo[1].objPartner.GetComponent<Char_Status>().CharStatusSetting(DBManager.PartnerData[PlayerPrefs.GetInt("Partner2")]);

        GameManager.Instance.objPlayer.GetComponent<Char_Status>().setItemSlotsNum(Player_Inventory.Instance.getPlayerSlot());
        PInfo[0].objPartner.GetComponent<Char_Status>().setItemSlotsNum(Player_Inventory.Instance.getPartner1Slot());
        PInfo[1].objPartner.GetComponent<Char_Status>().setItemSlotsNum(Player_Inventory.Instance.getPartner2Slot());

        GameManager.Instance.objPlayer.GetComponent<Char_Status>().setItemSlots(0, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor)]);
        GameManager.Instance.objPlayer.GetComponent<Char_Status>().setItemSlots(1, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + 1]);

        PInfo[0].objPartner.GetComponent<Char_Status>().setItemSlots(0, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + Player_Inventory.Instance.getPlayerSlot()]);
        PInfo[0].objPartner.GetComponent<Char_Status>().setItemSlots(1, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + Player_Inventory.Instance.getPlayerSlot() + 1]);

        PInfo[1].objPartner.GetComponent<Char_Status>().setItemSlots(0, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + Player_Inventory.Instance.getPlayerSlot() + Player_Inventory.Instance.getPartner1Slot()]);
        PInfo[1].objPartner.GetComponent<Char_Status>().setItemSlots(1, Player_Inventory.Instance.m_lSlot[(Player_Inventory.Instance.ver * Player_Inventory.Instance.hor) + Player_Inventory.Instance.getPlayerSlot() + Player_Inventory.Instance.getPartner1Slot() + 1]);

        objEnemy.GetComponent<Char_Status>().CharStatusSetting(DBManager.EnemyData[m_nEnemyID]);
        objEnemy.GetComponent<Char_Status>().SetSuperArmor(true);

        PInfo[0].PartnerCam.transform.parent = Partner1Obj.transform;
        PInfo[0].PartnerCam.transform.localPosition = new Vector3(0, 1.4f, 1.5f);
        PInfo[0].PartnerCam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        PInfo[1].PartnerCam.transform.parent = Partner2Obj.transform;
        PInfo[1].PartnerCam.transform.localPosition = new Vector3(0, 1.4f, 1.5f);
        PInfo[1].PartnerCam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        Camera.main.GetComponent<CameraPos>().Target = GameManager.Instance.objPlayer;
    }

    void UpdateUI()
    {
        Char_Status CS = GameManager.Instance.objPlayer.GetComponent<Char_Status>();

        //Player
        PlayerHPBar.value = CS.getHP();
        PlayerMPBar.value = CS.getMP();
        PlayerShieldBar.value = CS.getIdentityPoint();
        PlayerHPBar.maxValue = CS.getHPMax();
        PlayerMPBar.maxValue = CS.getMPMax();
        PlayerShieldBar.maxValue = CS.getIdentityPointMax();

        PlayerSkillCoolTime[0].value = 100 - CS.getSkill1CoolTimer() / 5 * 100;
        PlayerSkillCoolTime[1].value = 100 - CS.getSkill2CoolTimer() / 7 * 100;
        PlayerSkillCoolTime[2].value = 100 - CS.getSkill3CoolTimer() / 15 * 100;
        PlayerSkillCoolTime[3].value = 100 - CS.getSkill4CoolTimer() / 5 * 100;

        PlayerSkillCoolTime[0].maxValue = 100;
        PlayerSkillCoolTime[1].maxValue = 100;
        PlayerSkillCoolTime[2].maxValue = 100;
        PlayerSkillCoolTime[3].maxValue = 100;

        //Partner1
        PInfo[0].PartnerHPBar.value = PInfo[0].objPartner.GetComponent<Char_Status>().getHP();
        PInfo[0].PartnerHPBar.maxValue = PInfo[0].objPartner.GetComponent<Char_Status>().getHPMax();

        PInfo[0].PartnerMPBar.value = PInfo[0].objPartner.GetComponent<Char_Status>().getMP();
        PInfo[0].PartnerMPBar.maxValue = PInfo[0].objPartner.GetComponent<Char_Status>().getMPMax();


        //Partner2
        PInfo[1].PartnerHPBar.value = PInfo[1].objPartner.GetComponent<Char_Status>().getHP();
        PInfo[1].PartnerHPBar.maxValue = PInfo[1].objPartner.GetComponent<Char_Status>().getHPMax();

        PInfo[1].PartnerMPBar.value = PInfo[1].objPartner.GetComponent<Char_Status>().getMP();
        PInfo[1].PartnerMPBar.maxValue = PInfo[1].objPartner.GetComponent<Char_Status>().getMPMax();

        //Enemy
        EnemyHPBar.value = objEnemy.GetComponent<Char_Status>().getHP();
        EnemyHPBar.maxValue = objEnemy.GetComponent<Char_Status>().getHPMax();
        if (objEnemy.GetComponent<Char_Status>().getObjTarget() != null)
            Target.text = "Target : " + objEnemy.GetComponent<Char_Status>().objTarget.GetComponent<Char_Status>().getName()
                + "\n NextPattern : " + objEnemy.GetComponent<Char_Status>().getCS();

        
    }


    void GameEndText()
    {
        if (CharAllDeathCheck())
        {
            m_bGameEnd = true;
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Over";
        }
        if (objEnemy.GetComponent<Char_Status>().getCS() == GameManager.CharState.Death)
        {
            m_bGameEnd = true;
            //objGameEnd.SetActive(true);
            //objGameEndMessage.text = "Game Clear";
        }

    }

    public void GoVillage()
    {
        Player_Inventory.Instance.InventorySave();
        SceneManager.LoadScene("VillageScene");

    }

    //public void NextStage()
    //{
    //    Player_Inventory.Instance.InventorySave();
    //    if (m_nEnemyID == 1)
    //    {
    //        SceneManager.LoadScene("MainScene");
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("EnemyID", m_nEnemyID + 1);
    //        SceneManager.LoadScene("InGameScene");
    //    }

    //}


    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        GameEndText();


        if (m_bGameEnd &&Input.GetKeyDown(KeyCode.B))
        {
            GoVillage();
        }


    }
}
