using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //������ "�������" ��������
    public Sprite emptyItem;
    //������� �������
    public Item currentItem;
    //������ �������� ��������
    private int currentItemIndex;

    //UI ���������
    private GameObject inventory;
    //UI �������� �������� � ���� ������
    private GameObject itemObject;
    //������ �� Image
    private Image itemImage;
    //������ �� ������� ������ ��� �������� (���������, ����� � ���������)
    private PlayerLookDirectionChecker currentChecker;
    //������ �� ��������� raycast (���������, ���� � ���������)
    private PlayerRaycast currentRaycaster;
    //����� � ���� ����������� - ������ (����� �� ������ ������ ��� - ��������)
    private Image[] slots;

    //UI ���������� � ������� ��������: �����������, ��� � ��������
    private Image currentItemImage;
    private TMPro.TMP_Text currentItemName, currentItemInfo;

    //cooldown ����� �����������
    private float inventoryCooldown = 0.5f;
    //������� ����� cooldown
    private float currentCooldown = -0.01f;

    //'����������' ���������
    private Item[] items;
    private int maxItemsCount = 20;
    private AudioSource audioSource;

    //������� � ���� (�� �������� �������� ������ item, ������ �������)
    private GameObject inHandObject = null;
    //UI ���������� � �������
    private GameObject computerUI, noteUI;
    //����� ���������� �����
    private TMPro.TMP_Text coinsText;

    //���������� UI ���� - �� ���������� ������ ���������, ���� � ����.
    private bool canUpdate = true;

    public bool Updating { get => canUpdate; set => canUpdate = value; }

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Inventory instance error!");
            return;
        }
        instance = this;
    }
    #endregion

    public bool PutItemInHand(Sprite inHandObjectImage, GameObject obj)
    {
        if (obj == null)
            return false;
        if (inHandObject != null)
        {
            SubtitleReader.instance.SpeakLine(StringResources.instance.
                ElementAt(StringResources.LocalDictionaryType.speech, "cant_pick_item")[0],
                new Color(244f / 255f, 1f, 208f / 255f, 160f / 255f));
            return false;
        }
        if (currentItem != null)
        {
            SubtitleReader.instance.SpeakLine(StringResources.instance.
                ElementAt(StringResources.LocalDictionaryType.speech, "cant_pick_equipped")[0],
                new Color(244f / 255f, 1f, 208f / 255f, 160f / 255f));
            return false;
        }

        itemImage.sprite = inHandObjectImage;
        itemImage.color = new Color(1f, 1f, 1f, 128f / 255f);
        inHandObject = obj;
        return true;
    }

    private void DropItemFromHand()
    {
        if (inHandObject != null)
        {
            itemImage.sprite = emptyItem;
            itemImage.color = new Color(1f, 1f, 1f, 32f / 255f);
            //�������� ������ � ������������� ��� �������
            inHandObject.SetActive(true);
            inHandObject.transform.position = currentRaycaster.transform.position;
            inHandObject = null;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        inventory = transform.Find("Inventory").gameObject;
        coinsText = inventory.transform.Find("Coins").Find("Text").GetComponent<TMPro.TMP_Text>();
        itemObject = transform.Find("Current Item Image").gameObject;
        itemImage = itemObject.GetComponent<Image>();
        GameObject tmp = GameObject.Find("FPS Controller").
            transform.Find("Main Camera").gameObject;
        audioSource = tmp.GetComponent<AudioSource>();
        currentChecker = tmp.GetComponent<PlayerLookDirectionChecker>();
        currentRaycaster = tmp.GetComponent<PlayerRaycast>();
        slots = inventory.transform.Find("Slots").GetComponentsInChildren<Image>();
        
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        computerUI = GameObject.Find("Main Canvas").transform.Find("Computer UI").gameObject;
        noteUI = GameObject.Find("Main Canvas").transform.Find("Log").Find("Note").gameObject;

        items = new Item[maxItemsCount];
        for (int i = 0; i < maxItemsCount; i++)
        {
            items[i] = null;
        }

        tmp = inventory.transform.Find("Item Info").gameObject;
        currentItemImage = tmp.transform.Find("Image").GetComponent<Image>();
        currentItemName = tmp.transform.Find("Name").GetComponent<TMPro.TMP_Text>();
        currentItemInfo = tmp.transform.Find("Info").GetComponent<TMPro.TMP_Text>();
        currentItemIndex = -1;

        UpdateInventoryUI();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Updating)
            return;
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        float input = Input.GetAxis("Inventory");
        if (input > 0 && currentChecker.canMove)
        {
            DropItemFromHand();
            inventory.SetActive(!inventory.activeInHierarchy);
            itemObject.SetActive(!itemObject.activeInHierarchy);
            currentChecker.enabled = !currentChecker.enabled;
            currentRaycaster.enabled = !currentRaycaster.enabled;

            if (inventory.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                UpdateInventoryUI();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            currentCooldown = inventoryCooldown;
        }

        input = Input.GetAxis("Drop");
        if (input > 0)
        {
            DropItemFromHand();
            currentCooldown = inventoryCooldown;
        }
    }

    public void EnableUserLook(bool enable)
    {
        if (currentChecker == null)
            return;
        //���� �� ��������� � ����������, �������� ��� � ��������� - �� �������� ����
        if(computerUI.activeInHierarchy || inventory.activeInHierarchy)
        {
            currentChecker.enabled = false;
            currentRaycaster.enabled = false; 
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            return;
        }
        currentChecker.enabled = enable;
        currentRaycaster.enabled = enable;
        //���� � ������� - �� ��������� � ����� ������ currentChecker � raycaster
        if (noteUI.activeInHierarchy)
        {
            currentChecker.enabled = false;
            currentRaycaster.enabled = false;
        }
        if (!enable)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UpdateInventoryUI()
    {
        coinsText.text = StringResources.instance.GetCoinsCountAndMaxCoinsCount().Key +
            "/" + StringResources.instance.GetCoinsCountAndMaxCoinsCount().Value;
        for(int i = 0; i < maxItemsCount; i++)
        {
            slots[i].color = Color.white;
            if (items[i] != null)
                slots[i].sprite = items[i].photo;
            else
                slots[i].sprite = emptyItem;
        }

        if (currentItem != null)
        {
            currentItemImage.sprite = currentItem.photo;
            itemImage.sprite = currentItem.photo;
            itemImage.color = new Color(1f, 1f, 1f, 128f / 255f);
            currentItemInfo.text = currentItem.info;
            currentItemName.text = currentItem.name;
            slots[currentItemIndex].color = new Color(0.5f, 0.5f, 0.5f);
            return;
        }

        currentItemImage.sprite = emptyItem;
        itemImage.sprite = null;
        itemImage.color = new Color(1f, 1f, 1f, 32f / 255f);
        currentItemInfo.text = "";
        currentItemName.text = "";
    }

    public void UnEquipAndRemove()
    {
        int index = currentItemIndex;
        EquipItem(-1);
        RemoveItem(index);
    }

    public void EquipItem(int index)
    {
        currentItem = null;

        if (!(index < 0 || index > maxItemsCount ||
            items[index] == null || currentItemIndex == index))
        {
            currentItemIndex = index;
            currentItem = items[index];
        }
        else 
            currentItemIndex = -1;

        UpdateInventoryUI();
    }

    public bool AddItem(Item item)
    {
        int index = -1;

        //���� ������ �� ������� ������ ����
        for(int i = 0; i < maxItemsCount; i++)
        {
            if (items[i] == null)
            {
                index = i;
                break;
            }
        }

        //��� ����� � ���������
        if (index < 0)
            return false;

        audioSource.PlayOneShot(item.pickSound);
        //��������� ������� � ����
        items[index] = item;
        if (inventory.activeInHierarchy)
            UpdateInventoryUI();
        return true;
    }

    public bool RemoveItem(int index)
    {
        if (index < 0 || index > maxItemsCount || items[index] == null)
            return false;
        audioSource.PlayOneShot(items[index].useSound);
        //������� ����
        items[index] = null;
        if (inventory.activeInHierarchy)
            UpdateInventoryUI();
        return true;
    }
}
