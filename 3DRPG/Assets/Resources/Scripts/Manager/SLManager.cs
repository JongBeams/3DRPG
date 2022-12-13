using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;


public class GameData
{

    public List<int> ItemID = new List<int>();
    public List<string> ItemName = new List<string>();
    public List<int> ItemATK = new List<int>();
    public List<int> ItemDEF = new List<int>();
    public List<float> ItemSPD = new List<float>();
    public List<string> ItemSprite = new List<string>();
    public List<string> ItemMesh = new List<string>();
    public List<string> ItemMaterial = new List<string>();
    public List<int> ItemType = new List<int>();



    public GameData( List<int> _ItemID, List<string> _ItemName, List<int> _ItemATK, List<int> _ItemDEF, List<float> _ItemSPD, List<string> _ItemSprite, List<string> _ItemMesh, List<string> _ItemMaterial, List<int> _ItemType)
    {
        ItemID = _ItemID;
        ItemName = _ItemName;
        ItemATK = _ItemATK;
        ItemDEF = _ItemDEF;
        ItemSPD = _ItemSPD;
        ItemSprite = _ItemSprite;
        ItemMesh= _ItemMesh;
        ItemMaterial= _ItemMaterial;
        ItemType = _ItemType;
    }

}

public class SLManager : MonoBehaviour
{

    private string m_sSaveFileDirectory;  // 저장할 폴더 경로
    private string m_sSaveFileName = "/InventoryData.json"; // 파일 이름

    public GameData InvetoryData;



    public void RemoteStart()
    {
        m_sSaveFileDirectory = Application.dataPath + "/Save/";
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;

        if (!Directory.Exists(m_sSaveFileDirectory)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(m_sSaveFileDirectory); // 폴더 생성(경로 생성)


        _load();
        _save();

        //string jdata = JsonConvert.SerializeObject(gd);

    }



    public GameData getGData()
    {
        return InvetoryData;
    }


    public void _reset()
    {

        //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;
        File.Delete(filecheck);
    }



    public void _save()
    {



        //string jdata = JsonConvert.SerializeObject(InvetoryData, Formatting.Indented);

        //TextAsset textAsset = Resources.Load<TextAsset>("CharacterStatus");
        //characterStatusList = JsonConvert.DeserializeObject<List<Status>>(textAsset.text);

        //List 못읽음
        string jdata = JsonUtility.ToJson(InvetoryData);

        File.WriteAllText(m_sSaveFileDirectory+m_sSaveFileName, jdata);



        //ColObj.GetComponent<SpriteRenderer>().color = Color.blue;
    }



    public void _load()
    {



        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;


        //JObject
        //Debug.Log(File.Exists(filecheck) +"   " + jdata);

        if (File.Exists(filecheck))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + m_sSaveFileName);



            InvetoryData = JsonConvert.DeserializeObject<GameData>(jdata);
            //GameManager.instance.getSaveLoad().gData = gData;
            //Debug.Log("파일 불러오기");



        }
        else
        {
            
            InvetoryData = new GameData(new List<int>(),new List<string>(),new List<int>(),new List<int>(),new List<float>(),new List<string>(), new List<string>(), new List<string>(), new List<int>());




            //Debug.Log("파일 새로 생성");




        }

    }


}