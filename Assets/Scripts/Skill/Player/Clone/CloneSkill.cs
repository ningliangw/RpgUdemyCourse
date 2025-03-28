using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone Value")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private Color cloneColor;
    [SerializeField] private bool isDelayAttack;
    [SerializeField] private float attackDelay;

    [Header("Clone attack")]
    [SerializeField] private SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier; 
    [SerializeField] private bool canAttack;

    [Header("Aggresive clone")]
    [SerializeField] private SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Duplicate Clone")]
    [SerializeField] private SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] private SkillTreeSlot crystalInseadUnlockButton;
    [SerializeField] public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack); 
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone); 
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone); 
        crystalInseadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
    }

    #region Unlock region
    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }
    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }
    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }
    private void UnlockCrystalInstead()
    {
        if (crystalInseadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion
    public void CreateClone(Vector3 position, Quaternion rotation, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        var newClone = Instantiate(clonePrefab);
        newClone.transform.SetParent(PlayerManager.Instance.fx.transform);
        var controller = newClone.GetComponent<CloneSkillController>();
        controller.Setup(
            player,
            position,
            rotation,
            cloneDuration,
            cloneColor,
            isDelayAttack,
            offset,
            attackDelay,
            canDuplicateClone,
            duplicateProbability,
            FindClosestEnemy,
            attackMultiplier);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        CreateClone(enemyTransform.position, enemyTransform.rotation, new Vector3(2 * player.Flip.facingDir, 0));
    }

    protected override void SkillFunction()
    {

    }
}
