using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkillData 
{
    // ID
    int SkillID;
    // ��ų��
    string SkillName;
    // ��ų ���
    float SkillCeofficientPer1;
    float SkillCeofficientPer2;
    // ��ų ��Ÿ��
    float SkillCoolTime;
    // ��ų ��� ����
    int SkillUsingMana;
    // ��ų ����Ʈ ���ҽ�
    string SkillEffectResource;
    // Ÿ�� ����
    int TargetSelect;
    // ���� ����
    float SkillRange1;
    float SkillRange2;
    //��ų �ӵ�
    float SkillSpeed;
    //��ų ���� �ð�
    float SkillUsingTime;

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
        SkillID= _SkillID;
        //ĳ���͸�
        SkillName = _SkillName;
        // ��ų ���
        SkillCeofficientPer1= _SkillCeofficientPer1;
        SkillCeofficientPer2= _SkillCeofficientPer2;
        // ��ų ��Ÿ��
        SkillCoolTime = _SkillCoolTime;
        // ��ų ��� ����
        SkillUsingMana= _SkillUsingMana;
        // ��ų ����Ʈ ���� Ȯ��
        // ��ų ����Ʈ ���ҽ�
        SkillEffectResource = _SkillEffectResource;
        // Ÿ�� ����
        TargetSelect = _TargetSelect;
        // ���� ����
        SkillRange1= _SkillRange1;
        SkillRange2= _SkillRange2;
        //��ų �ӵ�
        SkillSpeed = _SkillSpeed;
        //��ų ���� �ð�
        SkillUsingTime=_SkillUsingTime;
    }


    // ID
    public int getSkillID()
    {
        return SkillID;
    }
    // ��ų��
    public string getSkillName()
    {
        return SkillName;
    }
    // ��ų ���
    public float getSkillCeofficientPer1()
    {
        return SkillCeofficientPer1;
    }
    public float getSkillCeofficientPer2()
    {
        return SkillCeofficientPer2;
    }
    // ��ų ��Ÿ��
    public float getSkillCoolTime()
    {
        return SkillCoolTime;
    }
    // ��ų ��� ����
    public int getSkillUsingMana()
    {
        return SkillUsingMana;
    }
    // ��ų ����Ʈ ���ҽ�
    public string getSkillEffectResource()
    {
        return SkillEffectResource;
    }
    // Ÿ�� ����
    public int getTargetSelect()
    {
        return TargetSelect;
    }
    // ���� ����
    public float getSkillRange1()
    {
        return SkillRange1;
    }
    public float getSkillRange2()
    {
        return SkillRange2;
    }

    //��ų �ӵ�
    public float getSkillSpeed()
    {
        return SkillSpeed;
    }
    //��ų ���� �ð�
    public float getSkillUsingTime()
    {
        return SkillUsingTime;
    }
}
