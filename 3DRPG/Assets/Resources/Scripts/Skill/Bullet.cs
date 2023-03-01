using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int SkillID;

    public GameObject Target;

    public Char_Base Attacker;

    public ParticleSystem Burning;
    public ParticleSystem Explosion;

    bool hit = false;

    bool end = false;

    bool FirstSetting = true;

    public void Setting(GameObject _Target, Char_Base _Attacker, int _SkillID)
    {
        if (FirstSetting)
        {
            Target = _Target;
            Attacker = _Attacker;
            SkillID = _SkillID;
            FirstSetting = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Burning = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        Explosion = this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        SkillData sd = DBManager.SkillData[SkillID];
        Destroy(this.gameObject, sd.SLT);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            if (!FirstSetting)
            {
                if (Target == null || Target.activeSelf == false && !end)
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
                    SkillData sd = DBManager.SkillData[SkillID];
                    transform.position = Vector3.MoveTowards(transform.position, vecTraget, sd.SSPD * Time.deltaTime);
                }
            }


        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if ((1<<other.gameObject.layer & Attacker.m_nTargetLayer[0]) != 0)
        {

            //Debug.Log(other.gameObject.layer+" "+ Target.layer+" "+Attacker.m_nTargetLayer);

            hit = true;
            Burning.Stop();
            Explosion.Play();
            SkillData sd = DBManager.SkillData[SkillID];
            other.gameObject.GetComponent<Char_Base>().delGetDamage((int)(Attacker.CharStatus.ATK*sd.SDP1));
            Destroy(this.gameObject, 1f);

        }

    }
}
