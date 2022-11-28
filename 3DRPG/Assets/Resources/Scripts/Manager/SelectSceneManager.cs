using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{


    public int[] CharID = new int[3];

    public Button[] CharButton;
    public Button[] CharSelectButton;
    public GameObject SelectUI;

    public Text PlayerName;
    public Text[] PartnerName;

    public int addID = 0;


    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < CharID.Length; i++)
        {
            CharID[i] = 0; 
        }


        //for (int i = 0; i < CharButton.Length; i++)
        //{
        //    CharButton[i].onClick.AddListener(() => SelectScreen(i));
        //}

        //for (int i = 0; i < CharSelectButton.Length; i++)
        //{
        //    CharSelectButton[i].onClick.AddListener(() => CharIDButton(i));
        //}

        SelectUI.SetActive(false);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("InGameScene");

        PlayerPrefs.SetInt("Player", CharID[0]);
        PlayerPrefs.SetInt("Partner1", CharID[1]);
        PlayerPrefs.SetInt("Partner2", CharID[2]);
    }


    public void SelectScreen(int num)
    {
        addID = num;
        SelectUI.SetActive(true);
    }

    public void CharIDButton(int num)
    {
        CharID[addID] = num;
        SelectUI.SetActive(false);

        if (addID == 0)
        {
            PlayerName.text = CharDataBase.instance.m_lPlayerDB[CharID[addID]].getName();
        }
        else
        {
            PartnerName[addID - 1].text = CharDataBase.instance.m_lPartnerDB[CharID[addID]].getName();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //PlayerName.text = CharDataBase.instance.m_lPlayerDB[CharID[0]].getName();
        //PartnerName[0].text = CharDataBase.instance.m_lPartnerDB[CharID[1]].getName();
        //PartnerName[1].text = CharDataBase.instance.m_lPartnerDB[CharID[2]].getName();
    }
}
