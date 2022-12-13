using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public GameObject Item;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Item.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("Mesh/Shield");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Item.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("Mesh/Sword");
        }
    }
}
