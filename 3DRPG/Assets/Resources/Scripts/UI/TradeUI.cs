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

    public bool n_bBuyItem = false;

    public bool n_bSellItem = false;

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


    void ClickItem()
    {
        if (GameManager.Instance.UIObj != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.UIObj.tag == "TItemSlot")
                {
                    if (GameManager.Instance.UIObj.GetComponent<ItemSlot>().item.getID() != 0)//NullReference
                    {

                        //Debug.Log(GameManager.Instance.UIObj.transform.parent);
                        //Debug.Log(this.gameObject.transform);
                        if (GameManager.Instance.UIObj.transform.parent == this.gameObject.transform)
                        {
                            //Debug.Log(GameManager.Instance.UIObj.transform);
                            if (TradeItem == GameManager.Instance.UIObj.transform)
                            {
                                TradeItem = null;
                                n_bBuyItem = false;
                                buttontext.GetComponent<Text>().text = "T\nr\na\nd\ne";
                            }
                            else
                            {
                                TradeItem = GameManager.Instance.UIObj.transform;
                                n_bBuyItem = true;
                                n_bSellItem = false;
                                buttontext.GetComponent<Text>().text = "B\nU\nY";
                            }
                        }
                        else
                        {
                            if (TradeItem == GameManager.Instance.UIObj.transform)
                            {
                                TradeItem = null;
                                n_bSellItem = false;
                                buttontext.GetComponent<Text>().text = "T\nr\na\nd\ne";
                            }
                            else
                            {
                                TradeItem = GameManager.Instance.UIObj.transform;
                                n_bSellItem = true;
                                n_bBuyItem = false;
                                buttontext.GetComponent<Text>().text = "S\ne\nl\nl";
                            }
                        }
                    }
                }

            }


        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
