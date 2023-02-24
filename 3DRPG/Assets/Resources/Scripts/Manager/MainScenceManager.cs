using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainScenceManager : MonoBehaviour
{


    public void newGameStart()
    {
        PlayerPrefs.SetInt("GameSet",0);
        SceneManager.LoadScene("VillageScene");
    }
    public void ContinueGameStart()
    {
        if (File.Exists(Application.dataPath + "/Save/InventoryData.json"))
        {
            PlayerPrefs.SetInt("GameSet", 1);
            SceneManager.LoadScene("VillageScene");
        }
        else{
            Debug.LogError(Application.dataPath + "/Save/InventoryData.json");
            Debug.LogError(File.Exists(Application.dataPath + "/Save/InventoryData.json"));
        }
        
    }

    public void SetResolution()
    {
        int setWidth = 1280; // 사용자 설정 너비
        int setHeight = 720; // 사용자 설정 높이

        Screen.SetResolution(setWidth,setHeight,false);
    }

        // Start is called before the first frame update
        void Start()
    {
        DBManager.Instance.CallDBManager();
        //GameManager.Instance.
        SetResolution();

        //GameManager.Instance.m_nScreenIdx = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
