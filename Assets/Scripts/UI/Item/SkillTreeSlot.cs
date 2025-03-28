using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillPrice;

    [SerializeField] public bool unlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkill()
    {
        if (PlayerManager.Instance.HaveEnoughMoney(skillPrice) == false)
        {
            return;
        }
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillPrice);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
            unlocked = value;
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName); 
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}