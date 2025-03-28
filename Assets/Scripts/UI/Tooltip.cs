using TMPro;
using UnityEngine;

public class Tooltip : ToolTipUtil
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescriptionText.text = item.GetDescription();

        AdjustPosition();
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}