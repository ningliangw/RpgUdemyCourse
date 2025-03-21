using TMPro;
using UnityEngine;

public class StatsSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText) statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        var playerStat = PlayerManager.Instance.player.GetComponent<Damageable>();
        if (playerStat)
        {
            statValueText.text = playerStat.StatsOfType(statType).GetValue().ToString();
        }

        if (statType == StatType.Health)
            statValueText.text = playerStat.MaxHp.GetValue().ToString();

        if (statType == StatType.Damage)
            statValueText.text = (playerStat.Damage.GetValue() + playerStat.Str.GetValue()).ToString();

        if (statType == StatType.CritPower) 
            statValueText.text = (playerStat.CritPower.GetValue() + playerStat.Str.GetValue()).ToString();

        if (statType == StatType.CritChance) 
            statValueText.text = (playerStat.CritChance.GetValue() + playerStat.Agi.GetValue()).ToString();

        if (statType == StatType.Evasion) 
            statValueText.text = (playerStat.Evasion.GetValue() + playerStat.Agi.GetValue()).ToString();

        if (statType == StatType.MagicRes)
            statValueText.text = (playerStat.MagicResistance.GetValue() + (playerStat.Int.GetValue() * 3)).ToString();
    }
}