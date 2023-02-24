using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine;

public class TradeUI : MonoBehaviour
{
    public static TradeUI Instance;

    public Transform slot;
    public List<ItemSlot> m_lSlot = new List<ItemSlot>();

    //public GameObject DragItem;
    //public GameObject DragImage = null;

    public int ver = 8;//세로
    public int hor = 2;//가로


    public Transform TradeItem;

    public TextMeshProUGUI buttontext;

    public bool m_bBuyItem = false;

    public bool m_bSellItem = false;

    public GameObject objInventory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RemoteStart()
    {
        m_lSlot.Clear();
        slot = Resources.Load<Transform>("Prefabs/UI/TradeItemSlot");
        //objInventory = Instantiate(Resources.Load<GameObject>("Prefabs/UI/TradeShop"));
        //objInventory.transform.parent = GameManager.Instance.objCanvas.transform;
        //objInventory.GetComponent<RectTransform>().anchoredPosition = new Vector3(-250, 0, 0);
        //buttontext.GetComponent<TextMeshPro>();

        for (int i = 0; i < ver; i++)
        {
            for (int j = 0; j < hor; j++)
            {
                Transform newSlot = Instantiate(slot);
                newSlot.name = "Slot" + (i + 1) + "." + (j + 1);
                newSlot.parent = objInventory.transform;
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                RectTransform ParslotRect = objInventory.transform.GetComponent<RectTransform>();

                slotRect.sizeDelta = new Vector2(ParslotRect.sizeDelta.x / hor, ParslotRect.sizeDelta.y / ver);
                slotRect.anchoredPosition = new Vector3((-ParslotRect.sizeDelta.x / 2 + ParslotRect.sizeDelta.x / (hor * 2)) + (ParslotRect.sizeDelta.x / hor * j), (ParslotRect.sizeDelta.y / 2 - ParslotRect.sizeDelta.y / (ver * 2)) - (ParslotRect.sizeDelta.y / ver * i), 0);



                slotRect.localScale = new Vector3(1, 1, 1);

                m_lSlot.Add(newSlot.GetComponent<ItemSlot>());
                newSlot.GetComponent<ItemSlot>().m_nSlotNum = i * ver + j;

                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 60);
            }
        }

        AddItem(3);
        //AddItem(2);
        objInventory.SetActive(false);
    }


    public void AddItem(int itmeID)
    {
        //bool checkslot = false;
        int i = 0;
        for (i = 0; i < m_lSlot.Count; i++)
        {
            //            Debug.Log((i+1)+"번째 칸 ItemID : "+slotScripts[i].item.ItemID);
            if (m_lSlot[i].item.ID == 0)
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
        if (_slot.GetComponent<ItemSlot>().item.ID == 0)
            _slot.GetChild(0).gameObject.SetActive(false);
        else
        {
            _slot.GetChild(0).gameObject.SetActive(true);
            _slot.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(_slot.GetComponent<ItemSlot>().item.IMG);
            _slot.GetChild(0).GetComponent<RectTransform>().sizeDelta = _slot.GetComponent<RectTransform>().sizeDelta;
        }
    }


    public void ClickItem()
    {
        if (GameManager.Instance.UIObj != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.UIObj.tag == "TItemSlot"&& GameManager.Instance.UIObj.GetComponent<ItemSlot>().item.TYP!=0)
                {
                    //Debug.Log(GameManager.Instance.UIObj.transform);
                    if (TradeItem == GameManager.Instance.UIObj.transform)
                    {
                        TradeItem = null;
                        m_bBuyItem = false;
                        buttontext.GetComponent<TextMeshProUGUI>().text = "Trade";
                    }
                    else
                    {
                        TradeItem = GameManager.Instance.UIObj.transform;
                        m_bBuyItem = true;
                        m_bSellItem = false;
                        buttontext.GetComponent<TextMeshProUGUI>().text = "BUY";
                    }

                }


                if (GameManager.Instance.UIObj.tag == "ItemSlot" && GameManager.Instance.UIObj.GetComponent<ItemSlot>().item.TYP != 0)
                {
                    if (TradeItem == GameManager.Instance.UIObj.transform)
                    {
                        TradeItem = null;
                        m_bSellItem = false;
                        buttontext.GetComponent<TextMeshProUGUI>().text = "Trade";
                    }
                    else
                    {
                        TradeItem = GameManager.Instance.UIObj.transform;
                        m_bSellItem = true;
                        m_bBuyItem = false;
                        buttontext.GetComponent<TextMeshProUGUI>().text = "Sell";
                    }
                }

            }


        }
    }


    public void Trading()
    {
        if (m_bBuyItem)
        {
            if (GameManager.Instance.m_nGold >= TradeItem.gameObject.GetComponent<ItemSlot>().item.BG)
            {
                Player_Inventory.Instance.AddItem(TradeItem.gameObject.GetComponent<ItemSlot>().item.ID);
                GameManager.Instance.m_nGold -= TradeItem.gameObject.GetComponent<ItemSlot>().item.BG;
            }
            
            
        }
        if (m_bSellItem)
        {
            GameManager.Instance.m_nGold += TradeItem.gameObject.GetComponent<ItemSlot>().item.SG;
            Player_Inventory.Instance.RemoveItem(TradeItem.gameObject.GetComponent<ItemSlot>().m_nSlotNum);
            TradeItem = null;
            m_bSellItem = false;
            buttontext.GetComponent<TextMeshProUGUI>().text = "Trade";
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
