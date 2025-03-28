using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;

    public bool dodgeUnlocked;
    [Header("Mirage dodge")]
    [SerializeField] private SkillTreeSlot unlockMirageDodge;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    protected override void SkillFunction()
    {
        
    }

    protected override void CheckUnlock()
    {
        UnlockDodge(); 
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            var playerStat = PlayerManager.Instance.player.GetComponent<Damageable>();
            Inventory.Instance.UpdateStatsUI();
            playerStat.Evasion.AddModifier(evasionAmount);
            
        }
            dodgeUnlocked = true;
    }
    private void UnlockMirageDodge()
    {
        if (unlockMirageDodge.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.Instance.Clone.CreateClone(player.transform.position, Quaternion.identity, new Vector3(2 * player.Flip.facingDir, 0));
    }
}
