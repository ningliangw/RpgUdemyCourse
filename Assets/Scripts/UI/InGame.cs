using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class InGame : MonoBehaviour
{
    private Damageable damageable;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentSouls;

    private SkillManager skills;

    private void Awake()
    {
        
    }

    void Start()
    {
        damageable = PlayerManager.Instance.player.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnTakeDamage += UpdateHpBar;
        }

        skills = SkillManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentSouls.text = PlayerManager.Instance.GetCurrency().ToString("#,#");

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.Dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.Crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && skills.Sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.Blackhole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.GetEquipmentByType(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.Dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.Crystal.cooldown);
        CheckCooldownOf(swordImage, skills.Sword.cooldown);
        CheckCooldownOf(blackholeImage, skills.Blackhole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.Instance.flaskCooldown);
    }

    private void UpdateHpBar(GameObject object1, GameObject object2)
    {
        slider.maxValue = damageable.GetMaxHealthValue();
        slider.value = damageable.currentHp;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
    }
}
