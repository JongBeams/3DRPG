using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;



public class DBManager : MonoSingleton<DBManager>
{
    private string m_sSaveFileDirectory;  // 저장할 폴더 경로



    public static List<PlayerCharData> PlayerData = new List<PlayerCharData>();
    public static List<PartnerCharData> PartnerData = new List<PartnerCharData>();
    public static List<EnemyCharData> EnemyData = new List<EnemyCharData>();
    public static List<SkillData> SkillData = new List<SkillData>();
    public static List<ItemData> ItemData = new List<ItemData>();

    //static List<GameData> DropListData = new List<GameData>();



    // Start is called before the first frame update
    void Awake()
    {
        m_sSaveFileDirectory = Application.dataPath + "/DataBase/";
        ReadData();
        
    }

    static T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var tmp = JsonConvert.DeserializeObject<T>(json);

        return tmp;
    }

    //람다식 + Linq 공부할것
    public static CharData GetPlayerStatusByIdx(int idx)
    {
        return DeepCopy(PlayerData.Find(x => x.getID() == idx));
    }

    public void ReadData()
    {


        if (File.Exists(m_sSaveFileDirectory+ "PlayerDataTable.json"))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + "PlayerDataTable.json");

            PlayerData = JsonConvert.DeserializeObject<List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (File.Exists(m_sSaveFileDirectory + "PartnerDataTable.json"))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + "PartnerDataTable.json");

            PlayerData = JsonConvert.DeserializeObject< List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (File.Exists(m_sSaveFileDirectory + "EnemyDataTable.json"))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + "PlayerDataTable.json");

            PlayerData = JsonConvert.DeserializeObject< List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (File.Exists(m_sSaveFileDirectory + "SkillDataTable.json"))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + "SkillDataTable.json");

            PlayerData = JsonConvert.DeserializeObject< List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (File.Exists(m_sSaveFileDirectory + "ItemDataTable.json"))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + "ItemDataTable.json");

            PlayerData = JsonConvert.DeserializeObject< List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

    }

    


}
