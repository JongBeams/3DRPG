using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDataBase : MonoBehaviour
{
    public static CharDataBase instance;

    public List<PlayerCharData> m_lPlayerDB = new List<PlayerCharData>();
    public List<PartnerCharData> m_lPartnerDB = new List<PartnerCharData>();
    public List<EnemyCharData> m_lEnemyDB = new List<EnemyCharData>();

    private void Awake()
    {
        if (CharDataBase.instance == null)
            CharDataBase.instance = this;
    }

    void AddPlayer(
        int _ID, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer,
        int _AttackID, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID,
        int _IdentitySkillID,int _IdentitySkillPoint, int _IdentitySkillPointRecovery, float _IdentityPointRecoveryTime)
    {
        m_lPlayerDB.Add(new PlayerCharData(_ID, _ATK, _DEF, _HP, _MP, _Speed, _MP_Recovery, _Layer, _AttackID, _Skill1ID, _Skill2ID, _Skill3ID, _Skill4ID, _IdentitySkillID,_IdentitySkillPoint, _IdentitySkillPointRecovery, _IdentityPointRecoveryTime));
    }
    void AddPartner(int _ID, int _ATK, int _DEF, int _HP, int _MP, float _Speed, float _MP_Recovery, int _Layer, int _AttackID, int _Skill1ID, int _Skill2ID)
    {
        m_lPartnerDB.Add(new PartnerCharData(_ID, _ATK, _DEF, _HP, _MP, _Speed, _MP_Recovery, _Layer, _AttackID, _Skill1ID, _Skill2ID));
    }
    void AddEnemy(int _ID, int _ATK, int _DEF, int _HP, float _Speed, int _Layer, int _Skill1ID, int _Skill2ID, int _Skill3ID, int _Skill4ID)
    {
        m_lEnemyDB.Add(new EnemyCharData(_ID, _ATK, _DEF, _HP, _Speed, _Layer, _Skill1ID, _Skill2ID, _Skill3ID, _Skill4ID));
    }

    // Start is called before the first frame update
    void Start()
    {
        //플레이어 캐릭터
        AddPlayer(0,20,5,25,100,4,3,6,6,7,8,9,10,11,30,5,5);
        //AI동료 캐릭터
        AddPartner(1, 4, 5, 25, 100, 4, 3, 9, 0, 1, 2);
        AddPartner(2, 4, 5, 25, 100, 4, 3, 9, 3, 4, 5);
        //적 캐릭터
        AddEnemy(10, 4, 5, 25, 4,8, 1, 2, 3, 4);

    }

}
