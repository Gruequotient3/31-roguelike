using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager singleton;

    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public CraftSlot[] craftSlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryGroup;
    int selectedSlot = -1;


    private bool active = false;
    private InputAction _inventory;
    private InputAction _numKeys;


    void Awake()
    {
        if (singleton != null)
        {
            UnityEngine.GameObject.Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
        
        
        inventoryGroup = GameObject.FindWithTag("InventoryGroup");
        SearchInventorySlot();
        SearchCraftingSlot();
    }

    void Start()
    {
        _inventory = InputSystem.actions.FindAction("Inventory");
        _numKeys = InputSystem.actions.FindAction("NumKey");

        ChangeSelectedSlot(0);
        if (inventoryGroup)
        {
            SetActive(false);
        }
        
    }

    public void SearchInventorySlot()
    {
        if (!inventoryGroup) return;
        GameObject tb = inventoryGroup.transform.GetChild(0).gameObject;
        for (int i = 0; i < tb.transform.childCount; ++i)
        {
            InventorySlot slot = tb.transform.GetChild(i).GetComponent<InventorySlot>();
            inventorySlots[i] = slot;
        } 

        GameObject invS = inventoryGroup.transform.GetChild(1).gameObject;
        for (int i = 0; i < invS.transform.childCount; ++i)
        {
            InventorySlot slot = invS.transform.GetChild(i).GetComponent<InventorySlot>();
            inventorySlots[i + tb.transform.childCount] = slot;
        }
    }

    public void SearchCraftingSlot()
    {
        if (!inventoryGroup) return;
        GameObject ct = inventoryGroup.transform.GetChild(2).gameObject;

        for (int i = 0; i < craftSlots.Length; ++i)
        {
            CraftSlot slot = ct.transform.GetChild(i).GetComponent<CraftSlot>();
            craftSlots[i] = slot;
        } 
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        if (inventorySlots[newValue])
            inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    void Update()
    {
        _numKeys.performed += context =>
        {
            bool isNumber = int.TryParse(context.control.name, out int number);
            if (isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        };
        if (_inventory.triggered && inventoryGroup)
        {
            SetActive(!active);
            WorldInteractor worldInteractor = FindFirstObjectByType<WorldInteractor>();
            if (worldInteractor)
            {
                worldInteractor.SetActive(!active);
            }
        }
    }

    public bool AddItem(Item item, int value)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null
                && itemInSlot.item == item
                && itemInSlot.count < maxStackedItems
                && itemInSlot.item.stackable)
            {
                int maxValue =  maxStackedItems - itemInSlot.count;
                int addedValue = maxValue < value ? maxValue : value;
                value -= addedValue;
                itemInSlot.count += addedValue;
                itemInSlot.RefreshCount();
                if(value <= 0) return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {

                int maxValue =  maxStackedItems;
                int addedValue = maxValue < value ? maxValue : value;
                value -= addedValue;
                SpawnNewItem(item, slot, addedValue);
                if(value <= 0) return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot, int count)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item, count);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }
        return null;
    }

    public void SetActive(bool value)
    {
        if (!inventoryGroup) return;
        for (int i = 1; i < inventoryGroup.transform.childCount; ++i)
        {
            inventoryGroup.transform.GetChild(i).gameObject.SetActive(value);
        }
        active = value;
    }
}
