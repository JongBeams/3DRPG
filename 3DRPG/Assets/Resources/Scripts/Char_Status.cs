using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Status : MonoBehaviour
{
    public GameManager.CharState CS;


    public float m_fPlayerSpeed = 5;

    public int Damage = 3;

    public int m_nPlayerHP = 10;
    public int m_nPlayerHPMax = 10;

    public int m_nPlayerMP = 100;
    public int m_nPlayerMPMax = 100;
    public float m_nPlayerMPRecoveryTimer = 3;


    public int m_nPlayerAumor = 0;

    bool m_bProtectBuff = false;
    float m_fProtectBuffTimer = 0;

    public void GetDamage(int _Damege)
    {
        if (CS !=GameManager.CharState.Death)
        {
            int totalDamage = (_Damege - m_nPlayerAumor);
            if (m_bProtectBuff)
            {
                totalDamage = totalDamage / 2;
            }

            if (this.gameObject.layer == 6)
            {
                Player_Ctrl pc = GetComponent<Player_Ctrl>();
                if (CS != GameManager.CharState.Death)
                {
                    if (CS == GameManager.CharState.Skill0)
                    {
                        //iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
                        if (totalDamage > pc.m_nPlayerShieldPoint)
                        {
                            pc.m_nPlayerShieldPoint -= totalDamage - pc.m_nPlayerShieldPoint;
                            pc.m_nPlayerShieldPoint = 0;
                            pc.m_fShieldChargeTimer = 5;
                            CS = GameManager.CharState.Hit;
                        }
                        else
                        {
                            pc.m_nPlayerShieldPoint -= totalDamage;
                        }

                    }
                    else if (CS == GameManager.CharState.Skill2)
                    {
                        m_nPlayerHP -= totalDamage / 2;
                    }
                    else
                    {
                        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.2, "y", 0.2, "time", 0.3f));
                        m_nPlayerHP -= totalDamage;
                        CS = GameManager.CharState.Hit;
                    }

                }
            }
            if (gameObject.layer == 9)
            {
                m_nPlayerHP -= totalDamage;
                CS = GameManager.CharState.Hit;
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
            Debug.Log(this.gameObject.name + " HP : " + m_nPlayerHP + "/" + m_nPlayerHPMax + "\n" + "GetDamage :" + totalDamage);
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

    public void setPlayerStatus(int _MaxHP,int _MaxMP,int _CharSpeed,int _Damage,int _Aumor)
    {
        m_nPlayerHPMax = _MaxHP;
        m_nPlayerHP = m_nPlayerHPMax;
        m_nPlayerMPMax = _MaxMP;
        m_nPlayerMP = m_nPlayerMPMax;
        m_fPlayerSpeed = _CharSpeed;
        Damage = _Damage;
        m_nPlayerAumor= _Aumor;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_nPlayerHP = m_nPlayerHPMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_nPlayerHP> m_nPlayerHPMax)
        {
            m_nPlayerHP = m_nPlayerHPMax;
        }
        if (m_bProtectBuff)
        {
            if (m_fProtectBuffTimer <= 0)
            {
                Debug.Log("offBuff");
                m_bProtectBuff = false;
            }
            m_fProtectBuffTimer -= Time.deltaTime;  
        }

        if (m_nPlayerMP < m_nPlayerMPMax)
        {
            if (m_nPlayerMPRecoveryTimer <= 0)
            {
                m_nPlayerMP+=10;
                m_nPlayerMPRecoveryTimer = 3;
            }
            else
            {
                m_nPlayerMPRecoveryTimer -= Time.deltaTime;
            }
        }

    }
}
