using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    public SkillTreeSlot dashUnlockedButton;

    [Header("CloneOnDash")]
    public bool cloneOnDashUnlocked;
    public SkillTreeSlot cloneOnDashUnlockedButton;

    [Header("CloneOnArrival")]
    public bool cloneOnArrivalUnlocked;
    public SkillTreeSlot cloneOnArrivalUnlockedButton;

    protected override void SkillFunction()
    {
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
    private void UnlockDash()
    {
        if (dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash(Transform playerTransform)
    {
        if (!cloneOnDashUnlocked) return;
        SkillManager.Instance.Clone.CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }

    public void CloneOnArrival(Transform playerTransform)
    {
        if (!cloneOnArrivalUnlocked) return;
        SkillManager.Instance.Clone.CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }
}
