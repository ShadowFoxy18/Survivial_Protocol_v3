using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // -------------------- Health Settings -------------------- //

    [Header("Health Settings")]
    [SerializeField] float maxHealth = 100f;

    float currentHealth;
    bool isDead = false;

    // -------------------- Shoot Settings -------------------- //

    [Header("Shoot Settings")]
    [SerializeField] float fireRate = 2f;

    float fireRateTimer = 0f;
    bool canShoot = true;

    // -------------------- Unity Methods -------------------- //

    void Awake()
    {
        currentHealth = maxHealth;
        GameManager.instance.RegisterEnemy();
    }

    void Update()
    {
        CheckFireRate();
    }

    // -------------------- Fire Rate -------------------- //

    void CheckFireRate()
    {
        if (!canShoot)
        {
            fireRateTimer -= Time.deltaTime;

            if (fireRateTimer <= 0f)
            {
                canShoot = true;
            }
        }
    }

    public bool CanShoot()
    {
        return canShoot && !isDead;
    }

    public void OnShoot()
    {
        canShoot = false;
        fireRateTimer = fireRate;
    }

    // -------------------- Damage -------------------- //

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        GameManager.instance.EnemyDied();

        // TODO: animacion de muerte
        Destroy(gameObject);
    }
}
