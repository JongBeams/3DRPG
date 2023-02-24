using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public GameObject Target;
    public float Distance = 5f;
    public float Height = 8f;
    public float Speed = 2f;

    Vector3 Pos;



    void Start()
    {

    }

    void Update()
    {
        if (Target)
        {
            Pos = new Vector3(Target.transform.position.x, Height, Target.transform.position.z - Distance);
            this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, Pos, Speed * Time.deltaTime);
        }
        

        //if (GameManager.instance.objPlayer.GetComponent<Char_Base>().CS != GameManager.CharState.Death||
        //    GameManager.instance.objHealer.GetComponent<Char_Base>().CS != GameManager.CharState.Death ||
        //    GameManager.instance.objThief.GetComponent<Char_Base>().CS != GameManager.CharState.Death)
        //{
        //    Pos = new Vector3(Target.transform.position.x, Height, Target.transform.position.z - Distance);
        //    this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, Pos, Speed * Time.deltaTime);
        //}

        //if (GameManager.instance.objPlayer.GetComponent<Char_Base>().CS != GameManager.CharState.Death)
        //{
        //    Target = GameManager.instance.objPlayer;
        //}
        //else if (GameManager.instance.objHealer.GetComponent<Char_Base>().CS != GameManager.CharState.Death)
        //{
        //    Target = GameManager.instance.objHealer;
        //}
        //else if (GameManager.instance.objThief.GetComponent<Char_Base>().CS != GameManager.CharState.Death)
        //{
        //    Target = GameManager.instance.objThief;
        //}

    }
}
