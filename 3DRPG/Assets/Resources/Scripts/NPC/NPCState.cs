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


public class TradeNPC : NPCState
{

    public List<int> ItemList = new List<int>();

    public override void ClickEvent()
    {
        if (EventUI.activeSelf == false)
        {
            EventUI.SetActive(true);
            Player_Inventory.Instance.objInventory.SetActive(true);
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

