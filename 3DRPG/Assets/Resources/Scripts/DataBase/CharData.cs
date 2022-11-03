using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class CharData
{
    // Start is called before the first frame update
    protected int ID;
    protected int ATK;
    protected int DEF;
    protected int HP;
    protected int MP;
    protected float Speed;
    protected float MP_Recovery;

    protected int Layer;

    // AI 캐릭터, 플레이어 사용
    protected int AttackID;
    // 적 유닛, AI 캐릭터, 플레이어 사용
    protected int Skill1ID;
    protected int Skill2ID;
    // 적 유닛, 플레이어 사용
    protected int Skill3ID;
    protected int Skill4ID;
    // 플레이어 사용
    protected int IdentitySkillID;
    protected int IdentitySkillPoint;




    public CharData()
    {

    }


}

[System.Serializable]
public class PlayerCharData : CharData
{
    public PlayerCharData(int _ID, int _ATK, int _DEF,int _HP,int _MP,float _Speed,float _MP_Recovery,int _Layer,int _AttackID,int _Skill1ID,int _Skill2ID,int _Skill3ID,int _Skill4ID,int _IdentitySkillID,int _IdentitySkillPoint)
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