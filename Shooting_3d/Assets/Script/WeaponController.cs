using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float bulletForce = 500f;
    [SerializeField] float fireRate = 0.5f;

    [Header("Ammo")]
    [SerializeField] int maxAmmo = 15;
    [SerializeField] bool infiniteAmmo = true;
    [SerializeField] float reloadTime = 2f;

    int currentAmmo;
    bool isReloading = false;
    float lastShotTime = 0f;

    // -- Events -- //
    public System.Action OnReloadStart;
    public System.Action OnReloadEnd;

    void Awake()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Auto reload when empty
        if (currentAmmo <= 0 && !isReloading)
        {
            TryReload();
        }
    }

    public void TryShoot()
    {
        if (!isReloading && currentAmmo > 0 && Time.time >= lastShotTime + fireRate)
        {
            Shoot();
            lastShotTime = Time.time;
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
        OnReloadStart?.Invoke();

        AnimatorController.instance.SetReload(true);

        yield return new WaitForSeconds(reloadTime);

        if (infiniteAmmo)
        {
            currentAmmo = maxAmmo;
        }

        AnimatorController.instance.SetReload(false);
        AnimatorController.instance.SetShooting(false);

        isReloading = false;
        OnReloadEnd?.Invoke();
    }

    void Shoot()
    {
        GameObject bullet = BulletPool.instance.PopObject();
        bullet.transform.position = muzzlePoint.position;
        bullet.transform.rotation = muzzlePoint.rotation;
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(muzzlePoint.forward * bulletForce, ForceMode.Impulse);

        AnimatorController.instance.SetShooting(true);

        if (!infiniteAmmo)
        {
            currentAmmo--;
        }
    }

    // -- Public getters for UI -- //
    public bool IsReloading() => isReloading;
    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
}
