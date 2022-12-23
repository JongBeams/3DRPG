using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class TradeNPCData
{
    public int ID;
    //아이템 드랍 DB
    public List<int> TI;

    TradeNPCData()
    {
        TI = new List<int>();

    }
}

[System.Serializable]
public class ItemDropData
{
    public int ID;
    //아이템 드랍 DB
    public List<int> IDT;
    public List<float> IDP;

    ItemDropData()
    {
        IDT = new List<int>();
        IDP = new List<float>();
    }
}

[System.Serializable]
public class SkillData
{
    // ID
    public int ID;
    // 스킬명
    public string Name;
    // 스킬 계수
    public float SDP1;
    public float SDP2;
    // 스킬 쿨타임
    public float SCT;
    // 스킬 사용 마나
    public int SM;
    // 스킬 이펙트 리소스
    public string SER;
    // 타깃 구분
    public string ST;
    // 범위 지정
    public float SR1;
    public float SR2;
    //스킬 속도
    public float SSPD;
    //스킬 지속 시간
    public float SLT;

    public SkillData(
    int _SkillID,
    string _SkillName,
    float _SkillCeofficientPer1,
    float _SkillCeofficientPer2,
    float _SkillCoolTime,
    int _SkillUsingMana,
    string _SkillEffectResource,
    string _TargetSelect,
    float _SkillRange1,
    float _SkillRange2,
    float _SkillSpeed,
    float _SkillUsingTime
        )
    {
        // ID
        ID = _SkillID;
        //캐릭터명
        Name = _SkillName;
        // 스킬 계수
        SDP1 = _SkillCeofficientPer1;
        SDP2 = _SkillCeofficientPer2;
        // 스킬 쿨타임
        SCT = _SkillCoolTime;
        // 스킬 사용 마나
        SM = _SkillUsingMana;
        // 스킬 이펙트 유무 확인
        // 스킬 이펙트 리소스
        SER = _SkillEffectResource;
        // 타깃 구분
        ST = _TargetSelect;
        // 범위 지정
        SR1 = _SkillRange1;
        SR2 = _SkillRange2;
        //스킬 속도
        SSPD = _SkillSpeed;
        //스킬 지속 시간
        SLT = _SkillUsingTime;
    }


    // ID
    public int getSkillID()
    {
        return ID;
    }
    // 스킬명
    public string getSkillName()
    {
        return Name;
    }
    // 스킬 계수
    public float getSkillCeofficientPer1()
    {
        return SDP1;
    }
    public float getSkillCeofficientPer2()
    {
        return SDP2;
    }
    // 스킬 쿨타임
    public float getSkillCoolTime()
    {
        return SCT;
    }
    // 스킬 사용 마나
    public int getSkillUsingMana()
    {
        return SM;
    }
    // 스킬 이펙트 리소스
    public string getSkillEffectResource()
    {
        return SER;
    }
    // 타깃 구분
    public string getTargetSelect()
    {
        return ST;
    }
    // 범위 지정
    public float getSkillRange1()
    {
        return SR1;
    }
    public float getSkillRange2()
    {
        return SR2;
    }

    //스킬 속도
    public float getSkillSpeed()
    {
        return SSPD;
    }
    //스킬 지속 시간
    public float getSkillUsingTime()
    {
        return SLT;
    }
}


[System.Serializable]
public class CharData
{


    //ID
    public int ID;
    //캐릭터명
    public string Name;
    //게임 오브젝트 프리펩 위치
    public string PFL;
    //공격력
    public int ATK;
    //방어력
    public int DEF;
    //HP
    public int HP;
    //MP
    public int MP;
    //이동속도
    public float SPD;
    //마나 회복 속도
    public float MPRP;
    //레이어 구분
    public int TYP;

    //캐릭터 스킬 ID
    // AI 캐릭터, 플레이어 사용
    public int S0ID;
    // 적 유닛, AI 캐릭터, 플레이어 사용
    public int S1ID;
    public int S2ID;
    // 적 유닛, 플레이어 사용
    public int S3ID;
    public int S4ID;
    // 플레이어 사용
    public int ISID;

    //플레이어 고유 스킬
    public int ISP;
    public int ISPRP;
    public float ISPRT;



    public CharData()
    {

    }

    //ID
    public int getID()
    {
        return ID;
    }
    //캐릭터명
    public string getName()
    {
        return Name;
    }
    //공격력
    public int getATK()
    {
        return ATK;
    }
    //방어력
    public int getDEF()
    {
        return DEF;
    }
    //HP
    public int getHP()
    {
        return HP;
    }
    //MP
    public int getMP()
    {
        return MP;
    }
    //이동속도
    public float getSpeed()
    {
        return SPD;
    }
    //마나 회복 속도
    public float getMP_Recovery()
    {
        return MPRP;
    }
    //레이어 구분
    public int getTYP()
    {
        return TYP;
    }

    //플레이어 고유 스킬
    public int getIdentitySkillPoint()
    {
        return ISP;
    }
    public int getIdentitySkillPointRecovery()
    {
        return ISPRP;
    }
    public float getIdentityPointRecoveryTime()
    {
        return ISPRT;
    }

    //캐릭터 스킬 ID
    // AI 캐릭터, 플레이어 사용
    public int getAttackID()
    {
        return S0ID;
    }
    // 적 유닛, AI 캐릭터, 플레이어 사용
    public int getSkill1ID()
    {
        return S1ID;
    }
    public int getSkill2ID()
    {
        return S2ID;
    }
    // 적 유닛, 플레이어 사용
    public int getSkill3ID()
    {
        return S3ID;
    }
    public int getSkill4ID()
    {
        return S4ID;
    }
    // 플레이어 사용
    public int getIdentitySkillID()
    {
        return ISID;
    }

    //게임 오브젝트 프리펩 위치
    public string getObjPrefab()
    {
        return PFL;
    }

}



[System.Serializable]
public class PlayerCharData : CharData
{
    public PlayerCharData(
        int _ID, string _Name, int _ATK, int _DEF,int _HP,int _MP,float _Speed,float _MP_Recovery,int _Layer,
        int _AttackID,int _Skill1ID,int _Skill2ID,int _Skill3ID,int _Skill4ID,
        int _IdentitySkillID,int _IdentitySkillPoint, int _IdentitySkillPointRecovery,float _IdentityPointRecoveryTime,string _ObjPrefab)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        SPD = _Speed;
        MPRP = _MP_Recovery;
        TYP = _Layer;
        S0ID = _AttackID;
        S1ID = _Skill1ID;
        S2ID = _Skill2ID;
        S3ID = _Skill3ID;
        S4ID = _Skill4ID;
        ISID = _IdentitySkillID;
        ISP = _IdentitySkillPoint;
        ISPRP = _IdentitySkillPointRecovery;
        ISPRT = _IdentityPointRecoveryTime;
        PFL = _ObjPrefab;
    }

    

}

[System.Serializable]
public class PartnerCharData : CharData
{
    public PartnerCharData(int _ID, string _Name, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer, int _AttackID, int _Skill1ID, int _Skill2ID, string _ObjPrefab)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        SPD = _Speed;
        MPRP = _MP_Recovery;
        TYP = _Layer;
        S0ID = _AttackID;
        S1ID = _Skill1ID;
        S2ID = _Skill2ID;
        PFL = _ObjPrefab;
    }

}


[System.Serializable]
public class EnemyCharData : CharData
{

    public EnemyCharData(int _ID, string _Name, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID, string _ObjPrefab)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        SPD = _Speed;
        TYP = _Layer;
        S1ID = _Skill1ID;
        S2ID = _Skill2ID;
        S3ID = _Skill3ID;
        S4ID = _Skill4ID;
        PFL = _ObjPrefab;

    }



}


[System.Serializable]
public class ItemData : CharData
{
    public string IMG;
    public string Mesh;
    public string Material;

    public int BG;
    public int SG;


    public ItemData()
    {
        ID = 0;
        Name = "Empty";
        ATK = 0;
        DEF = 0;
        HP = 0;
        MP = 0;
        IMG = "";
        Mesh="";
        Material="";
        TYP = 0;
        BG = 0;
        SG = 0;
    }

    public ItemData(int _ID, string _Name, int _ATK, int _DEF, int _HP,int _MP, string _ItemSprite, string _Mesh, string _Material, int _ItemType,int _BG,int _SG)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        IMG = _ItemSprite;
        Mesh = _Mesh;
        Material = _Material;
        TYP = _ItemType;
        BG = _BG;
        SG = _SG;
    }

    public string getItemSprite()
    {
        return IMG;
    }
    public string getItemMesh()
    {
        return Mesh;
    }
    public string getItemMaterial()
    {
        return Material;
    }

}