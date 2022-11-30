using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{


    public int[] CharID = new int[3];

    public Button[] NextButton;
    public Button[] PreviousButton;


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

        PlayerName.text = CharDataBase.instance.m_lPlayerDB[CharID[0]].getName();
        PartnerName[0].text = CharDataBase.instance.m_lPartnerDB[CharID[0]].getName();
        PartnerName[1].text = CharDataBase.instance.m_lPartnerDB[CharID[1]].getName();
        //for (int i = 0; i < CharButton.Length; i++)
        //{
        //    CharButton[i].onClick.AddListener(() => SelectScreen(i));
        //}

        //for (int i = 0; i < CharSelectButton.Length; i++)
        //{
        //    CharSelectButton[i].onClick.AddListener(() => CharIDButton(i));
        //}

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

        if (ID == 0)
        {
            
                if (CharID[ID] <= 0)
                {
                    CharID[ID] = CharDataBase.instance.m_lPlayerDB.Count-1;
                }
                else
                {
                    CharID[ID]--;
                }
                
            
            PlayerName.text = CharDataBase.instance.m_lPlayerDB[CharID[ID]].getName();
        }
        else
        {

            if (CharID[ID] <= 0)
            {
                CharID[ID] = CharDataBase.instance.m_lPartnerDB.Count-1;
            }
            else
            {
                CharID[ID]--;
            }

            
            PartnerName[ID - 1].text = CharDataBase.instance.m_lPartnerDB[CharID[ID]].getName();
        }
    }

    public void NextChar(int ID)
    {

      

        if (ID == 0)
        {
            
                if (CharID[ID] >= CharDataBase.instance.m_lPlayerDB.Count-1)
                {
                    CharID[ID] = 0;
                }
                else
                {
                    CharID[ID]++;
                }

           
            
            PlayerName.text = CharDataBase.instance.m_lPlayerDB[CharID[ID]].getName();
        }
        else
        {
            
                if (CharID[ID] >= CharDataBase.instance.m_lPartnerDB.Count-1)
                {
                    CharID[ID] = 0;
                }
                else
                {
                    CharID[ID]++;
                }
            
            PartnerName[ID - 1].text = CharDataBase.instance.m_lPartnerDB[CharID[ID]].getName();
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
