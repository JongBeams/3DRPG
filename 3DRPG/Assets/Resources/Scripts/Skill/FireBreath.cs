using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireBreath : MonoBehaviour
{
    public Char_Base Attacker;

    public int SkillID;
    public float m_fDis = 0f;

    public List<GameObject> objTarget;

    public List<GameObject> Hitobj = new List<GameObject>();

    public bool FirstSetting = true;


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
        
    //    Gizmos.DrawRay(this.transform.position + (transform.right), transform.forward * m_fDis);
    //    Gizmos.DrawRay(this.transform.position + (-transform.right), transform.forward * m_fDis);
    //    Gizmos.DrawRay(this.transform.position + (transform.forward * m_fDis) + (-transform.right* DBManager.SkillData[SkillID].SR2 / 2), transform.right * DBManager.SkillData[SkillID].SR2);
    //}

    public void Setting( Char_Base _attacker,int _SkillID)
    {
        if (FirstSetting)
        {
            Attacker = _attacker;
            SkillID = _SkillID;
            FirstSetting = false;
        }

        if (Attacker.CharStatus.TYP == LayerMask.NameToLayer("Enemy"))
        {
            objTarget.Add(GameManager.Instance.objPlayer);
            objTarget.Add(InGameSceneManager.Instance.PInfo[0].objPartner);
            objTarget.Add(InGameSceneManager.Instance.PInfo[1].objPartner);
            return;
        }
        if (Attacker.CharStatus.TYP == LayerMask.NameToLayer("Player")|| Attacker.CharStatus.TYP == LayerMask.NameToLayer("Partner"))
        {
            objTarget.Add(InGameSceneManager.Instance.objEnemy);
            return;
        }

    }

    // Start is called before the first frame update
    void Start()
    {



    }
    bool InsideCheck(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2,GameObject d)
    {
        float x_min = Mathf.Min(a1.x, a2.x, b1.x, b2.x);
        float x_max = Mathf.Max(a1.x, a2.x, b1.x, b2.x);
        float y_min = Mathf.Min(a1.y, a2.y, b1.y, b2.y);
        float y_max = Mathf.Max(a1.y, a2.y, b1.y, b2.y);
        float z_min = Mathf.Min(a1.z, a2.z, b1.z, b2.z);
        float z_max = Mathf.Max(a1.z, a2.z, b1.z, b2.z);

        return (d.transform.position.x > x_min && d.transform.position.x < x_max) &&
                        (d.transform.position.z > z_min && d.transform.position.z < z_max); ;
    }




    void CheckEnemy()
    {
        SkillData sd = DBManager.SkillData[SkillID];
        Vector3 a1 = this.transform.position + (transform.right * sd.SR2 / 2);
        Vector3 a2 = this.transform.position - (transform.right * sd.SR2 / 2);
        Vector3 b1 = this.transform.position + (transform.right * sd.SR2 / 2);
        Vector3 b2 = this.transform.position - (transform.right * sd.SR2 / 2);
        if (InsideCheck())
        {

        }
    }

    void SetRaycast()
    {
        SkillData sd = DBManager.SkillData[SkillID];

        if (m_fDis < DBManager.SkillData[SkillID].SR1)
        {
            RaycastHit Hit;


            int layerMask = Attacker.m_nTargetLayer[0];


            if (Physics.Raycast(this.transform.position + (transform.forward * m_fDis) + (-transform.right * sd.SR2 / 2), transform.right, out Hit, sd.SR2, layerMask, QueryTriggerInteraction.Collide) ||
                Physics.Raycast(this.transform.position + (-transform.right * sd.SR2 / 2), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Collide) ||
                Physics.Raycast(this.transform.position + (transform.right * sd.SR2 / 2), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Collide))
            {

                Debug.Log("Hit : " + Hit.collider.gameObject);
                Hitobj.Add(Hit.collider.gameObject);

                if (Hitobj.Count != Hitobj.Distinct().Count())
                {
                    Hitobj = Hitobj.Distinct().ToList();
                }
                else
                {
                    Debug.Log(Hit.collider.gameObject);
                    Hit.collider.gameObject.GetComponent<Char_Base>().delGetDamage((int)(Attacker.CharStatus.ATK * sd.getSkillCeofficientPer1()));
                    Debug.Log("Hit");
                }


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
        CheckEnemy();
    }
}
