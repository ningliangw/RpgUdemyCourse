using UnityEngine.EventSystems;

public class CraftSlot : ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if (_data == null)
        {
            return; 
        }
        item.data = _data;

        itemImage.sprite = _data.icon; 
        itemText.text = _data.itemName;

        if (itemText.text.Length > 12)
            itemText.fontSize = itemText.fontSize * .7f;
        else
            itemText.fontSize = 24;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
    }
}