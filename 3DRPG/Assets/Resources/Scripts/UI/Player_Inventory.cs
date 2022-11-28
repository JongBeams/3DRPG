using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    public Transform slot;

    public List<ItemSlot> m_lSlot = new List<ItemSlot>();

    public List<ItemSlot> m_lPlayerWearingSlot = new List<ItemSlot>();
    public List<ItemSlot> m_lPartner1WearingSlot = new List<ItemSlot>();
    public List<ItemSlot> m_lPartner2WearingSlot = new List<ItemSlot>();

    public int ver = 6;//����
    public int hor = 6;//����

    public float anchorSize = 0.15f;

    public GameObject objInventory;


    public void RemoteStart()
    {
        slot = Resources.Load<Transform>("Prefabs/UI/ItemSlot");
        objInventory = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory"));
        objInventory.transform.parent = GameManager.instance.objCanvas.transform;
        objInventory.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;


        //�κ��丮 ����
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
                //ParslotRect = ��ü�Ҵ������ = PR
                //������ġ x�� =  -��ü����/(����*2) + (��ü����/����*j)
                //������ġ y�� =  (��ü����/2 - ��ü����/(����*2)) - (��ü����/����*i)
                //��ü����/(2-����*2)
                //��ü����/2*(1-����)
                //(ParslotRect.sizeDelta.y / 2 - ParslotRect.sizeDelta.y / (ver * 2)) 
                // ParslotRect.sizeDelta.y / 2*(1 - ver)



                slotRect.localScale = new Vector3(1, 1, 1);

                m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
                newSlot.GetComponent<ItemSlot>().m_nSlotNum = i * ver + j;

                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
            }
            //objInventory.SetActive(false);

            
        }

        //�÷��̾� ���� ����
        for (int i = 0; i < 2; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = this.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(-125 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);



            m_lPlayerWearingSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }

        //����1 ���� ����
        for (int i = 0; i < 2; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = this.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(-25 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);



            m_lPartner1WearingSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }

        //����2 ���� ����
        for (int i = 0; i < 2; i++)
        {
            Transform newSlot = Instantiate(slot);
            newSlot.name = "Slot" + (i + 1);
            newSlot.parent = objInventory.transform.GetChild(1);
            newSlot.transform.tag = "WearItemSlot";
            RectTransform slotRect = newSlot.GetComponent<RectTransform>();
            RectTransform ParslotRect = this.transform.GetComponent<RectTransform>();

            slotRect.sizeDelta = new Vector2(40, 40);

            slotRect.anchoredPosition = new Vector3(75 + (45 * i), 0, 0);

            slotRect.localScale = new Vector3(1, 1, 1);



            m_lPartner2WearingSlot.Add(newSlot.GetComponent<ItemSlot>());
            newSlot.GetComponent<ItemSlot>().m_nSlotNum = i;

            newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
        }


        AddItem(1);
    }


    public void AddItem(int number)
    {
        //bool checkslot = false;
        int i = 0;
        for (i = 0; i < m_lSlot.Count; i++)
        {
            //            Debug.Log((i+1)+"��° ĭ ItemID : "+slotScripts[i].item.ItemID);
            if (m_lSlot[i].item.getID() == 0)
            {
                m_lSlot[i].item = CharDataBase.instance.m_lItemDB[number];
                ItemImageChange(m_lSlot[i].transform);

                break;
            }
        }
        if (i == m_lSlot.Count)
        {
            Debug.Log("������ ����");
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

}
