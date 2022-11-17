using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBullet : MonoBehaviour
{
    float m_fSpeed = 6;

    GameObject Target;

    public ParticleSystem Explosion;

    int Damage=0;

    bool FirstSetting = true;

    bool EnemyCheck=false;

    float LifeTime;


    public void Setting(GameObject _Target,int _Damage,float _Speed,bool _EnemyCheck,float _LifeTime)
    {
        if (FirstSetting)
        {
            Target = _Target;
            Damage = _Damage;
            m_fSpeed = _Speed;
            FirstSetting = false;
            EnemyCheck = _EnemyCheck;
            LifeTime = _LifeTime;
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        Explosion = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        Destroy(this.gameObject, LifeTime);
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
        if (EnemyCheck)
        {
            if (other.gameObject.layer == 8)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Explosion.Play();
                other.gameObject.GetComponent<Enemy_Ctrl>().GetDamage(Damage);
                Destroy(this.gameObject, 1f);

            }
        }
        else
        {
            if (other.gameObject.layer == 6 || other.gameObject.layer == 9)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Explosion.Play();
                other.gameObject.GetComponent<Enemy_Ctrl>().GetDamage(Damage);
                Destroy(this.gameObject, 1f);

            }
        }
        
    }
}
