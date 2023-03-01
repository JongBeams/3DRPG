using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class SelectSceneManager : MonoBehaviour
{
    public static SelectSceneManager Instance;

    [Header("CharSelectUI")]
    public int[] CharID = new int[3];
    public Button[] NextButton;
    public Button[] PreviousButton;
    public Text[] CharName;
    public int addID = 0;
    public GameObject[] CharObj=new GameObject[3];
    public GameObject objCharSelectUI;
    public GameObject objBackGround;

    [Header("PlayerUI")]
    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;
    public Slider[] PlayerSkillCoolTime=new Slider[4];

    [Header("UI")]
    public GameObject objCanvas;
    GraphicRaycaster GR;
    PointerEventData ped;

    [Header("EnemySelectUI")]
    public int EnemyID = 0;
    public Button EneymNButton;
    public Button EneymPButton;
    public Text EnemyName;
    public GameObject objEnemySelectUI;

    [Header("NPC")]
    public GameObject[] objNPC;


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


        SetCharSelectUI();

        SLManager.Instance.RemoteStart();

        Player_Inventory.Instance.RemoteStart();


        if (PlayerPrefs.GetInt("GameSet")==0) {
            Player_Inventory.Instance.InventorySave();
        }
        else if(PlayerPrefs.GetInt("GameSet") == 1)
        {
            Player_Inventory.Instance.InventoryLoad();
        }

        TradeUI.Instance.RemoteStart();

        GameManager.Instance.GetInstancePlayerChar(new Vector3 (0, 0, 0));

        setNPC();
        GameManager.Instance.m_nScreenIdx = SceneManager.GetActiveScene().buildIndex;

    }

    void setNPC()
    {
        objNPC[0].GetComponent<NpcEvent>().NS = new QuestNPC();
        objNPC[0].GetComponent<NpcEvent>().NS.EventUI = objEnemySelectUI;

        objNPC[1].GetComponent<NpcEvent>().NS = new TradeNPC();
        objNPC[1].GetComponent<NpcEvent>().NS.EventUI = TradeUI.Instance.objInventory;
    }


    void UpdateUI()
    {
        Char_Base CS = GameManager.Instance.objPlayer.GetComponent<Char_Base>();

        //Player
        PlayerHPBar.value = CS.m_nPlayerHP;
        PlayerMPBar.value = CS.m_nPlayerMP;
        PlayerShieldBar.value = CS.m_nIdentityPoint;
        PlayerHPBar.maxValue = CS.CharStatus.HP;
        PlayerMPBar.maxValue = CS.CharStatus.MP;
        PlayerShieldBar.maxValue = CS.CharStatus.ISP;

        //Debug.Log(PlayerSkillCoolTime[0].value);
        //Debug.Log(100f - CS.m_fSkillCoolTimer[1] / 5f * 100f);

        PlayerSkillCoolTime[0].value = 100f - CS.m_fSkillCoolTimer[1] / 5f * 100f;
        PlayerSkillCoolTime[1].value = 100f - CS.m_fSkillCoolTimer[2] / 7f * 100f;
        PlayerSkillCoolTime[2].value = 100f - CS.m_fSkillCoolTimer[3] / 15f * 100f;
        PlayerSkillCoolTime[3].value = 100f - CS.m_fSkillCoolTimer[4] / 5f * 100f;

        PlayerSkillCoolTime[0].maxValue = 100f;
        PlayerSkillCoolTime[1].maxValue = 100f;
        PlayerSkillCoolTime[2].maxValue = 100f;
        PlayerSkillCoolTime[3].maxValue = 100f;

    }

    void SetCharSelectUI()
    {
        //Char
        for (int i = 0; i < CharID.Length; i++)
        {
            CharID[i] = 0;
        }

        CharName[0].text = DBManager.PlayerData[CharID[0]].Name;
        CharName[1].text = DBManager.PartnerData[CharID[0]].Name;
        CharName[2].text = DBManager.PartnerData[CharID[1]].Name;

        GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[0]].PFL), new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)) );
        Player.GetComponent<NavMeshAgent>().enabled = false;
        Player.transform.parent = objBackGround.transform;
        Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
        Player.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[0] = Player;
        GameObject Partner1 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].PFL), new Vector3(10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Partner1.GetComponent<NavMeshAgent>().enabled = false;
        Partner1.transform.parent = objBackGround.transform;
        Partner1.transform.localPosition = new Vector3(0, 0, -1);
        Partner1.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[1] = Partner1;
        GameObject Partner2 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].PFL), new Vector3(20, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Partner2.GetComponent<NavMeshAgent>().enabled = false;
        Partner2.transform.parent = objBackGround.transform;
        Partner2.transform.localPosition = new Vector3(0.3f, 0, -1);
        Partner2.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[2] = Partner2;

        objCharSelectUI.SetActive(false);

        //Enemy
        EnemyID = 0;
        EnemyName.text = DBManager.EnemyData[EnemyID].Name;
        objEnemySelectUI.SetActive(false);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("InGameScene");

        
        PlayerPrefs.SetInt("Player", CharID[0]);
        PlayerPrefs.SetInt("Partner1", CharID[1]);
        PlayerPrefs.SetInt("Partner2", CharID[2]);

        PlayerPrefs.SetInt("Enemy",EnemyID);

    }

    public void SelectChar()
    {
        objEnemySelectUI.SetActive(false);
        objCharSelectUI.SetActive(true);
    }

    public void PreviousChar(int ID)
    {
        Destroy(CharObj[ID]);
        if (ID == 0)
        {
            
                if (CharID[ID] <= 0)
                {
                    CharID[ID] = DBManager.PlayerData.Count-1;
                }
                else
                {
                    CharID[ID]--;
                }
            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].PFL), new Vector3(ID, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.GetComponent<NavMeshAgent>().enabled = false;
            Player.transform.parent = objBackGround.transform;
            Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;

            CharName[ID].text = DBManager.PlayerData[CharID[ID]].Name;
        }
        else
        {

            if (CharID[ID] <= 0)
            {
                CharID[ID] = DBManager.PartnerData.Count-1;
            }
            else
            {
                CharID[ID]--;
            }
            
            GameObject Partner = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].PFL), new Vector3(ID*10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Partner.GetComponent<NavMeshAgent>().enabled = false;
            Partner.transform.parent = objBackGround.transform;
            Partner.transform.localPosition = new Vector3((ID - 1) * 0.3f, 0, -1);
            Partner.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Partner;
            CharName[ID].text = DBManager.PartnerData[CharID[ID]].Name;
        }

        

    }

    public void NextChar(int ID)
    {


        Destroy(CharObj[ID]);
        if (ID == 0)
        {
            
                if (CharID[ID] >= DBManager.PlayerData.Count-1)
                {
                    CharID[ID] = 0;
                }
                else
                {
                    CharID[ID]++;
                }

            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].PFL), new Vector3(ID, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.GetComponent<NavMeshAgent>().enabled = false;
            Player.transform.parent = objBackGround.transform;
            Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;

            CharName[ID].text = DBManager.PlayerData[CharID[ID]].Name;
        }
        else
        {
            
                if (CharID[ID] >= DBManager.PartnerData.Count-1)
                {
                    CharID[ID] = 0;
                }
                else
                {
                    CharID[ID]++;
                }
            GameObject Partner = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].PFL), new Vector3(ID * 10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Partner.GetComponent<NavMeshAgent>().enabled = false;
            Partner.transform.parent = objBackGround.transform;
            Partner.transform.localPosition = new Vector3((ID-1) * 0.3f, 0, -1);
            Partner.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Partner;
            CharName[ID].text = DBManager.PartnerData[CharID[ID]].Name;
        }
    }


    public void PreviousEnemy()
    {
        if (EnemyID <= 0)
        {
            EnemyID = DBManager.EnemyData.Count - 1;
        }
        else
        {
            EnemyID--;
        }
        EnemyName.text = DBManager.EnemyData[EnemyID].Name;
    }
    public void NextEnemy()
    {
        if (EnemyID >= DBManager.EnemyData.Count - 1)
        {
            EnemyID = 0;
        }
        else
        {
            EnemyID++;
        }
        EnemyName.text = DBManager.EnemyData[EnemyID].Name;
    }

    // Update is called once per frame
    void Update()
    {
        TradeUI.Instance.ClickItem();

        Invoke("UpdateUI",1f);
        //PlayerName.text = DBManager.PlayerData[CharID[0]].getName();
        //PartnerName[0].text = DBManager.PartnerData[CharID[1]].getName();
        //PartnerName[1].text = DBManager.PartnerData[CharID[2]].getName();
    }
}
