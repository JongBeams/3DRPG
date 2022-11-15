using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class CharData
{
    //ID
    protected int ID;
    //���ݷ�
    protected int ATK;
    //����
    protected int DEF;
    //HP
    protected int HP;
    //MP
    protected int MP;
    //�̵��ӵ�
    protected float Speed;
    //���� ȸ�� �ӵ�
    protected float MP_Recovery;
    //���̾� ����
    protected int Layer;

    //�÷��̾� ���� ��ų
    protected int IdentitySkillPoint;
    protected int IdentitySkillPointRecovery;
    protected float IdentityPointRecoveryTime;

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    protected int AttackID;
    // �� ����, AI ĳ����, �÷��̾� ���
    protected int Skill1ID;
    protected int Skill2ID;
    // �� ����, �÷��̾� ���
    protected int Skill3ID;
    protected int Skill4ID;
    // �÷��̾� ���
    protected int IdentitySkillID;

    

    public CharData()
    {

    }

    //ID
    public int getID()
    {
        return ID;
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
        return Speed;
    }
    //���� ȸ�� �ӵ�
    public float getMP_Recovery()
    {
        return MP_Recovery;
    }
    //���̾� ����
    public int getLayer()
    {
        return Layer;
    }

    //�÷��̾� ���� ��ų
    public int getIdentitySkillPoint()
    {
        return IdentitySkillPoint;
    }
    public int getIdentitySkillPointRecovery()
    {
        return IdentitySkillPointRecovery;
    }
    public float getIdentityPointRecoveryTime()
    {
        return IdentityPointRecoveryTime;
    }

    //ĳ���� ��ų ID
    // AI ĳ����, �÷��̾� ���
    public int getAttackID()
    {
        return AttackID;
    }
    // �� ����, AI ĳ����, �÷��̾� ���
    public int getSkill1ID()
    {
        return Skill1ID;
    }
    public int getSkill2ID()
    {
        return Skill2ID;
    }
    // �� ����, �÷��̾� ���
    public int getSkill3ID()
    {
        return Skill3ID;
    }
    public int getSkill4ID()
    {
        return Skill4ID;
    }
    // �÷��̾� ���
    public int getIdentitySkillID()
    {
        return IdentitySkillID;
    }


}

[System.Serializable]
public class PlayerCharData : CharData
{
    public PlayerCharData(
        int _ID, int _ATK, int _DEF,int _HP,int _MP,float _Speed,float _MP_Recovery,int _Layer,
        int _AttackID,int _Skill1ID,int _Skill2ID,int _Skill3ID,int _Skill4ID,
        int _IdentitySkillID,int _IdentitySkillPoint, int _IdentitySkillPointRecovery,float _IdentityPointRecoveryTime)
    {
        ID = _ID;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        Speed = _Speed;
        MP_Recovery = _MP_Recovery;
        Layer = _Layer;
        AttackID = _AttackID;
        Skill1ID = _Skill1ID;
        Skill2ID = _Skill2ID;
        Skill3ID = _Skill3ID;
        Skill4ID = _Skill4ID;
        IdentitySkillID = _IdentitySkillID;
        IdentitySkillPoint = _IdentitySkillPoint;
        IdentitySkillPointRecovery = _IdentitySkillPointRecovery;
        IdentityPointRecoveryTime = _IdentityPointRecoveryTime;
    }

    

}

[System.Serializable]
public class PartnerCharData : CharData
{
    public PartnerCharData(int _ID, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer, int _AttackID, int _Skill1ID, int _Skill2ID)
    {
        ID = _ID;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        MP = _MP;
        Speed = _Speed;
        MP_Recovery = _MP_Recovery;
        Layer = _Layer;
        AttackID = _AttackID;
        Skill1ID = _Skill1ID;
        Skill2ID = _Skill2ID;
    }

}


[System.Serializable]
public class EnemyCharData : CharData
{
    public EnemyCharData(int _ID, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID)
    {
        ID = _ID;
        ATK = _ATK;
        DEF = _DEF;
        HP = _HP;
        Speed = _Speed;
        Layer = _Layer;
        Skill1ID = _Skill1ID;
        Skill2ID = _Skill2ID;
        Skill3ID = _Skill3ID;
        Skill4ID = _Skill4ID;
    }


}