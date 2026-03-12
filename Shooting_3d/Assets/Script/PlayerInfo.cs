using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    // ------- Health ------- //
    [Header("Health Settings")]
    [SerializeField] float maxHealth = 200f;
    [SerializeField] float gameOverDelay = 2f;

    float currentHealth;
    bool isDead = false;

    // ------- Ammo ------- //
    [Header("Ammo Settings")]
    [SerializeField] int maxAmmo = 15;
    [SerializeField] bool infiniteAmmo = true;
    [SerializeField] float reloadTime = 2f;

    int currentAmmo;
    bool isReloading = false;

    // ------- UI ------- //
    [Header("UI")]
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject gameOverCanvas;

    // ------- Components ------- //
    PlayerBehavior playerBehavior;

    // -------------------- Unity Methods -------------------- //

    void Awake()
    {
        playerBehavior = GetComponent<PlayerBehavior>();

        currentHealth = maxHealth;
        currentAmmo = maxAmmo;

        gameOverCanvas.SetActive(false);

        UpdateHealthUI();
        UpdateAmmoUI();
    }

    // -------------------- Health -------------------- //

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            UpdateHealthUI();

            if (currentHealth <= 0f)
            {
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        playerBehavior.SetPermitedAct(false);
        AnimatorController.instance.SetDead(true);

        yield return new WaitForSeconds(gameOverDelay);

        gameOverCanvas.SetActive(true);
    }

    // -------------------- Ammo -------------------- //

    public bool CanShoot()
    {
        return !isReloading && currentAmmo > 0;
    }

    public void ConsumeAmmo()
    {
        if (!infiniteAmmo)
        {
            currentAmmo--;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }

        UpdateAmmoUI();

        // Auto reload when empty — only if not already reloading
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    public void TryReload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        playerBehavior.SetPermitedAct(false);
        AnimatorController.instance.SetReload(true);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        playerBehavior.SetPermitedAct(true);
        AnimatorController.instance.SetReload(false);
        UpdateAmmoUI();
    }

    // -------------------- UI -------------------- //

    void UpdateHealthUI()
    {
        float percentage = currentHealth / maxHealth;
        healthBar.fillAmount = percentage;
        healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    // -------------------- Public Getters -------------------- //

    public bool IsReloading() => isReloading;
    public bool IsDead() => isDead;
}
