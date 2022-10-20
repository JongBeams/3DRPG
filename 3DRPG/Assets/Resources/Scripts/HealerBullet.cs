using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBullet : MonoBehaviour
{
    float m_fSpeed = 6;

    public GameObject Target;

    public ParticleSystem Explosion;

    public void SelectTarget(GameObject _Target)
    {
        Target = _Target;

    }

    // Start is called before the first frame update
    void Start()
    {
        Explosion = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Target.transform.position);
        Vector3 vecTraget = new Vector3(Target.transform.position.x, 1, Target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, vecTraget, m_fSpeed * Time.deltaTime);
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
        if (other.gameObject.layer == 8)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Explosion.Play();
            other.gameObject.GetComponent<Enemy_Ctrl>().GetDamage(2);
            Destroy(this.gameObject, 1f);

        }
    }
}
