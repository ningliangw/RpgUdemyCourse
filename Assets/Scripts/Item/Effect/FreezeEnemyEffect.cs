using UnityEngine;

[CreateAssetMenu(fileName = "FreezeEnemyEffect", menuName = "Data/ItemEffect/FreezeEnemyEffect")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        Damageable damageable = PlayerManager.Instance.GetComponent<Damageable>();
        if (damageable.currentHp > damageable.GetMaxHealthValue() * .1f)
            return;

        if (!Inventory.Instance.CanUseArmor())
        {
            return;
        }
        var colliders = Physics2D.OverlapCircleAll(from.transform.position, 2);
        foreach (var hit in colliders)
        {
            if (!hit.CompareTag("Enemy")) return;
            hit.GetComponent<Enemy>()?.FreezeTimeForSeconds(duration);
        }
    }
}