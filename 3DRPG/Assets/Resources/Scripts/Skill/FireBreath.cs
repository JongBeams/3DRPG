using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireBreath : MonoBehaviour
{
    int m_nDamage = 0;
    float m_fDis = 0f;
    float m_fMaxDis = 5;
    float m_fSpeed = 3;
    int m_nLayerMask = 0;
    float m_fwidth = 0;

    public List<GameObject> Hitobj = new List<GameObject>();

    bool FirstSetting = true;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(this.transform.position + (transform.right), transform.forward * m_fDis);
        Gizmos.DrawRay(this.transform.position + (-transform.right), transform.forward * m_fDis);

        Gizmos.DrawRay(this.transform.position + (transform.forward * m_fDis) + (-transform.right* m_fwidth / 2), transform.right * m_fwidth);
    }

    public void Setting( int _Damage, float _Speed, float _MaxDis, int _LayerMask,float _width)
    {
        if (FirstSetting)
        {
            m_nDamage = _Damage;
            m_fSpeed = _Speed;
            FirstSetting = false;
            m_fMaxDis= _MaxDis;
            m_nLayerMask= _LayerMask;
            m_fwidth = _width;
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }


    void SetRaycast()
    {
        if (m_fDis < m_fMaxDis)
        {
            RaycastHit Hit;

            int layerMask = m_nLayerMask;


            if (Physics.Raycast(this.transform.position + (transform.forward * m_fDis) + (-transform.right * m_fwidth / 2), transform.right * m_fwidth, out Hit, 2, layerMask, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(this.transform.position + (-transform.right * m_fwidth), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(this.transform.position + (transform.right * m_fwidth), transform.forward, out Hit, m_fDis, layerMask, QueryTriggerInteraction.Ignore))
            {
                Hitobj.Add(Hit.collider.gameObject);
                if (Hitobj.Count != Hitobj.Distinct().Count())
                {
                    Hitobj = Hitobj.Distinct().ToList();
                }
                else
                {
                    Hit.collider.gameObject.GetComponent<Char_Status>().delGetDamae(m_nDamage);
                    Debug.Log("Hit");
                }

            }

            m_fDis += m_fSpeed * Time.deltaTime;
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
