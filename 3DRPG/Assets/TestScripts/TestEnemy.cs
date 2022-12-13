using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public GameObject objTarget;

    //int meleePer=50;

    void TargetSetting()
    {
        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 15f, m_nMask);
        int i = 0;

        while (i < hitcol.Length)
        {
            Debug.Log(hitcol[i].gameObject+"+"+Vector3.Distance(hitcol[i].transform.position, this.transform.position));
            i++;
        }
    }


    void TargetSetting2()
    {
        //근거리의 적이 많으면 근거리 패턴을 쓸 확률 이올라간다.
        //원거리의 적이 많으면 원거리의 적을 공격할 확률이 올라간다.
        //
        //


        // 범위 안의 적을 탐색한다.
        // 탐색된 적중 근접 한 적의 수를 특정한다.

        int m_nMask = 0;
        m_nMask = 1 << (LayerMask.NameToLayer("Player"));
        Collider[] hitcol = Physics.OverlapSphere(transform.position, 15f, m_nMask);
        int count = 0;
        int i = 0;

        objTarget = null;


        while (i < hitcol.Length)
        {
            if (Vector3.Distance(hitcol[count].transform.position, this.transform.position) < 8f)
            {
                if (objTarget == null)
                {
                    objTarget = hitcol[count].gameObject;
                    count++;
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        objTarget = hitcol[count].gameObject;

                    }
                    count++;
                }

            }
            i++;
        }

        if (count < 2)
        {
           
        }


        //GameObject[] objMeleeTarget = new GameObject[2];
        //while (count<hitcol.Length)
        //{
        //    if (Vector3.Distance(hitcol[count].transform.position, this.transform.position) < 8f)
        //    {
        //        if (objMeleeTarget[0] == null)
        //        {
        //            objMeleeTarget[0] = hitcol[count].gameObject;
        //        }
        //        else
        //        {
        //            objMeleeTarget[1] = hitcol[count].gameObject;
        //            break;
        //        }
        //    }
        //    count++;
        //}


        //Debug.Log(hitcol[0].gameObject);
        //if (hitcol.Length != 0)
        //{
        //    Debug.Log(hitcol[0].gameObject);
        //    hitcol[0].GetComponent<Player_Ctrl>().GetDamage(2);
        //    Debug.Log(hitcol[0].GetComponent<Player_Ctrl>().m_nPlayerHP);
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        TargetSetting();

        TargetSetting2();

        DBManager.Instance.CallDBManager();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       

        Gizmos.DrawWireSphere(this.transform.position,15f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 8f);
    }



    // Update is called once per frame
    void Update()

    {
        
    }
}
