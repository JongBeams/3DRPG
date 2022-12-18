using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcEvent : MonoBehaviour
{
    public NPCState NS;
    
    // Start is called before the first frame update
    void Start()
    {
        NS = new QuestNPC();
        NS.EventUI = SelectSceneManager.Instance.objEnemySelectUI;
        Debug.Log(NS.EventUI);
    }

    public void Set(NPCState _NS)
    {
        NS = _NS;
    }


    // Update is called once per frame
    void Update()
    {
        NS.Update();
    }
}
