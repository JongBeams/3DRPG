using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameSceneManager : MonoBehaviour
{
    public InGameSceneManager instance;


    public struct PartnerInfo {

        public GameObject objPartner;
        public Slider PartnerHPBar;
        public Slider PartnerMPBar;
        public Camera Partner1Cam;
    }

    [Header("PlayerUI")]
    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;
    public Slider PlayerSkillQCoolTime;
    public Slider PlayerSkillWCoolTime;
    public Slider PlayerSkillECoolTime;
    public Slider PlayerSkillRCoolTime;

    [Header("Partner")]
    public PartnerInfo[] PInfo = new PartnerInfo[2];

    [Header("Enemy")]
    public GameObject objEnemy;
    public Slider EnemyHPBar;
    public Text Target;

    [Header("UI")]
    public GameObject objGameEnd;
    public Text objGameEndMessage;
    bool m_bGameEnd = false;
    public int m_nEnemyID = 0;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        getInstanceChar();

        Player_Inventory.Instance.InventoryLoad();
    }


    void getInstanceChar()
    {
        m_nEnemyID = PlayerPrefs.GetInt("EnemyID");
        //
        GameObject PlayerObj = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[PlayerPrefs.GetInt("Player")].getObjPrefab()), new Vector3(0, 0, -15), Quaternion.identity);
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

        objPlayer.GetComponent<Char_Status>().setItemSlots(0, PI.m_lSlot[(PI.ver * PI.hor)]);
        objPlayer.GetComponent<Char_Status>().setItemSlots(1, PI.m_lSlot[(PI.ver * PI.hor) + 1]);

        objHealer.GetComponent<Char_Status>().setItemSlots(0, PI.m_lSlot[(PI.ver * PI.hor) + PI.getPlayerSlot()]);
        objHealer.GetComponent<Char_Status>().setItemSlots(1, PI.m_lSlot[(PI.ver * PI.hor) + PI.getPlayerSlot() + 1]);

        objThief.GetComponent<Char_Status>().setItemSlots(0, PI.m_lSlot[(PI.ver * PI.hor) + PI.getPlayerSlot() + PI.getPartner1Slot()]);
        objThief.GetComponent<Char_Status>().setItemSlots(1, PI.m_lSlot[(PI.ver * PI.hor) + PI.getPlayerSlot() + PI.getPartner1Slot() + 1]);

        objEnemy.GetComponent<Char_Status>().CharStatusSetting(DBManager.EnemyData[m_nEnemyID]);
        objEnemy.GetComponent<Char_Status>().SetSuperArmor(true);

        Partner1Cam.transform.parent = Partner1Obj.transform;
        Partner1Cam.transform.localPosition = new Vector3(0, 1.4f, 1.5f);
        Partner1Cam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        Partner2Cam.transform.parent = Partner2Obj.transform;
        Partner2Cam.transform.localPosition = new Vector3(0, 1.4f, 1.5f);
        Partner2Cam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    void UpdateUI()
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
        if (objEnemy.GetComponent<Char_Status>().getObjTarget() != null)
            Target.text = "Target : " + objEnemy.GetComponent<Char_Status>().objTarget.GetComponent<Char_Status>().getName()
                + "\n NextPattern : " + objEnemy.GetComponent<Char_Status>().getCS();

        if (PI.objInventory.transform.GetChild(3).gameObject.activeSelf == true)
        {
            //Debug.LogError(PI.objInventory.transform.GetChild(3).gameObject);
            //Debug.LogError(PI.objInventory.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text);
            PI.objInventory.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Gold : " + Gold;
        }
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
        if ((objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death && objPlayer.GetComponent<Char_Status>().getCS() == CharState.Death) || (objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death && m_nEnemyID == 1))
        {
            objGameEnd.SetActive(true);
            objGameEndMessage.text = "Game Clear";
        }

    }

    //public void GoMain()
    //{

    //    if (m_nEnemyID != 1 && objEnemy.GetComponent<Char_Status>().getCS() == CharState.Death)
    //    {
    //        PlayerPrefs.SetInt("EnemyID", m_nEnemyID + 1);
    //        SceneManager.LoadScene("InGameScene");
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene("MainScene");
    //    }

    //}

    //public void NextStage()
    //{
    //    PI.InventorySave();
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
    }
}
