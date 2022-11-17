using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkillData 
{
    // ID
    int SkillID;
    // 스킬명
    string SkillName;
    // 스킬 계수
    float SkillCeofficientPer1;
    float SkillCeofficientPer2;
    // 스킬 쿨타임
    float SkillCoolTime;
    // 스킬 사용 마나
    int SkillUsingMana;
    // 스킬 이펙트 리소스
    string SkillEffectResource;
    // 타깃 구분
    int TargetSelect;
    // 범위 지정
    float SkillRange1;
    float SkillRange2;
    //스킬 속도
    float SkillSpeed;
    //스킬 지속 시간
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
        //캐릭터명
        SkillName = _SkillName;
        // 스킬 계수
        SkillCeofficientPer1= _SkillCeofficientPer1;
        SkillCeofficientPer2= _SkillCeofficientPer2;
        // 스킬 쿨타임
        SkillCoolTime = _SkillCoolTime;
        // 스킬 사용 마나
        SkillUsingMana= _SkillUsingMana;
        // 스킬 이펙트 유무 확인
        // 스킬 이펙트 리소스
        SkillEffectResource = _SkillEffectResource;
        // 타깃 구분
        TargetSelect = _TargetSelect;
        // 범위 지정
        SkillRange1= _SkillRange1;
        SkillRange2= _SkillRange2;
        //스킬 속도
        SkillSpeed = _SkillSpeed;
        //스킬 지속 시간
        SkillUsingTime=_SkillUsingTime;
    }


    // ID
    public int getSkillID()
    {
        return SkillID;
    }
    // 스킬명
    public string getSkillName()
    {
        return SkillName;
    }
    // 스킬 계수
    public float getSkillCeofficientPer1()
    {
        return SkillCeofficientPer1;
    }
    public float getSkillCeofficientPer2()
    {
        return SkillCeofficientPer2;
    }
    // 스킬 쿨타임
    public float getSkillCoolTime()
    {
        return SkillCoolTime;
    }
    // 스킬 사용 마나
    public int getSkillUsingMana()
    {
        return SkillUsingMana;
    }
    // 스킬 이펙트 리소스
    public string getSkillEffectResource()
    {
        return SkillEffectResource;
    }
    // 타깃 구분
    public int getTargetSelect()
    {
        return TargetSelect;
    }
    // 범위 지정
    public float getSkillRange1()
    {
        return SkillRange1;
    }
    public float getSkillRange2()
    {
        return SkillRange2;
    }

    //스킬 속도
    public float getSkillSpeed()
    {
        return SkillSpeed;
    }
    //스킬 지속 시간
    public float getSkillUsingTime()
    {
        return SkillUsingTime;
    }
}
