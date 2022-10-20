using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float m_fSpeed = 2;

    public GameObject Target;

    public ParticleSystem Burning;
    public ParticleSystem Explosion;

    bool hit = false;

    bool end = false;

    public void SelectTarget(GameObject _Target)
    {
        Target = _Target;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Burning = this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        //Debug.Log(this.transform.GetChild(1).gameObject);
        Explosion = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            if (Target == null|| Target.activeSelf==false&& !end)
            {
                Burning.Stop();
                Explosion.Play();
                Destroy(this.gameObject, 1f);
                end = true;
            }
            else
            {
                transform.LookAt(Target.transform.position);
                Vector3 vecTraget = new Vector3(Target.transform.position.x, 1, Target.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, vecTraget, m_fSpeed * Time.deltaTime);
            }

        }
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
    //    {
    //        Burning.Stop();
    //        Explosion.Play();
    //        collision.gameObject.GetComponent<Char_Status>().GetDamage(8);
    //        Destroy(this.gameObject, 1f);

    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==6|| other.gameObject.layer == 9)
        {
            hit = true;
            Burning.Stop();
            Explosion.Play();
            other.gameObject.GetComponent<Char_Status>().GetDamage(15);
            Destroy(this.gameObject, 1f);

        }
    }
}
