using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireBreath : MonoBehaviour
{
    Char_Base Attacker;

    int SkillID;
    float m_fDis = 0f;

    public List<GameObject> Hitobj = new List<GameObject>();

    bool FirstSetting = true;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(this.transform.position + (transform.right), transform.forward * m_fDis);
        Gizmos.DrawRay(this.transform.position + (-transform.right), transform.forward * m_fDis);
        Gizmos.DrawRay(this.transform.position + (transform.forward * m_fDis) + (-transform.right* DBManager.SkillData[SkillID].SR2 / 2), transform.right * DBManager.SkillData[SkillID].SR2);
    }

    public void Setting( Char_Base _attacker,int _SkillID)
    {
        if (FirstSetting)
        {
            Attacker = _attacker;
            SkillID = _SkillID;
            FirstSetting = false;
        }


    }

    // Start is called before the first frame update
    void Start()
    {



    }


    void SetRaycast()
    {
        SkillData sd = DBManager.SkillData[SkillID];

        if (m_fDis < DBManager.SkillData[SkillID].SR1)
        {
            RaycastHit Hit;

            int layerMask = Attacker.m_nTargetLayer[0];


            if (Physics.Raycast(this.transform.position + (transform.forward * m_fDis) + (-transform.right * sd.SR2 / 2), transform.right * sd.SR2, out Hit, 2, layerMask, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(this.transform.position + (-transform.right * sd.SR2), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(this.transform.position + (transform.right * sd.SR2), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Ignore))
            {
                Hitobj.Add(Hit.collider.gameObject);
                if (Hitobj.Count != Hitobj.Distinct().Count())
                {
                    Hitobj = Hitobj.Distinct().ToList();
                }
                else
                {
                    Hit.collider.gameObject.GetComponent<Char_Base>().delGetDamage((int)(Attacker.CharStatus.ATK * sd.getSkillCeofficientPer1()));
                    Debug.Log("Hit");
                }
                //(int)(CharStatus.ATK * SkillDB.getSkillCeofficientPer1()), SkillDB.getSkillSpeed(),
                //SkillDB.getSkillRange1(), SetTargetLayerMask(SkillDB.getTargetSelect()), SkillDB.getSkillRange2()
            }

            m_fDis += sd.SSPD * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject, 1);
        }

        


    }

    // Update is called once per frame
    void Update()
    {
        SetRaycast();
    }
}
