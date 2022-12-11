using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class SkillData
{
    // ID
    int ID;
    // ��ų��
    string Name;
    // ��ų ���
    float SDP1;
    float SDP2;
    // ��ų ��Ÿ��
    float SCT;
    // ��ų ��� ����
    int SM;
    // ��ų ����Ʈ ���ҽ�
    string SER;
    // Ÿ�� ����
    int ST;
    // ���� ����
    float SR1;
    float SR2;
    //��ų �ӵ�
    float SSPD;
    //��ų ���� �ð�
    float SLT;

    public SkillData(
    int _SkillID,
    string _SkillName,
    float _SkillCeofficientPer1,
    float _SkillCeofficientPer2,
    float _SkillCoolTime,
    int _SkillUsingMana,
    string _SkillEffectResource,
    int _TargetSelect,
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
    public int getTargetSelect()
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


[System.Serializable]
public class CharData
{


    //ID
    protected int ID;
    //ĳ���͸�
    protected string Name;
    //���� ������Ʈ ������ ��ġ
    protected string PFL;
    //���ݷ�
    protected int ATK;
    //����
    protected int DEF;
    //HP
    protected int HP;
    //MP
    protected int MP;
    //�̵��ӵ�
    protected float SPD;
    //���� ȸ�� �ӵ�
    protected float MPRP;
    //���̾� ����
    protected int TYP;

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    protected int S0ID;
    // �� ����, AI ĳ����, �÷��̾� ���
    protected int S1ID;
    protected int S2ID;
    // �� ����, �÷��̾� ���
    protected int S3ID;
    protected int S4ID;
    // �÷��̾� ���
    protected int ISID;

    //�÷��̾� ���� ��ų
    protected int ISP;
    protected int ISPRP;
    protected float ISPRT;



    public CharData()
    {

    }

    //ID
    public int getID()
    {
        return ID;
    }
    //ĳ���͸�
    public string getName()
    {
        return Name;
    }
    //���ݷ�
    public int getATK()
    {
        return ATK;
    }
    //����
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
    //�̵��ӵ�
    public float getSpeed()
    {
        return SPD;
    }
    //���� ȸ�� �ӵ�
    public float getMP_Recovery()
    {
        return MPRP;
    }
    //���̾� ����
    public int getLayer()
    {
        return TYP;
    }

    //�÷��̾� ���� ��ų
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

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    public int getAttackID()
    {
        return S0ID;
    }
    // �� ����, AI ĳ����, �÷��̾� ���
    public int getSkill1ID()
    {
        return S1ID;
    }
    public int getSkill2ID()
    {
        return S2ID;
    }
    // �� ����, �÷��̾� ���
    public int getSkill3ID()
    {
        return S3ID;
    }
    public int getSkill4ID()
    {
        return S4ID;
    }
    // �÷��̾� ���
    public int getIdentitySkillID()
    {
        return ISID;
    }

    //���� ������Ʈ ������ ��ġ
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
    protected string IMG;
    public ItemData()
    {
        ID = 0;
        Name = "Empty";
        ATK = 0;
        DEF = 0;
        SPD = 0;
        IMG = "";
        TYP = 0;
    }

    public ItemData(int _ID, string _Name, int _ATK, int _DEF, float _Speed,string _ItemSprite,int _ItemType)
    {
        ID = _ID;
        Name = _Name;
        ATK = _ATK;
        DEF = _DEF;
        SPD = _Speed;
        IMG = _ItemSprite;
        TYP = _ItemType;
    }

    public string getItemSprite()
    {
        return IMG;
    }

    public int getItemType()
    {
        return TYP;
    }
}