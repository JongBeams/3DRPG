using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemInfo : MonoBehaviour
{

    public MeshFilter mesh;

    public MeshRenderer MR;

    int ID = 0;

    // Start is called before the first frame update
    void Start()
    {
        mesh = this.gameObject.transform.GetChild(0).GetComponent<MeshFilter>();
        MR = this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
    }


    public void SetItem(int _ID,string _Mesh, string _Material)
    {
        ID = _ID;

        mesh.mesh = Resources.Load<Mesh>(_Mesh);
        MR.material= Resources.Load<Material>(_Material);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
