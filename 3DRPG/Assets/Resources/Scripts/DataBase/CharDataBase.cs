using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDataBase : MonoBehaviour
{
    public static CharDataBase instance;

    public List<PlayerCharData> m_lPlayerDB = new List<PlayerCharData>();
    public List<PartnerCharData> m_lPartnerDB = new List<PartnerCharData>();
    public List<EnemyCharData> m_lEnemyDB = new List<EnemyCharData>();
    public List<SkillData> m_lSkillDB = new List<SkillData>();
    public List<ItemData> m_lItemDB = new List<ItemData>();

    private void Awake()
    {
        if (CharDataBase.instance == null)
            CharDataBase.instance = this;
    }

    void AddPlayer(
        int _ID, string _Name, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer,
        int _AttackID, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID,
        int _IdentitySkillID,int _IdentitySkillPoint, int _IdentitySkillPointRecovery, float _IdentityPointRecoveryTime, string _ObjPrefab)
    {
        m_lPlayerDB.Add(new PlayerCharData(_ID,_Name, _ATK, _DEF, _HP, _MP, _Speed, _MP_Recovery, _Layer, _AttackID, _Skill1ID, _Skill2ID, _Skill3ID, _Skill4ID, _IdentitySkillID,_IdentitySkillPoint, _IdentitySkillPointRecovery, _IdentityPointRecoveryTime, _ObjPrefab));
    }
    void AddPartner(int _ID, string _Name, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer, int _AttackID, int _Skill1ID, int _Skill2ID, string _ObjPrefab)
    {
        m_lPartnerDB.Add(new PartnerCharData(_ID, _Name, _ATK, _DEF, _HP, _MP, _Speed, _MP_Recovery, _Layer, _AttackID, _Skill1ID, _Skill2ID, _ObjPrefab));
    }
    void AddEnemy(int _ID, string _Name, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID, string _ObjPrefab)
    {
        m_lEnemyDB.Add(new EnemyCharData(_ID, _Name, _ATK, _DEF, _HP, _Speed, _Layer, _Skill1ID, _Skill2ID, _Skill3ID, _Skill4ID, _ObjPrefab));
    }

    void AddSkill(int _SkillID,string _SkillName, float _SkillCeofficientPer1, float _SkillCeofficientPer2, float _SkillCoolTime,int _SkillUsingMana, string _SkillEffectResource,int _TargetSelect, float _SkillRange1,float _SkillRange2,float _SkillSpeed,float _SkillUsingTime)
    {
        m_lSkillDB.Add(new SkillData(_SkillID, _SkillName, _SkillCeofficientPer1, _SkillCeofficientPer2, _SkillCoolTime, _SkillUsingMana, _SkillEffectResource, _TargetSelect, _SkillRange1, _SkillRange2,_SkillSpeed, _SkillUsingTime));
    }

    void AddItem(int _ID, string _Name, int _ATK, int _DEF,  float _Speed, string _ItemSprite, GameManager.ItemType _ItemType)
    {
        m_lItemDB.Add(new ItemData(_ID, _Name, _ATK, _DEF, _Speed,  _ItemSprite, _ItemType));
    }

        // Start is called before the first frame update
        void Start()
    {
        //플레이어 캐릭터
        AddPlayer(0, "기사",2,5,25,100,4,3,6,6,7,8,9,10,15,30,10,10,"Prefabs/Character/Player");
        //AI동료 캐릭터
        AddPartner(1,"사제", 2, 5, 15, 200, 4, 3, 9, 0, 1, 2, "Prefabs/Character/Healer");
        AddPartner(2,"도적", 4, 5, 15, 100, 4, 3, 9, 3, 4, 5, "Prefabs/Character/Thief");
        AddPartner(3,"마법사", 4, 5, 15, 100, 4, 3, 9, 0, 16, 17, "Prefabs/Magician");
        //적 캐릭터
        AddEnemy(10,"붉은 용", 10, 0, 150, 8,8, 11, 12, 13, 14, "Prefabs/Character/Enemy");
        AddEnemy(11,"초록 용", 8, 0, 120, 12,8, 11, 18, 19, 20, "Prefabs/Green");


        //스킬 정보
        // ID, 스킬명, 스킬계수1, 스킬계수2, 스킬 쿨타임, 마나사용량,스킬 리소스 위치, 타겟 레이어 마스크, 스킬범위1, 스킬범위2, 스킬속도, 스킬지속시간
        AddSkill(0,"사제_기본공격",1,0,0,0, "Prefabs/SkillEffect/HealerBullet", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,6,3f);
        AddSkill(1,"사제_단일힐",0.1f,0,5,50, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,0,0);
        AddSkill(2,"사제_전체힐", 0.15f,0,10,100, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10f,0,0,0);
        AddSkill(3,"도적_기본공격",1,2,0,0, "", 1 << (LayerMask.NameToLayer("Enemy")), 45,0,0,0);
        AddSkill(4,"도적_뒤잡기",0,0,5f,40, "", 1 << (LayerMask.NameToLayer("Enemy")), 10f,0,0,3f);
        AddSkill(5,"도적_암살",2,2,0,50, "", 1 << (LayerMask.NameToLayer("Enemy")),45,0,0,0);
        AddSkill(6,"기사_기본공격",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,3f);
        AddSkill(7,"기사_방패공격",2f,0,5,30, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,3f);
        AddSkill(8,"기사_방패돌진",4f,0,7,40, "", 1 << (LayerMask.NameToLayer("Enemy")),0,0,0,3f);
        AddSkill(9,"기사_수호지대",0.5f,0,15f,70, "Prefabs/SkillEffect/ProtectZone", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 30,0,0,10f);
        AddSkill(10,"기사_단일도발",0,0,5f,10, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,0);
        AddSkill(11,"용_단일공격",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,0,0);
        AddSkill(12,"용_각도범위공격",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10,35,0,0);
        AddSkill(13,"용_화염구",1.5f,0,0,0, "Prefabs/SkillEffect/FireBall", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,2,5);
        AddSkill(14,"용_화염숨결",1.5f,0,0,0, "Prefabs/SkillEffect/FireBreath", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 20,20,0,1);
        AddSkill(15, "기사_방패막기", 0.5f, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Enemy")), 0, 0, 0, 0);
        //임시
        AddSkill(16, "마법사_화염구", 2f, 0, 5, 50, "Prefabs/SkillEffect/FireBall", 1 << (LayerMask.NameToLayer("Enemy")), 0, 0, 6, 5);
        AddSkill(17, "마법사_화염숨결", 3f, 0, 7, 80, "Prefabs/SkillEffect/FireBreath", 1 << (LayerMask.NameToLayer("Enemy")), 20, 20, 0, 1);
        AddSkill(18, "용_각도범위공격", 2, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 30, 0, 0);
        AddSkill(19, "용_각도범위공격", 1.5f, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 180, 0, 0);
        AddSkill(20, "용_각도범위공격", 1, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 360, 0, 0);


        //Item Manager
        AddItem(0, "빈 아이템",0,0,0,"",GameManager.ItemType.Empty);
        AddItem(1, "검",10,0,0, "Texture/Sword", GameManager.ItemType.Wearable);
        AddItem(1, "방패",0,5,0, "Texture/Shield", GameManager.ItemType.Wearable);

    }

}
