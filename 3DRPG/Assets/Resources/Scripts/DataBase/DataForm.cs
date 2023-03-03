using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public enum CharState
{
    Idle,
    Move,
    Action,
    Hit,
    Death,
    Stay,
}

[System.Serializable]
public enum ItemType
{
    Empty,
    Wearable,
    Expendalbe
}

[System.Serializable]
public enum ActionState
{
    Attack,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    IdentitySkill,
}


[System.Serializable]
public class TradeNPCData
{
    public int ID;
    //������ ��� DB
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
    //������ ��� DB
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
    // ��ų��
    public string Name;
    // ��ų ���
    public float SDP1;
    public float SDP2;
    // ��ų ��Ÿ��
    public float SCT;
    // ��ų ��� ����
    public int SM;
    // ��ų ����Ʈ ���ҽ�
    public string SER;
    // Ÿ�� ����
    public string ST;
    // ���� ����
    public float SR1;
    public float SR2;
    //��ų �ӵ�
    public float SSPD;
    //��ų ���� �ð�
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
        //ĳ���͸�
        Name = _SkillName;
        // ��ų ���
        SDP1 = _SkillCeofficientPer1;
        SDP2 = _SkillCeofficientPer2;
        // ��ų ��Ÿ��
        SCT = _SkillCoolTime;
        // ��ų ��� ����
        SM = _SkillUsingMana;
        // ��ų ����Ʈ ���� Ȯ��
        // ��ų ����Ʈ ���ҽ�
        SER = _SkillEffectResource;
        // Ÿ�� ����
        ST = _TargetSelect;
        // ���� ����
        SR1 = _SkillRange1;
        SR2 = _SkillRange2;
        //��ų �ӵ�
        SSPD = _SkillSpeed;
        //��ų ���� �ð�
        SLT = _SkillUsingTime;
    }


    // ID
    public int getSkillID()
    {
        return ID;
    }
    // ��ų��
    public string getSkillName()
    {
        return Name;
    }
    // ��ų ���
    public float getSkillCeofficientPer1()
    {
        return SDP1;
    }
    public float getSkillCeofficientPer2()
    {
        return SDP2;
    }
    // ��ų ��Ÿ��
    public float getSkillCoolTime()
    {
        return SCT;
    }
    // ��ų ��� ����
    public int getSkillUsingMana()
    {
        return SM;
    }
    // ��ų ����Ʈ ���ҽ�
    public string getSkillEffectResource()
    {
        return SER;
    }
    // Ÿ�� ����
    public string getTargetSelect()
    {
        return ST;
    }
    // ���� ����
    public float getSkillRange1()
    {
        return SR1;
    }
    public float getSkillRange2()
    {
        return SR2;
    }

    //��ų �ӵ�
    public float getSkillSpeed()
    {
        return SSPD;
    }
    //��ų ���� �ð�
    public float getSkillUsingTime()
    {
        return SLT;
    }
}

/*
[System.Serializable]
public class CharData {
    //ID
    public int ID;
    //�̸�
    public string Name;
    //Ÿ�� ����
    public int TYP;


}

[System.Serializable]
public class PlayerCharData : CharData {

    //���� ������Ʈ ������ ��ġ
    public string PFL;
    //���ݷ�
    public int ATK;
    //����
    public int DEF;
    //HP
    public int HP;
    //MP
    public int MP;
    //�̵��ӵ�
    public float SPD;
    //���� ȸ�� �ӵ�
    public float MPRP;

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    public int SID[0];
    // �� ����, AI ĳ����, �÷��̾� ���
    public int SID[1];
    public int SID[2];
    // �� ����, �÷��̾� ���
    public int SID[3];
    public int SID[4];
    // �÷��̾� ���
    public int ISID;

    //�÷��̾� ���� ��ų
    public int ISP;
    public int ISPRP;
    public float ISPRT;

    public PlayerCharData()
    {
        ID = 0;
        Name = "Empty";
        ATK = 0;
        DEF = 0;
        HP = 0;
        MP = 0;
        SPD = 0;
        MPRP = 0;
        TYP = 0;
        SID[0] = 0;
        SID[1] = 0;
        SID[2] = 0;
        SID[3] = 0;
        SID[4] = 0;
        ISID = 0;
        ISP = 0;
        ISPRP = 0;
        ISPRT = 0;
        PFL = "";
    }



    public PlayerCharData(
        int _ID, string _Name, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer,
        int _AttackID, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID,
        int _IdentitySkillID, int _IdentitySkillPoint, int _IdentitySkillPointRecovery, float _IdentityPointRecoveryTime, string _ObjPrefab)
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
        SID[0] = _AttackID;
        SID[1] = _Skill1ID;
        SID[2] = _Skill2ID;
        SID[3] = _Skill3ID;
        SID[4] = _Skill4ID;
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
    //���� ������Ʈ ������ ��ġ
    public string PFL;
    //���ݷ�
    public int ATK;
    //����
    public int DEF;
    //HP
    public int HP;
    //MP
    public int MP;
    //�̵��ӵ�
    public float SPD;
    //���� ȸ�� �ӵ�
    public float MPRP;

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    public int SID[0];
    // �� ����, AI ĳ����, �÷��̾� ���
    public int SID[1];
    public int SID[2];

    public PartnerCharData()
    {
        ID = 0;
        Name = "Empty";
        ATK = 0;
        DEF = 0;
        HP = 0;
        MP = 0;
        SPD = 0;
        MPRP = 0;
        TYP = 0;
        SID[0] = 0;
        SID[1] = 0;
        SID[2] = 0;
        PFL = "";
    }


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
        SID[0] = _AttackID;
        SID[1] = _Skill1ID;
        SID[2] = _Skill2ID;
        PFL = _ObjPrefab;
    }

}


[System.Serializable]
public class EnemyCharData : CharData
{
    //���� ������Ʈ ������ ��ġ
    public string PFL;
    //���ݷ�
    public int ATK;
    //����
    public int DEF;
    //HP
    public int HP;
    //�̵��ӵ�
    public float SPD;


    //ĳ���� ��ų ID
    // �� ����, AI ĳ����, �÷��̾� ���
    public int SID[1];
    public int SID[2];
    // �� ����, �÷��̾� ���
    public int SID[3];
    public int SID[4];
    // �÷��̾� ���
    public int ISID;

    public EnemyCharData()
    {
        ID = 0;
        Name = "Empty";
        ATK = 0;
        DEF = 0;
        HP = 0;
        SPD = 0;
        TYP = 0;
        SID[1] = 0;
        SID[2] = 0;
        SID[3] = 0;
        SID[4] = 0;
        PFL = "";

    }

    public EnemyCharData(int _ID, string _Name, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID, string _ObjPrefab)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        SPD = _Speed;
        TYP = _Layer;
        SID[1] = _Skill1ID;
        SID[2] = _Skill2ID;
        SID[3] = _Skill3ID;
        SID[4] = _Skill4ID;
        PFL = _ObjPrefab;

    }



}


[System.Serializable]
public class ItemData : CharData
{
    //���ݷ�
    public int ATK;
    //����
    public int DEF;
    //HP
    public int HP;
    //MP
    public int MP;

    //�̹���
    public string IMG;
    public string Mesh;
    public string Material;

    //����
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
        Mesh = "";
        Material = "";
        TYP = 0;
        BG = 0;
        SG = 0;
    }

    public ItemData(int _ID, string _Name, int _ATK, int _DEF, int _HP, int _MP, string _ItemSprite, string _Mesh, string _Material, int _ItemType, int _BG, int _SG)
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
*/



[System.Serializable]
public class CharData
{

    //ID
    public int ID;
    //ĳ���͸�
    public string Name;
    //���ݷ�
    public int ATK;
    //����
    public int DEF;
    //HP
    public int HP;
    //MP
    public int MP;
    //�̵��ӵ�
    public float SPD;
    //���� ȸ�� �ӵ�
    public float MPRP;
    //���̾� ����
    public int TYP;


    //������Ʈ ������ ��ġ
    public string PFL;
    public int[] SID;

    //���� ��ų ����Ʈ
    public int ISP;
    //���� ��ų ȸ����
    public int ISPRP;
    //���� ��ų ȸ���ð�
    public float ISPRT;


    public CharData()
    {

    }


}



[System.Serializable]
public class PlayerCharData : CharData
{
    



    public PlayerCharData()
    {

    }

    public PlayerCharData(
        int _ID, string _Name, int _ATK, int _DEF,int _HP,int _MP,float _Speed,float _MP_Recovery,int _Layer,
        int _AttackID,int _Skill1ID,int _Skill2ID,int _Skill3ID,int _Skill4ID,
        int _IdentitySkillID,int _IdentitySkillPoint, int _IdentitySkillPointRecovery,float _IdentityPointRecoveryTime,string _ObjPrefab)
    {
        SID = new int[5];
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        SPD = _Speed;
        MPRP = _MP_Recovery;
        TYP = _Layer;
        SID[0] = _AttackID;
        SID[1] = _Skill1ID;
        SID[2] = _Skill2ID;
        SID[3] = _Skill3ID;
        SID[4] = _Skill4ID;
        SID[5] = _IdentitySkillID;
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
        SID = new int[3];
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        SPD = _Speed;
        MPRP = _MP_Recovery;
        TYP = _Layer;
        SID[0] = _AttackID;
        SID[1] = _Skill1ID;
        SID[2] = _Skill2ID;
        PFL = _ObjPrefab;
    }

}


[System.Serializable]
public class EnemyCharData : CharData
{


    public EnemyCharData(int _ID, string _Name, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID, string _ObjPrefab)
    {
        SID = new int[4];
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        SPD = _Speed;
        TYP = _Layer;
        SID[0] = _Skill1ID;
        SID[1] = _Skill2ID;
        SID[2] = _Skill3ID;
        SID[3] = _Skill4ID;
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


}
