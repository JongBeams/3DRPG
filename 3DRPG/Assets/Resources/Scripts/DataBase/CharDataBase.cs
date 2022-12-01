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
        //�÷��̾� ĳ����
        AddPlayer(0, "���",2,5,25,100,4,3,6,6,7,8,9,10,15,30,10,10,"Prefabs/Character/Player");
        //AI���� ĳ����
        AddPartner(1,"����", 2, 5, 15, 200, 4, 3, 9, 0, 1, 2, "Prefabs/Character/Healer");
        AddPartner(2,"����", 4, 5, 15, 100, 4, 3, 9, 3, 4, 5, "Prefabs/Character/Thief");
        AddPartner(3,"������", 4, 5, 15, 100, 4, 3, 9, 0, 16, 17, "Prefabs/Magician");
        //�� ĳ����
        AddEnemy(10,"���� ��", 10, 0, 150, 8,8, 11, 12, 13, 14, "Prefabs/Character/Enemy");
        AddEnemy(11,"�ʷ� ��", 8, 0, 120, 12,8, 11, 18, 19, 20, "Prefabs/Green");


        //��ų ����
        // ID, ��ų��, ��ų���1, ��ų���2, ��ų ��Ÿ��, ������뷮,��ų ���ҽ� ��ġ, Ÿ�� ���̾� ����ũ, ��ų����1, ��ų����2, ��ų�ӵ�, ��ų���ӽð�
        AddSkill(0,"����_�⺻����",1,0,0,0, "Prefabs/SkillEffect/HealerBullet", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,6,3f);
        AddSkill(1,"����_������",0.1f,0,5,50, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,0,0);
        AddSkill(2,"����_��ü��", 0.15f,0,10,100, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10f,0,0,0);
        AddSkill(3,"����_�⺻����",1,2,0,0, "", 1 << (LayerMask.NameToLayer("Enemy")), 45,0,0,0);
        AddSkill(4,"����_�����",0,0,5f,40, "", 1 << (LayerMask.NameToLayer("Enemy")), 10f,0,0,3f);
        AddSkill(5,"����_�ϻ�",2,2,0,50, "", 1 << (LayerMask.NameToLayer("Enemy")),45,0,0,0);
        AddSkill(6,"���_�⺻����",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,3f);
        AddSkill(7,"���_���а���",2f,0,5,30, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,3f);
        AddSkill(8,"���_���е���",4f,0,7,40, "", 1 << (LayerMask.NameToLayer("Enemy")),0,0,0,3f);
        AddSkill(9,"���_��ȣ����",0.5f,0,15f,70, "Prefabs/SkillEffect/ProtectZone", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 30,0,0,10f);
        AddSkill(10,"���_���ϵ���",0,0,5f,10, "", 1 << (LayerMask.NameToLayer("Enemy")), 0,0,0,0);
        AddSkill(11,"��_���ϰ���",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,0,0);
        AddSkill(12,"��_������������",1,0,0,0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10,35,0,0);
        AddSkill(13,"��_ȭ����",1.5f,0,0,0, "Prefabs/SkillEffect/FireBall", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 0,0,2,5);
        AddSkill(14,"��_ȭ������",1.5f,0,0,0, "Prefabs/SkillEffect/FireBreath", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 20,20,0,1);
        AddSkill(15, "���_���и���", 0.5f, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Enemy")), 0, 0, 0, 0);
        //�ӽ�
        AddSkill(16, "������_ȭ����", 2f, 0, 5, 50, "Prefabs/SkillEffect/FireBall", 1 << (LayerMask.NameToLayer("Enemy")), 0, 0, 6, 5);
        AddSkill(17, "������_ȭ������", 3f, 0, 7, 80, "Prefabs/SkillEffect/FireBreath", 1 << (LayerMask.NameToLayer("Enemy")), 20, 20, 0, 1);
        AddSkill(18, "��_������������", 2, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 30, 0, 0);
        AddSkill(19, "��_������������", 1.5f, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 180, 0, 0);
        AddSkill(20, "��_������������", 1, 0, 0, 0, "", 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner")), 10, 360, 0, 0);


        //Item Manager
        AddItem(0, "�� ������",0,0,0,"",GameManager.ItemType.Empty);
        AddItem(1, "��",10,0,0, "Texture/Sword", GameManager.ItemType.Wearable);
        AddItem(1, "����",0,5,0, "Texture/Shield", GameManager.ItemType.Wearable);

    }

}
