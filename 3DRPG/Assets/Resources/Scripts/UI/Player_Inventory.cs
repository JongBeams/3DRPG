using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    public Transform slot;

    public List<ItemSlot> m_lSlot = new List<ItemSlot>();


    public int ver = 6;//세로
    public int hor = 6;//가로

    int n_mPlayerSlot = 2;
    int n_mPartner1Slot = 2;
    int n_mPartner2Slot = 2;

    public float anchorSize = 0.15f;

    public GameObject objInventory;

    public int getPlayerSlot()
    {
        return n_mPlayerSlot;
    }
    public int getPartner1Slot()
    {
        return n_mPartner1Slot;
    }
    public int getPartner2Slot()
    {
        return n_mPartner2Slot;
    }

    public void RemoteStart()
    {
        slot = Resources.Load<Transform>("Prefabs/UI/ItemSlot");
        objInventory = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory"));
        objInventory.transform.parent = GameManager.instance.objCanvas.transform;
        objInventory.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        

        //인벤토리 슬롯
        for (int i = 0; i < ver; i++)
        {
            for (int j = 0; j < hor; j++)
            {
                Transform newSlot = Instantiate(slot);
                newSlot.name = "Slot" + (i + 1) + "." + (j + 1);
                newSlot.parent = objInventory.transform.GetChild(2);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                RectTransform ParslotRect = objInventory.transform.GetChild(2).GetComponent<RectTransform>();

                slotRect.sizeDelta = new Vector2(ParslotRect.sizeDelta.x / hor, ParslotRect.sizeDelta.y / ver);

                slotRect.anchoredPosition = new Vector3((-ParslotRect.sizeDelta.x / 2 + ParslotRect.sizeDelta.x / (hor * 2)) + (ParslotRect.sizeDelta.x / hor * j), (ParslotRect.sizeDelta.y / 2 - ParslotRect.sizeDelta.y / (ver * 2)) - (ParslotRect.sizeDelta.y / ver * i), 0);
                //ParslotRect = 전체할당사이즈 = PR
                //생성위치 x값 =  -전체가로/(가로*2) + (전체가로/가로*j)
                //생성위치 y값 =  (전체세로/2 - 전체세로/(세로*2)) - (전체세로/세로*i)
                //전체세로/(2-세로*2)
                //전체세로/2*(1-세로)
                //(ParslotRect.sizeDelta.y / 2 - ParslotRect.sizeDelta.y / (ver * 2)) 
                // ParslotRect.sizeDelta.y / 2*(1 - ver)



                slotRect.localScale = new Vector3(1, 1, 1);

                m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
                newSlot.GetComponent<ItemSlot>().m_nSlotNum = i * ver + j;


                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
            }
            //objInventory.SetActive(false);
        }

        //플레이어 착용 슬롯
        for (int i = 0; i < n_mPlayerSlot; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            //newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = objInventory.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(-125 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);


            m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = (ver* hor) + i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }

        //동료1 착용 슬롯
        for (int i = 0; i < n_mPartner1Slot; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            //newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = objInventory.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(-25 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);



            m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = (ver * hor)+ n_mPlayerSlot + i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }

        //동료2 착용 슬롯
        for (int i = 0; i < n_mPartner2Slot; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            //newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = objInventory.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(75 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);



            m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = (ver * hor) + n_mPlayerSlot + n_mPartner1Slot + i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }



        AddItem(1);
        AddItem(2);
        AddItem(1);
        AddItem(2);
        AddItem(1);
        AddItem(2);


        objInventory.SetActive(false);
    }


    public void InventoryLoad()
    {
        GameManager.instance.slM._load();
        GameData GD = GameManager.instance.slM.InvetoryData;
        for (int i = 0; i < (ver * hor) + n_mPlayerSlot + n_mPartner1Slot + n_mPartner2Slot; i++)
        {

            m_lSlot[i].GetComponent<ItemSlot>().item=new ItemData(GD.ItemID[i], GD.ItemName[i], GD.ItemATK[i], GD.ItemDEF[i], GD.ItemSPD[i], GD.ItemSprite[i], GD.ItemType[i]);
            ItemImageChange(m_lSlot[i].transform);
        }
    }

    public void InventorySave()
    {
        List<int> ItemID = new List<int>();
        List<string> ItemName = new List<string>();
        List<int> ItemATK = new List<int>();
        List<int> ItemDEF = new List<int>();
        List<float> ItemSPD = new List<float>();
        List<string> ItemSprite = new List<string>();
        List<int> ItemType = new List<int>();


        for (int i = 0; i < (ver * hor) + n_mPlayerSlot + n_mPartner1Slot + n_mPartner2Slot; i++)
        {
            ItemID.Add(m_lSlot[i].item.getID());
            ItemName.Add(m_lSlot[i].item.getName());
            ItemATK.Add(m_lSlot[i].item.getATK());
            ItemDEF.Add(m_lSlot[i].item.getDEF());
            ItemSPD.Add(m_lSlot[i].item.getSpeed());
            ItemSprite.Add(m_lSlot[i].item.getItemSprite());
            ItemType.Add((int)m_lSlot[i].item.getItemType());

        }
        GameManager.instance.slM.InvetoryData = new GameData(ItemID, ItemName, ItemATK, ItemDEF, ItemSPD, ItemSprite, ItemType);
        GameManager.instance.slM._save();
    }


    public void AddItem(int itmeID)
    {
        //bool checkslot = false;
        int i = 0;
        for (i = 0; i < m_lSlot.Count; i++)
        {
            //            Debug.Log((i+1)+"번째 칸 ItemID : "+slotScripts[i].item.ItemID);
            if (m_lSlot[i].item.getID() == 0)
            {
                m_lSlot[i].item = DBManager.ItemData[itmeID];
                ItemImageChange(m_lSlot[i].transform);

                break;
            }
        }
        if (i == m_lSlot.Count)
        {
            Debug.Log("아이템 꽉참");
        }

    }

    void ItemImageChange(Transform _slot)
    {
        if (_slot.GetComponent<ItemSlot>().item.getID() == 0)
            _slot.GetChild(0).gameObject.SetActive(false);
        else
        {
            _slot.GetChild(0).gameObject.SetActive(true);
            _slot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(_slot.GetComponent<ItemSlot>().item.getItemSprite());
            _slot.GetChild(0).GetComponent<RectTransform>().sizeDelta = _slot.GetComponent<RectTransform>().sizeDelta;
        }
    }


    public void ChangeItemSlot(int MovingItemNum,int DropItemNum)
    {
        
        ItemData Saveitem= m_lSlot[DropItemNum].item;
        m_lSlot[DropItemNum].item = m_lSlot[MovingItemNum].item;
        ItemImageChange(m_lSlot[DropItemNum].transform);
        
        m_lSlot[MovingItemNum].item = Saveitem;
        ItemImageChange(m_lSlot[MovingItemNum].transform);

    }

    public void RemoveItem(int ItemNum)
    {

        m_lSlot[ItemNum].item=new ItemData();
        ItemImageChange(m_lSlot[ItemNum].transform);
    }


}
