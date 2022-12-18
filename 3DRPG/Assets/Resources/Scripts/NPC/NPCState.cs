using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCState
{
    public GameObject EventUI;

    public abstract void ClickEvent();

    public abstract void Update();


}

public class QuestNPC : NPCState
{

    public override void ClickEvent()
    {
        if (EventUI.activeSelf == false)
        {
            EventUI.SetActive(true);
        }

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EventUI.activeSelf == true)
            {
                EventUI.SetActive(false);
            }

        }
    }
}

