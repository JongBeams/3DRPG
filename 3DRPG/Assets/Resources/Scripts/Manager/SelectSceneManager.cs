using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{

    [Header("CharSelectUI")]
    public int[] CharID = new int[3];
    public Button[] NextButton;
    public Button[] PreviousButton;
    public Text[] CharName;
    public int addID = 0;
    public GameObject[] CharObj=new GameObject[3];
    public GameObject objCharSelectUI;

    [Header("PlayerUI")]
    public Slider PlayerHPBar;
    public Slider PlayerMPBar;
    public Slider PlayerShieldBar;
    public Slider PlayerSkillQCoolTime;
    public Slider PlayerSkillWCoolTime;
    public Slider PlayerSkillECoolTime;
    public Slider PlayerSkillRCoolTime;



    // Start is called before the first frame update
    void Start()
    {
        SetCharSelectUI();
        GameManager.Instance.GetInstancePlayerChar(new Vector3 (0, 0, 0));
    }


    void SetCharSelectUI()
    {
        for (int i = 0; i < CharID.Length; i++)
        {
            CharID[i] = 0;
        }

        CharName[0].text = DBManager.PlayerData[CharID[0]].getName();
        CharName[1].text = DBManager.PartnerData[CharID[0]].getName();
        CharName[2].text = DBManager.PartnerData[CharID[1]].getName();

        GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[0]].getObjPrefab()), new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Player.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[0] = Player;
        GameObject Partner1 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].getObjPrefab()), new Vector3(10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Partner1.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[1] = Partner1;
        GameObject Partner2 = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[0]].getObjPrefab()), new Vector3(20, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
        Partner2.GetComponent<Rigidbody>().isKinematic = true;
        CharObj[2] = Partner2;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("InGameScene");

        PlayerPrefs.SetInt("Player", CharID[0]);
        PlayerPrefs.SetInt("Partner1", CharID[1]);
        PlayerPrefs.SetInt("Partner2", CharID[2]);



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
            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].getObjPrefab()), new Vector3(ID * 10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
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
            
            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].getObjPrefab()), new Vector3(ID*10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;
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

            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PlayerData[CharID[ID]].getObjPrefab()), new Vector3(ID * 10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
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
            GameObject Player = Instantiate(Resources.Load<GameObject>(DBManager.PartnerData[CharID[ID]].getObjPrefab()), new Vector3(ID * 10, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0)));
            Player.GetComponent<Rigidbody>().isKinematic = true;
            CharObj[ID] = Player;
            CharName[ID].text = DBManager.PartnerData[CharID[ID]].getName();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //PlayerName.text = DBManager.PlayerData[CharID[0]].getName();
        //PartnerName[0].text = DBManager.PartnerData[CharID[1]].getName();
        //PartnerName[1].text = DBManager.PartnerData[CharID[2]].getName();
    }
}
