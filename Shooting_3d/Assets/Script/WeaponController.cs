using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // ------- Weapon Settings ------- //
    [Header("Weapon Settings")]
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float bulletForce = 500f;
    [SerializeField] float raycastRange = 100f;
    public float fireRate = 0.5f;

    [SerializeField] float animationTime = 3f;
    float actualTime = 0;

    [Header("Shoot Timing")]
    [SerializeField] float shootAnimDuration = 0.5f;

    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform leftHandBone;

    float shootAnimTimer = 0f;

    // ------- Components ------- //
    PlayerInfo playerInfo;

    // -------------------- Unity Methods -------------------- //

    void Awake()
    {
        playerInfo = GetComponentInParent<PlayerInfo>();
    }

    void Update()
    {
        HandleWeaponPosition();

        shootAnimTimer -= Time.deltaTime;
    }

    // -------------------- Shoot -------------------- //

    public void Shoot()
    {
        AnimatorController.instance.SetShooting(true);
        shootAnimTimer = shootAnimDuration;

        Vector3 target = GetAimPoint();
        FireBullet(target);
    }

    // -------------------- Aim Raycast -------------------- //

    Vector3 GetAimPoint()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
        {
            return hit.point;
        }

        return cameraTransform.position + cameraTransform.forward * raycastRange;
    }

    void FireBullet(Vector3 target)
    {
        GameObject bullet = BulletPool.instance.PopObject();
        bullet.transform.position = muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(target - muzzlePoint.position);
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(
            (target - muzzlePoint.position).normalized * bulletForce,
            ForceMode.Impulse
        );

        playerInfo.ConsumeAmmo();

        actualTime = animationTime;

        AnimatorController.instance.SetShooting(false);
    }

    // -------------------- Weapon Position -------------------- //

    void HandleWeaponPosition()
    {
        if (leftHandBone != null)
        {
            transform.position = leftHandBone.position;
            transform.rotation = leftHandBone.rotation;
        }
    }

    // -------------------- Reload -------------------- //

    public void Reload()
    {
        playerInfo.TryReload();
    }

    // -------------------- Public Getters -------------------- //

    public bool IsShooting()
    {
        return shootAnimTimer > 0f;
    }
}
