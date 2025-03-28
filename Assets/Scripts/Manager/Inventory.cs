using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory Instance { get; private set; }

    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDic;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDic;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDic;

    [Header("UI")]
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform statSlotParent;

    private EquipmentSlot[] equipmentSlots;
    private ItemSlot[] inventoryItemSlots;
    private ItemSlot[] stashItemSlots;
    private StatsSlot[] statSlots;

    [Header("ItemsCooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        equipmentItems = new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItems = new List<InventoryItem>();
        inventoryDic = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDic = new Dictionary<ItemData, InventoryItem>();

        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<StatsSlot>();

        AddStartItem();
    }

    private void AddStartItem()
    {
        foreach (ItemDataEquipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
        }
    }

    public void EquipItem(ItemData item)
    {
        var newEquipment = item as ItemDataEquipment;
        var newItem = new InventoryItem(newEquipment);

        var old = equipmentDic.FirstOrDefault(x => x.Key.equipmentType == newEquipment.equipmentType).Key;

        if (old)
        {
            UnEquipMethod(old);
            AddItemMethod(inventoryItems, inventoryDic, old);
        }

        RemoveItemMethod(inventoryItems, inventoryDic, item);
        EquipMethod(newEquipment, newItem);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
    }

    public void UnEquipItem(ItemData item)
    {
        var unequipData = item as ItemDataEquipment;
        var unequipItem = new InventoryItem(unequipData);

        UnEquipMethod(unequipData);
        AddItemMethod(inventoryItems, inventoryDic, unequipData);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
    }

    private void EquipMethod(ItemDataEquipment newEquipment, InventoryItem newItem)
    {
        equipmentItems.Add(newItem);
        equipmentDic.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
    }

    private void UnEquipMethod(ItemDataEquipment old)
    {
        if (equipmentDic.TryGetValue(old, out var oldValue))
        {
            equipmentItems.Remove(oldValue);
            equipmentDic.Remove(old);
            old.RemoveModifiers();
        }
    }

    public void AddItem(ItemData item)
    {
        if (!CanAddItem()) return;
        switch (item.itemType)
        {
            case ItemType.Material:
                AddItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                AddItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }

    public void RemoveItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                RemoveItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                RemoveItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }

    private void AddItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            var newItem = new InventoryItem(item);
            items.Add(newItem);
            itemDic.Add(item, newItem);
        }
    }

    private void RemoveItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                items.Remove(value);
                itemDic.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }

    public void UpdateSlotUI(ItemSlot[] slots, List<InventoryItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i] as EquipmentSlot;

            if (!slot)
            {
                slots[i].UpdateSlot(i < items.Count ? items[i] : null);
            }
            else
            {
                slots[i].UpdateSlot(null);
                for (int j = 0; j < items.Count; j++)
                {
                    var data = items[j].data as ItemDataEquipment;
                    if (!data || slot.slotType != data.equipmentType) continue;
                    slots[i].UpdateSlot(items[j]);
                }
            }
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValue();
        }
    }

    public ItemDataEquipment GetEquipmentByType(EquipmentType equipmentType)
    {
        ItemDataEquipment equipment = null;
        foreach (var item in equipmentItems)
        {
            var equipmentData = item.data as ItemDataEquipment;
            if (!equipmentData || equipmentData.equipmentType != equipmentType) continue;
            equipment = equipmentData;
        }
        return equipment;
    }

    public void UsedFlask()
    {
        var currentFlask = GetEquipmentByType(EquipmentType.Flask);
        if (!currentFlask) return;
        var canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
        if (canUseFlask)
        {
            flaskCooldown = currentFlask.ItemCooldown;
            currentFlask.ExecuteItemEffect(null, null);
            lastTimeUsedFlask = Time.time;
        }
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipmentByType(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.ItemCooldown;
            lastTimeUsedArmor = Time.time; 
            return true;
        }
        return false;
    }

    public bool CanAddItem()
    {
        return inventoryItems.Count < inventoryItemSlots.Length;
    }

    public bool CanCraft(ItemDataEquipment craftData, List<InventoryItem> requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDic.TryGetValue(requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(craftData); 
        Debug.Log("Here is your item" + craftData.name);
        return true;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDic)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDic)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDic)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[]{ "Assets/Scripts/Item/ScriptableObject/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
}