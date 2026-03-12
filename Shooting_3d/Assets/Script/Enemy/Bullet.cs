using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] int damage = 10;
    [SerializeField] GameObject impactParticlePrefab;

    void OnEnable()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        if (impactParticlePrefab != null)
        {
            GameObject particle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, 2f);
        }

        BulletPool.instance.PushObject(gameObject);
    }
}
