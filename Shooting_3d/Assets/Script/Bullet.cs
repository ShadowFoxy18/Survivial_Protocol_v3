using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] int damage = 10;
    [SerializeField] GameObject impactParticlePrefab;

    void OnEnable()
    {
        // Reset velocity every time bullet is reused from pool
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Deal damage if hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Uncomment when EnemyHealth script is ready
            // collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        // Return to pool on any collision
        BulletPool.instance.PushObject(gameObject);
    }
}
