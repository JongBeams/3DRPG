using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Char_Status : MonoBehaviour
{
    public delegate void Del(int Damage);
    public Del delGetDamae = null;


    //ID
    int m_nCharID = 0;
    //캐릭터명
    string m_sName = "";
    //공격력
    int m_nATK = 3;
    //방어력
    int m_nDEF = 0;
    //HP
    public int m_nPlayerHP = 10;
    int m_nPlayerHPMax = 10;
    //MP
    int m_nPlayerMP = 100;
    int m_nPlayerMPMax = 100;
    //이동속도
    float m_fPlayerSpeed = 5;
    //마나 회복량
    float m_fPlayerMPRecoveryPoint = 30;
    //마나 회복 속도
    float m_fPlayerMPRecoveryTimer = 5;

    //캐릭터 구분 레이어
    int m_nLayer = 0;

    //플레이어 고유 스킬
    int m_nIdentityPoint = 0;
    int m_nIdentityPointtMax = 0;
    int m_fIdentityPointRecovery = 0;

    float m_fIdentityPointRecoveryTimer = 0;
    float m_fIdentityPointRecoveryTime = 0;

    //데미지 감소율
    float m_fDRper = 0;


    //SkillID
    int m_nAttackID = 0;
    int[] m_nSkillID = new int[4];
    int m_nIdentitySkillID = 0;


    //int m_nLastGetDamage=0;

    //캐릭터 상태
    public GameManager.CharState CS;

    //타겟
    public GameObject objTarget;
    //공격 위치
    Transform AttackPos;
    //애니메이션(메카님 애니메이션을 통한 제어)
    Animator animator;

    public ItemSlot[] itemSlots;

    // 상태 체크
    bool m_bProtectBuff = false;
    bool m_bSuperArmor = false;
    bool m_bTaunt = false;


    //스킬 쿨 타임
    float m_fSkill1CoolTimer = 0;
    float m_fSkill2CoolTimer = 0;
    float m_fSkill3CoolTimer = 0;
    float m_fSkill4CoolTimer = 0;

    //Skill 사용 가능 여부
    bool m_bSkill1On = false;
    bool m_bSkill2On = false;
    bool m_bSkill3On = false;
    bool m_bSkill4On = false;




    //Skill 사용중 상태 체크
    bool[] m_bSkillUsing = new bool[4];
    bool m_bIdentitySkillUsing = false;


    //bool Check : 캐릭터 별 체크 사함
    bool m_bCheck01 = false; //Healer = RunAway Dist Check //Thief = BackPos
    bool m_bCheck02 = false; //Healer = Target Enemy Check




    //get
    public GameManager.CharState getCS()
    {
        return CS;
    }
    public GameObject getObjTarget()
    {
        return objTarget;
    }
    public Transform getAttackPos()
    {
        return AttackPos;
    }
    public Animator getAnimator()
    {
        return animator;
    }
    //public int getLastGetDamage()
    //{
    //    return m_nLastGetDamage;
    //}


    //get Skill On
    public bool getSkill1On()
    {
        return m_bSkill1On;
    }
    public bool getSkill2On()
    {
        return m_bSkill2On;
    }
    public bool getSkill3On()
    {
        return m_bSkill3On;
    }
    public bool getSkill4On()
    {
        return m_bSkill4On;
    }

    public bool getIdentitySkillUsing()
    {
        return m_bIdentitySkillUsing;
    }

    //get status
    public int getID()
    {
        return m_nCharID;
    }
    public int getATK()
    {
        if (m_nLayer == 8)
        {
            return m_nATK;
        }
        else
        {
            return itemSlots[0].item.getATK() + itemSlots[1].item.getATK() + m_nATK;
        }
        
    }
    public int getDEF()
    {
        if (m_nLayer == 8)
        {
            return m_nDEF;
        }
        else
        {
            return itemSlots[0].item.getDEF() + itemSlots[1].item.getDEF() + m_nDEF;
        }
        
    }
    public int getHP()
    {
        return m_nPlayerHP;
    }
    public int getHPMax()
    {
        return m_nPlayerHPMax;
    }
    public int getMP()
    {
        return m_nPlayerMP;
    }
    public int getMPMax()
    {
        return m_nPlayerMPMax;
    }
    public float getSpeed()
    {
        if (m_nLayer == 8)
        {
            return m_fPlayerSpeed;
        }
        else
        {
            return itemSlots[0].item.getSpeed() + itemSlots[1].item.getSpeed() + m_fPlayerSpeed;
        }
        
    }
    public int getIdentityPoint()
    {
        return m_nIdentityPoint;
    }
    public int getIdentityPointMax()
    {
        return m_nIdentityPointtMax;
    }

    public int getLayer()
    {
        return m_nLayer;
    }

    public string getName()
    {
        return m_sName;
    }



    // get ID
    public int getAttackID()
    {
        return m_nAttackID;
    }
    public int[] getSkillID()
    {
        return m_nSkillID;
    }
    public int getIdentitySkillID()
    {
        return m_nIdentitySkillID;
    }


    //get Skill Cool Time
    public float getSkill1CoolTimer()
    {
        return m_fSkill1CoolTimer;
    }
    public float getSkill2CoolTimer()
    {
        return m_fSkill2CoolTimer;
    }
    public float getSkill3CoolTimer()
    {
        return m_fSkill3CoolTimer;
    }
    public float getSkill4CoolTimer()
    {
        return m_fSkill4CoolTimer;
    }


    //get Skill Using Checking State
    public bool[] getSkillUsing()
    {
        return m_bSkillUsing;
    }



    //get bool Check
    public bool getCheck01()
    {
        return m_bCheck01;
    }
    public bool getCheck02()
    {
        return m_bCheck02;
    }

    public bool getTaunt() 
    {
        return m_bTaunt;
    }





    //set
    public void setCS(GameManager.CharState _CS)
    {
        CS = _CS;
    }
    public void setItemSlotsNum(int num)
    {
        itemSlots = new ItemSlot[num];
    }
    public void setItemSlots(int num,ItemSlot _ItmeSlot)
    {
        itemSlots[num] = _ItmeSlot;
    }

    //set CharStatus
    public void CharStatusSetting(CharData _chardata)
    {
        //ID
        m_nCharID = _chardata.getID();
        //캐릭터명
        m_sName = _chardata.getName();
        //공격력
        m_nATK = _chardata.getATK();
        //방어력
        m_nDEF = _chardata.getDEF();
        //HP
        m_nPlayerHPMax = _chardata.getHP();
        m_nPlayerHP = m_nPlayerHPMax;
        //이동속도
        m_fPlayerSpeed = _chardata.getSpeed();

        //LayerNum
        m_nLayer = _chardata.getLayer();
        this.gameObject.layer = m_nLayer;


        if (_chardata.getLayer() == 6)
        {
            //MP
            m_nPlayerMPMax = _chardata.getMP();
            m_nPlayerMP = m_nPlayerMPMax;


            //마나 회복 양
            m_fPlayerMPRecoveryPoint = _chardata.getMP_Recovery();

            //playerIdentitySkill
            m_nIdentityPointtMax = _chardata.getIdentitySkillPoint();
            m_nIdentityPoint = m_nIdentityPointtMax;
            m_fIdentityPointRecovery = _chardata.getIdentitySkillPointRecovery();

            m_fIdentityPointRecoveryTime = _chardata.getIdentityPointRecoveryTime();
            m_fIdentityPointRecoveryTimer = m_fIdentityPointRecoveryTime;

            //SkillID
            m_nAttackID = _chardata.getAttackID();
            m_nSkillID[0] = _chardata.getSkill1ID();
            m_nSkillID[1] = _chardata.getSkill2ID();
            m_nSkillID[2] = _chardata.getSkill3ID();
            m_nSkillID[3] = _chardata.getSkill4ID();
            m_nIdentitySkillID = _chardata.getIdentitySkillID();
        }
        if (_chardata.getLayer() == 8)
        {
            //SkillID
            m_nSkillID[0] = _chardata.getSkill1ID();
            m_nSkillID[1] = _chardata.getSkill2ID();
            m_nSkillID[2] = _chardata.getSkill3ID();
            m_nSkillID[3] = _chardata.getSkill4ID();


        }
        if (_chardata.getLayer() == 9)
        {
            //MP
            m_nPlayerMPMax = _chardata.getMP();
            m_nPlayerMP = m_nPlayerMPMax;

            //마나 회복 속도
            m_fPlayerMPRecoveryPoint = _chardata.getMP_Recovery();

            //playerIdentitySkill
            m_nIdentityPointtMax = _chardata.getIdentitySkillPoint();
            m_nIdentityPoint = m_nIdentityPointtMax;
            m_fIdentityPointRecovery = _chardata.getIdentitySkillPointRecovery();

            m_fIdentityPointRecoveryTime = _chardata.getIdentityPointRecoveryTime();
            m_fIdentityPointRecoveryTimer = m_fIdentityPointRecoveryTime;

            //SkillID
            m_nAttackID = _chardata.getAttackID();
            m_nSkillID[0] = _chardata.getSkill1ID();
            m_nSkillID[1] = _chardata.getSkill2ID();
        }


    }


    //set Skill Cool Time
    public void setSkill1CoolTimer(float _Timer)
    {
        m_fSkill1CoolTimer = _Timer;
    }
    public void setSkill2CoolTimer(float _Timer)
    {
        m_fSkill2CoolTimer = _Timer;
    }
    public void setSkill3CoolTimer(float _Timer)
    {
        m_fSkill3CoolTimer = _Timer;
    }
    public void setSkill4CoolTimer(float _Timer)
    {
        m_fSkill4CoolTimer = _Timer;
    }

    //set Skill On
    public void setSkill1On(bool _check)
    {
        m_bSkill1On = _check;
    }
    public void setSkill2On(bool _check)
    {
        m_bSkill2On = _check;

    }
    public void setSkill3On(bool _check)
    {
        m_bSkill3On = _check;

    }
    public void setSkill4On(bool _check)
    {
        m_bSkill4On = _check;

    }

    public void setIdentitySkillUsing(bool _IdentitySkillUsing)
    {
        m_bIdentitySkillUsing = _IdentitySkillUsing;
    }


    //set Skill Using Checking State
    public void setSkillUsing(int num,bool Using)
    {
        m_bSkillUsing[num] = Using;
    }



    //set bool
    public void setCheck01(bool _check)
    {
        m_bCheck01 = _check;
    }
    public void setCheck02(bool _check)
    {
        m_bCheck02 = _check;
    }

    public void SetSuperArmor(bool _SuperArmor)
    {
        m_bSuperArmor = _SuperArmor;
    }



    public void HealingHP(int HealingPoint)
    {
        if (HealingPoint > 0)
            m_nPlayerHP += HealingPoint;

        if (m_nPlayerHP > m_nPlayerHPMax)
        {
            m_nPlayerHP = m_nPlayerHPMax;
        }
    }


    public void SetObjTarget(GameObject _objTarget)
    {
        if (CS == GameManager.CharState.Idle)
            objTarget = _objTarget;
    }


    public void GetDamage(int _Damege)
    {
        if (CS !=GameManager.CharState.Death)
        {
            int totalDamage = (int)((_Damege - m_nDEF)*(1-m_fDRper));

            if (!m_bSuperArmor && !m_bIdentitySkillUsing)
            {
                gameObject.GetComponent<Char_Dynamics>().SetCharStatus(GameManager.CharState.Hit);
                
            }
            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
            //m_nLastGetDamage = totalDamage;
            m_nPlayerHP -= totalDamage;

        }
    }


    public void UseMana(int _Cost)
    {
        if (m_nPlayerMP >= _Cost)
        {
            m_nPlayerMP -= _Cost;
        }
        else
        {
            Debug.Log("No Mana");
        }
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

    void SkillCoolTimer()
    {
        if (m_fSkill1CoolTimer <= 0)
        {
            m_bSkill1On = true;
        }
        else
        {
            m_fSkill1CoolTimer -= Time.deltaTime;
        }

        if (m_fSkill2CoolTimer <= 0)
        {
            m_bSkill2On = true;
        }
        else
        {
            m_fSkill2CoolTimer -= Time.deltaTime;
        }

        if (m_fSkill3CoolTimer <= 0)
        {
            m_bSkill3On = true;
        }
        else
        {
            m_fSkill3CoolTimer -= Time.deltaTime;
        }

        if (m_fSkill4CoolTimer <= 0)
        {
            m_bSkill4On = true;
        }
        else
        {
            m_fSkill4CoolTimer -= Time.deltaTime;
        }
    }

    void Recovery()
    {
        if (m_nPlayerMP < m_nPlayerMPMax)
        {
            if (m_fPlayerMPRecoveryTimer <= 0)
            {
                m_nPlayerMP += (int)m_fPlayerMPRecoveryPoint;
                m_fPlayerMPRecoveryTimer = 5;
            }
            else
            {
                m_fPlayerMPRecoveryTimer -= Time.deltaTime;
            }
        }

        if (m_nIdentityPoint < m_nIdentityPointtMax)
        {
            if (m_fIdentityPointRecoveryTimer <= 0)
            {
                m_nIdentityPoint += m_fIdentityPointRecovery;
                m_fIdentityPointRecoveryTimer = m_fIdentityPointRecoveryTime;
            }
            else
            {
                m_fIdentityPointRecoveryTimer -= Time.deltaTime;
            }
        }
    }

    //void SkillEffectTimer()
    //{
    //    if (m_bProtectBuff)
    //    {
    //        if (m_fProtectBuffTimer <= 0)
    //        {
    //            //Debug.Log("offBuff");
    //            m_nProtectPer = 0;
    //            m_bProtectBuff = false;
    //        }
    //        m_fProtectBuffTimer -= Time.deltaTime;
    //    }
    //}

    public void OnDrBuff(float _Time, float _DRper)
    {
        StartCoroutine(OnDRBuffCoroutine(_Time, _DRper));
    }

    IEnumerator OnDRBuffCoroutine(float _Time,float _DRper)
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




    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        objTarget = null;
        AttackPos = this.transform.GetChild(0);
        //m_nPlayerHP = m_nPlayerHPMax;
        //m_nIdentityPoint = m_nIdentityPointtMax;
        CS=GameManager.CharState.Idle;
        
        delGetDamae = GetDamage;

        for(int i = 0; i < m_bSkillUsing.Length; i++)
        {
            m_bSkillUsing[i] =false;
        }
        



    }

    // Update is called once per frame
    void Update()
    {

        SkillCoolTimer();
        Recovery();
        //SkillEffectTimer();

        

        

    }
}
