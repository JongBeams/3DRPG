using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    public Transform slot;
    public List<ItemSlot> slotScripts = new List<ItemSlot>();

    //public GameObject DragItem;
    //public GameObject DragImage = null;

    public int ver = 6;//세로
    public int hor = 6;//가로


    public Transform TradeItem;

    public Text buttontext;

    public bool n_bBuyItem = false;

    public bool n_bSellItem = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RemoteStart()
    {
        for (int i = 0; i < ver; i++)
        {
            for (int j = 0; j < hor; j++)
            {
                Transform newSlot = Instantiate(slot);
                newSlot.name = "Slot" + (i + 1) + "." + (j + 1);
                newSlot.parent = transform;
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                RectTransform ParslotRect = this.transform.GetComponent<RectTransform>();

                slotRect.sizeDelta = new Vector2(ParslotRect.sizeDelta.x / hor, ParslotRect.sizeDelta.y / ver);
                slotRect.anchoredPosition = new Vector3((-ParslotRect.sizeDelta.x / 2 + ParslotRect.sizeDelta.x / (hor * 2)) + (ParslotRect.sizeDelta.x / hor * j), (ParslotRect.sizeDelta.y / 2 - ParslotRect.sizeDelta.y / (ver * 2)) - (ParslotRect.sizeDelta.y / ver * i), 0);



                slotRect.localScale = new Vector3(1, 1, 1);

                slotScripts.Add(newSlot.GetComponent<ItemSlot>());
                newSlot.GetComponent<ItemSlot>().m_nSlotNum = i * ver + j;

                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
            }
        }

        //AddItem(1);
        //AddItem(2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
