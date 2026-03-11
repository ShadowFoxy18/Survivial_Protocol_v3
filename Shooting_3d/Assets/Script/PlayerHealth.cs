using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] float maxHealth = 200f;
    [SerializeField] float gameOverDelay = 2f;

    [Header("UI")]
    [SerializeField] Image healthBar;

    float currentHealth;
    bool isDead = false;

    // -- Events -- //
    public System.Action OnPlayerDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            // Update health bar as percentage
            if (healthBar != null)
            {
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            if (currentHealth <= 0f)
            {
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        // Block all player actions
        GetComponent<CharacterController>().SetPermitedAct(false);

        // Trigger death animation
        AnimatorController.instance.SetDead(true);

        yield return new WaitForSeconds(gameOverDelay);

        // Notify GameManager
        OnPlayerDeath?.Invoke();
    }

    // -- Public getters -- //
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public bool IsDead() => isDead;
}
