using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Char_Base : MonoBehaviour 
{
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }


    #region 스테이터스

    [Header("스테이터스")]
    public CharData CharStatus;
    public int m_nPlayerHP;
    public int m_nPlayerMP;
    public float m_fPlayerMPRecoveryTimer;

    public int m_nIdentityPoint;

    public float m_fDRper;

    [Header("상태")]
    public CharState CS;
    public GameObject objTarget;
    public Vector3 vecMovePoint;
    public bool m_bProtectBuff = false;
    public bool m_bSuperArmor = false;
    public bool m_bTaunt = false;
    public bool[] m_bCheck;

    [Header("컴포넌트")]
    protected Transform AttackPos;
    protected Animator animator;
    protected NavMeshAgent agent;

    [Header("장비")]
    public ItemSlot[] itemSlots;

    [Header("스킬")]
    public float[] m_fSkillCoolTimer;
    public bool[] m_bSkillOn;
    public bool[] m_bSkillUsing;
    public int[] m_nTargetLayer;
    public int m_nActionIdx;
    public string strActionAniName;

    
    public void SetComponents()
    {
        animator = this.GetComponent<Animator>();
        agent=this.GetComponent<NavMeshAgent>();
        objTarget = null;
        AttackPos = this.transform.GetChild(0);
        CS = CharState.Idle;
        delGetDamage = GetDamage;
        m_fPlayerMPRecoveryTimer = 5;
        m_nTargetLayer = new int[2];
        m_nActionIdx = 0;
        m_bProtectBuff = false;
        m_bSuperArmor = false;
        m_bTaunt = false;

        if (CharStatus.TYP == LayerMask.NameToLayer("Player"))
        {

            m_fSkillCoolTimer = new float[5];
            m_bSkillOn = new bool[5];
            m_bSkillUsing = new bool[5];
            m_nTargetLayer[0] = 1 << (LayerMask.NameToLayer("Enemy"));
            m_nTargetLayer[1] = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));


            return;
        }
        if (CharStatus.TYP == LayerMask.NameToLayer("Partner"))
        {

            m_fSkillCoolTimer = new float[3];
            m_bSkillOn = new bool[3];
            m_bSkillUsing = new bool[3];
            m_nTargetLayer[0] = 1 << (LayerMask.NameToLayer("Enemy"));
            m_nTargetLayer[1] = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
            return;
        }
        if (CharStatus.TYP == LayerMask.NameToLayer("Enemy"))
        {

            m_fSkillCoolTimer = new float[4];
            m_bSkillOn = new bool[4];
            m_bSkillUsing = new bool[4];
            m_nTargetLayer[0] = 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Partner"));
            m_nTargetLayer[1] = 1 << (LayerMask.NameToLayer("Enemy"));
            return;
        }
    }

    protected bool CheckEndAni(string _aniName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_aniName) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            return true;
        else
            return false;
    }

    protected void SetCheck(int idx)
    {
        m_bCheck = new bool[idx];
        for (int i =0;i<idx;i++)
        {
            m_bCheck[i] = false;
        }
    }

    protected void SetSkillCoolTime()
    {
        m_bSkillOn[m_nActionIdx] = false;
        m_fSkillCoolTimer[m_nActionIdx] = DBManager.SkillData[m_nActionIdx].SCT;
    }

    #endregion

    #region 쿨타임

    public void SkillCooTimer()
    {
        for (int i = 0; i < m_bSkillOn.Length; i++)
        {
            if (m_bSkillOn[i])
                continue;

            if (m_fSkillCoolTimer[i] <= 0)
            {
                m_bSkillOn[i] = true;
            }
            else
            {
                m_fSkillCoolTimer[i] -= Time.deltaTime;
            }
        }

    }
    public abstract void Recovery();

    #endregion

    #region 상태머신

    public abstract void SetCharStatus(CharState _CS);// 한번 실행

    public abstract void UpdateCharStatus();// 지속 실행

    #endregion

    #region 변동(피격, 회복)

    public delegate void Del(int AddIndex);
    public Del delGetDamage = null;

    public void HealingHP(int HealingPoint)
    {
        if (HealingPoint > 0)
            m_nPlayerHP += HealingPoint;

        if (m_nPlayerHP > CharStatus.HP)
        {
            m_nPlayerHP = CharStatus.HP;
        }
    }

    public void GetDamage(int _Damage)
    {
        if (CS != CharState.Death)
        {
            int totalDamage = (int)((_Damage - CharStatus.DEF) * (1 - m_fDRper));

            if (!m_bSuperArmor)
            {
                SetCharStatus(CharState.Hit);

            }
            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
            //m_nLastGetDamage = totalDamage;
            m_nPlayerHP -= totalDamage;

        }
    }

    #endregion

    #region 버프, 디버프
    public void OnDrBuff(float _Time, float _DRper)
    {
        StartCoroutine(OnDRBuffCoroutine(_Time, _DRper));
    }

    IEnumerator OnDRBuffCoroutine(float _Time, float _DRper)
    {
        m_bProtectBuff = true;
        m_fDRper = _DRper;
        yield return new WaitForSeconds(_Time);
        m_fDRper = 0;
        m_bProtectBuff = false;
    }
    public void OnTaunt(float _Time, GameObject _Target)
    {
        StartCoroutine(OnOnTauntCoroutine(_Time, _Target));
    }

    IEnumerator OnOnTauntCoroutine(float _Time, GameObject _Target)
    {
        m_bTaunt = true;
        objTarget = _Target;
        yield return new WaitForSeconds(_Time);
        m_bTaunt = false;
    }

    public void UseIdentitiy(int _Cost)
    {

        if (m_nIdentityPoint <= 0)
        {
            m_nIdentityPoint = 0;
        }
        else
        {
            m_nIdentityPoint -= _Cost;
        }

    }

    #endregion

    #region 위치


    public Vector3 PlayerLookingPoint()
    {
        return new Vector3(vecMovePoint.x, this.transform.position.y, vecMovePoint.z);
    }

    //protected void LookingTarget()
    //{
    //    // 타깃 바라보기
    //    Vector3 vecEnemyLookingPoint;
    //    if (CharStatus.TYP == LayerMask.NameToLayer("Player"))
    //    {
    //        vecEnemyLookingPoint = new Vector3(vecMovePoint.x, transform.position.y, vecMovePoint.z);
    //    }
    //    else
    //    {
    //        vecEnemyLookingPoint = new Vector3(objTarget.transform.position.x, transform.position.y, objTarget.transform.position.z);
    //    }
    //    transform.LookAt(vecEnemyLookingPoint);
    //}

    #endregion

    #region 알고리즘

    #endregion

}
