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


    public static List<ItemDropData> DropItemData = new List<ItemDropData>();




    //static List<GameData> DropListData = new List<GameData>();

    public void CallDBManager()
    {
        Debug.Log("Call DBManager");
    }


    // Start is called before the first frame update
    void Awake()
    {
        m_sSaveFileDirectory = "/DataBase/";
        ReadData();




        Debug.Log(DropItemData[0].IDP[0]);
    }

    static T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var tmp = JsonConvert.DeserializeObject<T>(json);

        return tmp;
    }

    //람다식 + Linq 공부할것
    public static PlayerCharData GetPlayerStatusByIdx(int idx)
    {
        return DeepCopy(PlayerData.Find(x => x.getID() == idx));
    }

    public static PartnerCharData GetPartnerStatusByIdx(int idx)
    {
        return DeepCopy(PartnerData.Find(x => x.getID() == idx));
    }
    public static EnemyCharData GetEnemyStatusByIdx(int idx)
    {
        return DeepCopy(EnemyData.Find(x => x.getID() == idx));
    }
    public static SkillData GetSkillStatusByIdx(int idx)
    {
        return DeepCopy(SkillData.Find(x => x.getSkillID() == idx));
    }
    public static ItemData GetItemStatusByIdx(int idx)
    {
        return DeepCopy(ItemData.Find(x => x.getID() == idx));
    }
    public static ItemDropData GetItemDropDataByIdx(int idx)
    {
        return DeepCopy(DropItemData.Find(x => x.ID == idx));
    }



    public void ReadData()
    {
        

        if (Resources.Load<TextAsset>("DataBase/PlayerDataTable")!=null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/PlayerDataTable").text;

            PlayerData = JsonConvert.DeserializeObject<List<PlayerCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/PartnerDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/PartnerDataTable").text;

            PartnerData = JsonConvert.DeserializeObject<List<PartnerCharData>>(jdata);

        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/EnemyDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/EnemyDataTable").text;

            EnemyData = JsonConvert.DeserializeObject<List<EnemyCharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/SkillDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/SkillDataTable").text;

            SkillData = JsonConvert.DeserializeObject<List<SkillData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/ItemDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/ItemDataTable").text;

            ItemData = JsonConvert.DeserializeObject<List<ItemData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/ItemDropTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/ItemDropTable").text;


            DropItemData = JsonConvert.DeserializeObject<List<ItemDropData>>(jdata);


            //ItemData = JsonConvert.DeserializeObject<List<ItemData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }




    }




}
