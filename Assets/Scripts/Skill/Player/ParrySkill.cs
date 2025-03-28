using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked;

    [Header("Parry restore")]
    [SerializeField] private SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPerentage;
    public bool restoreUnlocked;

    [Header("Parry with mirage")]
    [SerializeField] private SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked;


    protected override void SkillFunction()
    {
        if (restoreUnlocked)
        {
            var playerStat = PlayerManager.Instance.player.GetComponent<Damageable>();
            int restoreAmount = Mathf.RoundToInt(playerStat.MaxHp.GetValue() * restoreHealthPerentage);
            playerStat.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked) 
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked) 
            restoreUnlocked = true;
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.Instance.Clone.CreateCloneOnCounterAttack(respawnTransform);
    }
}
