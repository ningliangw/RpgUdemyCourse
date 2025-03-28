using UnityEngine;

public class ItemThunderStrikeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var player = PlayerManager.Instance.player;
            other.GetComponent<Damageable>().TakeDamage(player, 1f, false, false);
            Invoke(nameof(DestroyMe), 0.5f);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}