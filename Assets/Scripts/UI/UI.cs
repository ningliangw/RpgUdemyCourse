using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public FadeScreen fadeScreen;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public Tooltip tooltip;
    public StatTooltip statTooltip;
    public SkillTooltip skillToolTip;
    public CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(inGameUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
            return;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
            return;
        }
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<FadeScreen>() != null;
            
            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        menu?.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(inGameUI);
    }
}