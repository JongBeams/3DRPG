using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedDragon_Char : Char_Base
{
    // Start is called before the first frame update
    void Start()
    {
        SetComponents();
        SetCheck(2);
        m_nPlayerHP = CharStatus.HP;
        m_bSuperArmor = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharStatus();
        Recovery();
        SkillCooTimer();
    }


    #region 스테이터스

    public override void Recovery()
    {

    }

    protected override void SetSkillCoolTime()
    {
        m_bSkillOn[m_nActionIdx-1] = false;
        m_fSkillCoolTimer[m_nActionIdx-1] = DBManager.SkillData[CharStatus.SID[m_nActionIdx-1]].SCT;
    }


    #endregion


    #region 상태

    public Action skillAction = null;

    public async Task Skill(float _delayTime)
    {
        await Task.Delay(TimeSpan.FromSeconds(_delayTime));
        skillAction?.Invoke();
    }

    void SetAction(int _idx)
    {
        switch ((ActionState)_idx)
        {
            case ActionState.Skill1:
                skillAction = MeleeTargetAttack;
                animator.SetBool("Skill1", true);
                strActionAniName = "Skill1";
                break;
            case ActionState.Skill2:
                skillAction = RangeAngleAttack;
                animator.SetBool("Skill2", true);
                strActionAniName = "Skill2";
                break;
            case ActionState.Skill3:
                skillAction = FireBall;
                animator.SetBool("Skill3", true);
                strActionAniName = "Skill3";
                break;
            case ActionState.Skill4:
                skillAction = FireBreath;
                animator.SetBool("Skill4", true);
                strActionAniName = "Skill4";
                break;

        }

    }

    void AniBoolOffAll()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Skill1", false);
        animator.SetBool("Skill2", false);
        animator.SetBool("Skill3", false);
        animator.SetBool("Skill4", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Identity", false);
    }



    public override void SetCharStatus(CharState _CS) // 한번 실행
    {
        switch (_CS)
        {
            case CharState.Idle:
                objTarget = null;
                agent.velocity = Vector3.zero;
                agent.SetDestination(this.transform.position);
                AniBoolOffAll();
                break;
            case CharState.Move:
                animator.SetBool("Move", true);
                agent.SetDestination(PlayerLookingPoint());
                break;
            case CharState.Action:
                agent.velocity = Vector3.zero;
                agent.SetDestination(this.transform.position);
                SetAction(m_nActionIdx);
                transform.LookAt(PlayerLookingPoint());
                skillAction?.Invoke();
                SetSkillCoolTime();
                break;
            case CharState.Hit:
                AniBoolOffAll();
                animator.SetBool("Hit", true);
                break;
            case CharState.Death:
                if(CS!= CharState.Death)
                    ItemDrop();
                animator.SetBool("Death", true);
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().isTrigger = true;
                //Destroy(this.gameObject, 5f);
                break;
            case CharState.Stay:
                AniBoolOffAll();
                break;

        }


        CS = _CS;
    }


    public override void UpdateCharStatus()// 지속 실행
    {
        switch (CS)
        {
            case CharState.Idle:
                if (CheckEndAni("Idle"))
                {
                    SetAlgorithm();
                }
                break;
            case CharState.Move:
                MoveAlgorithm();
                break;
            case CharState.Action:
                if (m_bSkillUsing[m_nActionIdx-1])
                {
                    skillAction?.Invoke();
                }
                else
                {
                    if (CheckEndAni(strActionAniName))
                    {
                        SetCharStatus(CharState.Idle);
                    }
                }
                break;
            case CharState.Hit:
                if (CheckEndAni("Hit"))
                {
                    SetCharStatus(CharState.Idle);
                }
                break;
            case CharState.Death:

                break;
            case CharState.Stay:
                break;

        }
    }


    #endregion


    #region 알고리즘

    void SetAlgorithm()
    {
        List<GameObject> targetobj = new List<GameObject>();
        if (CharStatus.TYP == LayerMask.NameToLayer("Enemy"))
        {
            targetobj.Add(GameManager.Instance.objPlayer);
            targetobj.Add(InGameSceneManager.Instance.PInfo[0].objPartner);
            targetobj.Add(InGameSceneManager.Instance.PInfo[1].objPartner);
            
        }
        if (CharStatus.TYP == LayerMask.NameToLayer("Player")|| CharStatus.TYP == LayerMask.NameToLayer("Partner"))
        {
            targetobj.Add(InGameSceneManager.Instance.objEnemy);
        }
        for (int i = 0; i < targetobj.Count; i++)
        {
            if(targetobj[i].GetComponent<Char_Base>().CS== CharState.Death)
            {
                targetobj.RemoveAt(i);
            }
        }

        if (targetobj.Count == 0)
        {
            SetCharStatus(CharState.Stay);
            return;
        }

        if (!m_bTaunt)
        {
            int TargetRan = Random.Range(0, targetobj.Count);
            objTarget = targetobj[TargetRan].gameObject;
        }

        vecMovePoint = objTarget.transform.position;

        //Debug.Log(i+","+count + "," + hitcol.Length);
        if (GameManager.Instance.m_nScreenIdx != 2)
        {
            SetCharStatus(CharState.Stay);
            return;
        }
        if (GameManager.Instance.m_nScreenIdx == 2 && InGameSceneManager.Instance.m_bGameEnd)
        {
            SetCharStatus(CharState.Stay);
            return;
        }

        int random = Random.Range(0, 100);
        if (Vector3.Distance(objTarget.transform.position, transform.position) < 8f)
        {
            if (random < 75)
            {

                if (Random.Range(0, 2) == 0)
                {
                    m_nActionIdx = 1;
                }
                else
                {
                    m_nActionIdx = 2;
                }

            }
            else
            {
                //AttackDelayTimer = AttackDelayTime * 2;
                if (Random.Range(0, 2) == 0)
                {
                    m_nActionIdx = 3;
                }
                else
                {
                    m_nActionIdx = 4;
                }

            }
            SetCharStatus(CharState.Action);
        }
        else if (Vector3.Distance(objTarget.transform.position, transform.position) >= 8f && Vector3.Distance(objTarget.transform.position, transform.position) < 20f)
        {
            if (random < 75)
            {
                if (Random.Range(0, 2) == 0)
                {
                    m_nActionIdx = 3;
                }
                else
                {
                    m_nActionIdx = 4;
                }
                SetCharStatus(CharState.Action);
            }
            else
            {
                agent.SetDestination(vecMovePoint);
                SetCharStatus(CharState.Move);
            }
        }
        else
        {
            agent.SetDestination(vecMovePoint);
            SetCharStatus(CharState.Move);

        }


    }

    void MoveAlgorithm()
    {

        // 타겟과의 거리
        float dis = Vector3.Distance(transform.position, vecMovePoint);

        float Range = 7.5f;

        if (dis <= Range)
        {
            agent.velocity = Vector3.zero;
            SetCharStatus(CharState.Idle);
            return;
        }


    }

    #endregion


    #region 스킬
    bool m_bAttackCheck;

    void AttackCheck()
    {
        m_bAttackCheck = true;
    }



    void MeleeTargetAttack()//vector 크기만다르다
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[0]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);


        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapBox(AttackPos.position, new Vector3(3, 3, 3),
            Quaternion.Euler(new Vector3(0, GetAngle(gameObject.transform.position, PlayerLookingPoint()), 0)), m_nMask);

        //Debug.Log(hitcol[0].gameObject);
        if (hitcol.Length != 0)
        {

            hitcol[0].GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));

        }

    }





    void RangeAngleAttack()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[1]];


        Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length / 2);

        int m_nMask = m_nTargetLayer[0];
        Collider[] hitcol = Physics.OverlapSphere(transform.position, SkillDB.getSkillRange1(), m_nMask);//충돌감지 저장
        int count = 0;
        //int i = 0;
        while (count < hitcol.Length)
        {

            Vector3 targetDir = hitcol[count].transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
            if (angle <= SkillDB.getSkillRange2())
            {

                hitcol[count].gameObject.GetComponent<Char_Base>().delGetDamage((int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()));
            }

            count++;
        }


    }

    void FireBall()
    {
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[2]];

        if (objTarget != null || objTarget.activeSelf == false)
        {
            GameObject objFireBall = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity);
            objFireBall.GetComponent<Bullet>().Setting(objTarget, this, CharStatus.SID[2]);
        }


    }

    void FireBreath()
    {

        //스킬 정보
        SkillData SkillDB = DBManager.SkillData[CharStatus.SID[3]];


        if (objTarget != null || objTarget.activeSelf == false)
        {

            GameObject FireBreathEffect = Instantiate(Resources.Load<GameObject>(SkillDB.getSkillEffectResource()), AttackPos.position, Quaternion.identity, AttackPos);
            FireBreathEffect.transform.parent = AttackPos;
            FireBreathEffect.transform.localPosition = Vector3.zero;
            FireBreathEffect.transform.localRotation = Quaternion.identity;
            FireBreathEffect.transform.parent = null;
            FireBreathEffect.GetComponent<ParticleSystem>().Play();
            FireBreathEffect.GetComponent<FireBreath>().Setting(this, CharStatus.SID[3]);

        }

    }



    #endregion


    #region 연산

    float GetAngle(Vector3 start, Vector3 end)
    {
        Vector3 v2 = end - start;
        return Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;
    }

    #endregion


    #region 아이템드랍

    void ItemDrop()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            ItemDropData IDD = DBManager.GetItemDropDataByIdx(CharStatus.ID);
            GameManager.Instance.m_nGold += 100;
            //Debug.Log(IDD.IDP.Count);
            for (int i = 0; i < IDD.IDP.Count; i++)
            {
                int ran = Random.Range(0, 100);
                //Debug.Log(i+"번째 아이템 드랍 "+(ran+1)+"/100");
                if (ran < IDD.IDP[i])
                {
                    GameObject Item = Instantiate(Resources.Load<GameObject>("Prefabs/Item/DropItem"), this.transform.position, Quaternion.identity);
                    //Debug.Log("드랍성공");
                    //Debug.Log("ItemID : "+ IDD.IDT[i]);
                    //Debug.Log("ItemMesh : "+ DBManager.GetItemStatusByIdx(IDD.IDT[i]).Mesh);
                    //Debug.Log("ItemMaterial : "+ DBManager.GetItemStatusByIdx(IDD.IDT[i]).Material);
                    Item.GetComponent<DropItemInfo>().SetItem(IDD.IDT[i], DBManager.GetItemStatusByIdx(IDD.IDT[i]).Mesh, DBManager.GetItemStatusByIdx(IDD.IDT[i]).Material);
                }
                else
                {
                    //Debug.Log("드랍 실패!");
                }
            }
        }
    }

    #endregion

    }
