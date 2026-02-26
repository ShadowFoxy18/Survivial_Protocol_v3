using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField]
    Transform characterHand;
    [SerializeField]
    Transform weaponTrigger;

    [SerializeField]
    Transform weaponTarget;
    /*
    [SerializeField]
    GameObject bullets;
    Rigidbody rbBullet;
    */
    [SerializeField]
    float bulletVelocity = 30;
    public int bulletsCapacity = 10;

    public bool idleAction = false, idleReloadAction = false;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Vector3 animationIdle = Vector3.one;

    [SerializeField]
    Vector3 animationIDleReload = Vector3.one;

    [SerializeField]
    TextMeshProUGUI bulletReload;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        //rbBullet = bullets.GetComponent<Rigidbody>();
        WeaponToHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (idleAction)
        {
            weaponTrigger.localEulerAngles = animationIdle;
        } 



    }

    void WeaponToHand()
    {
        weaponTrigger.localEulerAngles = animationIdle;
        Debug.LogWarning("WeaponToCharacter");
    }



    public void WeaponReloading()
    {
        weaponTrigger.localEulerAngles = animationIDleReload;
        // Detener todo el move Set del bicho.
    }

    public void EndWeaponReloading()
    {
        animator.SetBool("Reload", false);
    }
}
