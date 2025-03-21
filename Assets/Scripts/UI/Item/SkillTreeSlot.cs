using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    private UI ui;

    [SerializeField] public bool unlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Image skillImage;
    [SerializeField] private string skillName;
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    public void UnlockSkill()
    {
        foreach (var item in shouldBeUnlocked)
        {
            if (!item.unlocked) return;
        }

        foreach (var item in shouldBeLocked)
        {
            if (item.unlocked) return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTips.ShowToolTip(skillDescription, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTips.HideToolTip();
    }*/
}