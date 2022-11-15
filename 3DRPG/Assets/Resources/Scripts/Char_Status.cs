using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Char_Status : MonoBehaviour
{
    //ID
    int m_nCharID = 0;
    //공격력
    int m_nATK = 3;
    //방어력
    int m_nDEF = 0;
    //HP
    int m_nPlayerHP = 10;
    int m_nPlayerHPMax = 10;
    //MP
    int m_nPlayerMP = 100;
    int m_nPlayerMPMax = 100;
    //이동속도
    float m_fPlayerSpeed = 5;
    //마나 회복 속도
    float m_nPlayerMPRecoveryTimer = 3;

    //캐릭터 구분 레이어
    int m_nLayer = 0;

    //플레이어 고유 스킬
    int m_nIdentityPoint = 0;
    int m_nIdentityPointtMax = 0;
    int m_fIdentityPointRecovery = 0;

    float m_fIdentityPointRecoveryTimer = 0;
    float m_fIdentityPointRecoveryTime = 0;
    

    //SkillID
    int m_nAttackID = 0;
    int m_nSkill1ID=0;
    int m_nSkill2ID=0;
    int m_nSkill3ID=0;
    int m_nSkill4ID=0;
    int m_nIdentitySkillID=0;

    //캐릭터 상태
    public GameManager.CharState CS;

    //타겟
    public GameObject objTarget;
    //공격 위치
    Transform AttackPos;
    //애니메이션(메카님 애니메이션을 통한 제어)
    Animator animator;

    //버프 상태
    bool m_bProtectBuff = false;
    float m_fProtectBuffTimer = 0;


    //스킬 쿨 타임
    float m_fSkill1CoolTimer = 3;
    float m_fSkill2CoolTimer = 10;
    float m_fSkill3CoolTimer = 10;
    float m_fSkill4CoolTimer = 10;

    //Skill 사용 가능 여부
    bool m_bSkill1On = false;
    bool m_bSkill2On = false;
    bool m_bSkill3On = false;
    bool m_bSkill4On = false;

    bool m_bIdentitySkillOn = false;


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

    public bool getIdentitySkillOn()
    {
        return m_bIdentitySkillOn;
    }

    //get status
    public int getID()
    {
        return m_nCharID;
    }
    public int getATK()
    {
        return m_nATK;
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
        return m_fPlayerSpeed;
    }
    public int getIdentityPoint()
    {
        return m_nIdentityPoint;
    }
    public int getIdentityPointMax()
    {
        return m_nIdentityPointtMax;
    }



    // get ID
    public int getAttackID()
    {
        return m_nAttackID;
    }
    public int getSkill1ID()
    {
        return m_nSkill1ID;
    }
    public int getSkill2ID()
    {
        return m_nSkill2ID;
    }
    public int getSkill3ID()
    {
        return m_nSkill3ID;
    }
    public int getSkill4ID()
    {
        return m_nSkill4ID;
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


    //get bool Check
    public bool getCheck01()
    {
        return m_bCheck01;
    }
    public bool getCheck02()
    {
        return m_bCheck02;
    }


    //set
    public void setCS(GameManager.CharState _CS)
    {
        CS = _CS;
    }


    //set CharStatus
    public void CharStatusSetting(CharData _chardata)
    {
        //ID
        m_nCharID = _chardata.getID();
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

            //마나 회복 속도
            m_nPlayerMPRecoveryTimer = _chardata.getMP_Recovery();

            //playerIdentitySkill
            m_nIdentityPointtMax = _chardata.getIdentitySkillPoint();
            m_nIdentityPoint = m_nIdentityPointtMax;
            m_fIdentityPointRecovery = _chardata.getIdentitySkillPointRecovery();

            m_fIdentityPointRecoveryTime = _chardata.getIdentityPointRecoveryTime();
            m_fIdentityPointRecoveryTimer = m_fIdentityPointRecoveryTime;

            //SkillID
            m_nAttackID = _chardata.getAttackID();
            m_nSkill1ID = _chardata.getSkill1ID();
            m_nSkill2ID = _chardata.getSkill2ID();
            m_nSkill3ID = _chardata.getSkill3ID();
            m_nSkill4ID = _chardata.getSkill4ID();
            m_nIdentitySkillID = _chardata.getIdentitySkillID();
        }
        if (_chardata.getLayer() == 8)
        {
            //SkillID
            m_nSkill1ID = _chardata.getSkill1ID();
            m_nSkill2ID = _chardata.getSkill2ID();
            m_nSkill3ID = _chardata.getSkill3ID();
            m_nSkill4ID = _chardata.getSkill4ID();


        }
        if (_chardata.getLayer() == 9)
        {
            //MP
            m_nPlayerMPMax = _chardata.getMP();
            m_nPlayerMP = m_nPlayerMPMax;

            //마나 회복 속도
            m_nPlayerMPRecoveryTimer = _chardata.getMP_Recovery();

            //playerIdentitySkill
            m_nIdentityPointtMax = _chardata.getIdentitySkillPoint();
            m_nIdentityPoint = m_nIdentityPointtMax;
            m_fIdentityPointRecovery = _chardata.getIdentitySkillPointRecovery();

            m_fIdentityPointRecoveryTime = _chardata.getIdentityPointRecoveryTime();
            m_fIdentityPointRecoveryTimer = m_fIdentityPointRecoveryTime;

            //SkillID
            m_nAttackID = _chardata.getAttackID();
            m_nSkill1ID = _chardata.getSkill1ID();
            m_nSkill2ID = _chardata.getSkill2ID();
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

    public void setIdentitySkillOn(bool _IdentitySkillOn)
    {
        m_bIdentitySkillOn = _IdentitySkillOn;
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
            int totalDamage = (_Damege - m_nDEF);
            if (m_bProtectBuff)
            {
                totalDamage = totalDamage / 2;
            }

            if (this.gameObject.layer == 6)
            {
                Player_Ctrl pc = GetComponent<Player_Ctrl>();
                if (CS != GameManager.CharState.Death)
                {
                    if (CS == GameManager.CharState.IdentitySkill)
                    {
                        //iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
                        //if (totalDamage > pc.m_nPlayerShieldPoint)
                        //{
                        //    pc.m_nPlayerShieldPoint -= totalDamage - pc.m_nPlayerShieldPoint;
                        //    pc.m_nPlayerShieldPoint = 0;
                        //    pc.m_fShieldChargeTimer = 5;
                        //    CS = GameManager.CharState.Hit;
                        //}
                        //else
                        //{
                        //    pc.m_nPlayerShieldPoint -= totalDamage;
                        //}

                    }
                    else if (CS == GameManager.CharState.Skill2)
                    {
                        m_nPlayerHP -= totalDamage / 2;
                    }
                    else
                    {
                        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
                        m_nPlayerHP -= totalDamage;
                        gameObject.GetComponent<Char_Dynamics>().SetCharStatus(GameManager.CharState.Hit);
                    }

                }
            }
            if (gameObject.layer == 9)
            {
                m_nPlayerHP -= totalDamage;
                gameObject.GetComponent<Char_Dynamics>().SetCharStatus(GameManager.CharState.Hit);
                //CS = GameManager.CharState.Hit;
                //if (this.gameObject.name =="Thief")
                //{
                //    Thief_Dynamic td =GetComponent<Thief_Dynamic>();
                //    td.PS = GameManager.PartnerState.Hit;
                //}
                //else
                //{
                //    Parter_Dynamic pd = GetComponent<Parter_Dynamic>();

                //}

            }
            //Debug.Log(this.gameObject.name + " HP : " + m_nPlayerHP + "/" + m_nPlayerHPMax + "\n" + "GetDamage :" + totalDamage);
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

    public void onProtectBuff()
    {
        Debug.Log("onBuff");
        m_bProtectBuff = true;
        m_fProtectBuffTimer = 10;
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
            if (m_nPlayerMPRecoveryTimer <= 0)
            {
                m_nPlayerMP += 10;
                m_nPlayerMPRecoveryTimer = 3;
            }
            else
            {
                m_nPlayerMPRecoveryTimer -= Time.deltaTime;
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


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        objTarget = null;
        AttackPos = this.transform.GetChild(0);
        //m_nPlayerHP = m_nPlayerHPMax;
        //m_nIdentityPoint = m_nIdentityPointtMax;
        CS=GameManager.CharState.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        SkillCoolTimer();
        Recovery();


        if (m_bProtectBuff)
        {
            if (m_fProtectBuffTimer <= 0)
            {
                Debug.Log("offBuff");
                m_bProtectBuff = false;
            }
            m_fProtectBuffTimer -= Time.deltaTime;  
        }

        

    }
}
