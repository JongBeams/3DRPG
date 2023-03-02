using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemInfo : MonoBehaviour
{

    public MeshFilter meshfilter;

    public MeshRenderer MR;

    public Mesh mesh;

    int ID = 0;

    float rotate = 0;
    bool getidem = false;

    // Start is called before the first frame update
    void Start()
    {
        meshfilter = this.transform.GetChild(0).GetComponent<MeshFilter>();
        MR = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        
        Vector3 dir = new Vector3(this.transform.position.x+Random.Range(-100f,101f)/500f, this.transform.position.y + 1f, this.transform.position.z + Random.Range(-100f, 101f) / 500f);
        //Vector3 dir = new Vector3(this.transform.position.x+0.1f, this.transform.position.y+1f, this.transform.position.z);
        Debug.Log(dir);
        this.GetComponent<Rigidbody>().AddForce((dir - this.transform.position)*500f);
    }


    public void SetItem(int _ID, string _Mesh, string _Material)
    {
        ID = _ID;
        Debug.Log(_Mesh);
        meshfilter.sharedMesh = Resources.Load<Mesh>(_Mesh);
        MR.materials[0] = Resources.Load<Material>(_Material);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.GetChild(0).rotation = Quaternion.Euler(0, rotate, 0);
        rotate += 120 * Time.deltaTime;
        if (rotate>=360)
        {
            rotate = 0;
        }

        float dis = Vector3.Distance(GameManager.Instance.objPlayer.transform.position, this.transform.position);

        if (!getidem&&dis < 1f)
        {
            Player_Inventory.Instance.AddItem(ID);
            Destroy(this.gameObject, 0.1f);
            getidem = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            
        }
    }

}
