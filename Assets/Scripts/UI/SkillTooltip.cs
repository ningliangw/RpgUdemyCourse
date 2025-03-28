using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltip : ToolTipUtil
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowToolTip(string _skillDescprtion, string _skillName, int _price)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescprtion;
        skillCost.text = "Cost: " + _price;

        AdjustPosition();
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);

}
