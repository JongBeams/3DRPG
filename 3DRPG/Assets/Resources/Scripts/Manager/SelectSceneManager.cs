using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    }

    void setNPC()
    {
        objNPC[0].GetComponent<NpcEvent>().NS = new QuestNPC();
        objNPC[0].GetComponent<NpcEvent>().NS.EventUI = objEnemySelectUI;

        objNPC[1].GetComponent<NpcEvent>().NS = new TradeNPC();
        objNPC[1].GetComponent<NpcEvent>().NS.EventUI = TradeUI.Instance.objInventory;
    }




    void SetCharSelectUI()
    {
        //Char
        for (int i = 0; i < CharID.Length; i++)
        {
            CharID[i] = 0;
        }

        CharName[0].text = DBManager.PlayerData[CharID[0]].getName();
        CharName[1].text = DBManager.PartnerData[CharID[0]].getName();
        CharName[2].text = DBManager.PartnerData[CharID[1]].getName();

        GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[0]].getObjPrefab()), new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)) );
        Player.transform.parent = objBackGround.transform;
        Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
        Player.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[0] = Player;
        GameObject Partner1 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].getObjPrefab()), new Vector3(10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Partner1.transform.parent = objBackGround.transform;
        Partner1.transform.localPosition = new Vector3(0, 0, -1);
        Partner1.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[1] = Partner1;
        GameObject Partner2 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].getObjPrefab()), new Vector3(20, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));

        Partner2.transform.parent = objBackGround.transform;
        Partner2.transform.localPosition = new Vector3(0.3f, 0, -1);
        Partner2.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[2] = Partner2;

        objCharSelectUI.SetActive(false);

        //Enemy
        EnemyID = 0;
        EnemyName.text = DBManager.EnemyData[EnemyID].getName();
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
            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].getObjPrefab()), new Vector3(ID, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.transform.parent = objBackGround.transform;
            Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;

            CharName[ID].text = DBManager.PlayerData[CharID[ID]].getName();
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
            
            GameObject Partner = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].getObjPrefab()), new Vector3(ID*10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Partner.transform.parent = objBackGround.transform;
            Partner.transform.localPosition = new Vector3((ID - 1) * 0.3f, 0, -1);
            Partner.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Partner;
            CharName[ID].text = DBManager.PartnerData[CharID[ID]].getName();
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

            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].getObjPrefab()), new Vector3(ID, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.transform.parent = objBackGround.transform;
            Player.transform.localPosition = new Vector3(-0.3f, 0, -1);
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;

            CharName[ID].text = DBManager.PlayerData[CharID[ID]].getName();
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
            GameObject Partner = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].getObjPrefab()), new Vector3(ID * 10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Partner.transform.parent = objBackGround.transform;
            Partner.transform.localPosition = new Vector3((ID-1) * 0.3f, 0, -1);
            Partner.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Partner;
            CharName[ID].text = DBManager.PartnerData[CharID[ID]].getName();
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
        EnemyName.text = DBManager.EnemyData[EnemyID].getName();
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
        EnemyName.text = DBManager.EnemyData[EnemyID].getName();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerName.text = DBManager.PlayerData[CharID[0]].getName();
        //PartnerName[0].text = DBManager.PartnerData[CharID[1]].getName();
        //PartnerName[1].text = DBManager.PartnerData[CharID[2]].getName();
    }
}
