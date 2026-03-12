using UnityEngine;
using UnityEngine.AI;

public class PlayerBehavior : MonoBehaviour
{
    // ------- Movement Values ------- //
    Vector2 moveInput;
    Vector2 lookInput;
    bool permitedAct = true;

    // ------- Movement Settings ------- //
    [Header("Move speed")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Mouse Sensibility")]
    [SerializeField] float mouseSensitivity = 100f;

    // ------- Camera ------- //
    [Header("Camera")]
    [SerializeField] Transform playerCamera;

    [Header("Vertical Aim")]
    [SerializeField] bool enableVerticalAim = false;

    [Header("Camera Inertia")]
    [SerializeField] float inertiaThreshold = 5f;
    [SerializeField] float inertiaMaxAngle = 15f;

    float verticalRot = 0f;
    float currentInertiaAngle = 0f;

    // ------- Weapon ------- //
    [Header("Weapon")]
    [SerializeField] WeaponController weapon;

    float rate = 0f;

    // ------- Components ------- //
    Rigidbody rb;
    NavMeshAgent agent;
    PlayerInfo playerInfo;

    // --------------- Input Actions ----------------- //
    private GameplayBehavior inputActions;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    // ----------------------------------------------- //

    // -------------------- Unity Methods -------------------- //

    void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
        playerInfo = GetComponent<PlayerInfo>();
        agent.speed = moveSpeed;
        rb.isKinematic = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputActions = new GameplayBehavior();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        inputActions.Player.Lock.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Lock.canceled  += ctx => lookInput = Vector2.zero;

        inputActions.Player.Shoot.performed += ctx => TryShoot();
        inputActions.Player.Shoot.canceled  -= ctx => TryShoot();

        inputActions.Player.Reload.performed += ctx => TryReload();
        inputActions.Player.Reload.canceled  -= ctx => TryReload();
    }

    void Update()
    {
        if (permitedAct)
        {
            MoveCharacter();
            RotateCharacter();
            RotateCamera();
        }

        if (rate > 0f)
        {
            rate -= Time.deltaTime;
        }
    }

    // -------------------- Movement -------------------- //

    void MoveCharacter()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 direction = transform.parent.forward * moveInput.y
                              + transform.parent.right   * moveInput.x;

            agent.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }

        AnimatorController.instance.SetWalking(moveInput != Vector2.zero ? 1f : 0f);
    }

    // -------------------- Rotation -------------------- //

    void RotateCharacter()
    {
        if (lookInput.x != 0f)
        {
            float delta = lookInput.x * mouseSensitivity * Time.deltaTime;
            transform.parent.Rotate(0f, delta, 0f);
        }

        if (enableVerticalAim)
        {
            float delta = lookInput.y * mouseSensitivity * Time.deltaTime;
            verticalRot -= delta;
            verticalRot = Mathf.Clamp(verticalRot, -15f, 15f);
            transform.parent.localEulerAngles = new Vector3(verticalRot, transform.parent.localEulerAngles.y, 0f);
        }
    }

    void RotateCamera()
    {
        if (lookInput.x != 0f)
        {
            float delta = lookInput.x * mouseSensitivity * Time.deltaTime;

            if (Mathf.Abs(delta) > inertiaThreshold)
            {
                currentInertiaAngle = Mathf.Clamp(-delta * 2f, -inertiaMaxAngle, inertiaMaxAngle);
            }
            else
            {
                currentInertiaAngle = 0f;
            }

            playerCamera.localEulerAngles = new Vector3(
                playerCamera.localEulerAngles.x,
                currentInertiaAngle,
                playerCamera.localEulerAngles.z
            );
        }
        else
        {
            currentInertiaAngle = 0f;
            playerCamera.localEulerAngles = new Vector3(
                playerCamera.localEulerAngles.x,
                0f,
                playerCamera.localEulerAngles.z
            );
        }
    }

    // -------------------- Combat -------------------- //

    void TryShoot()
    {
        if (permitedAct && rate <= 0f && playerInfo.CanShoot() && !weapon.IsShooting())
        {
            weapon.Shoot();
            rate = weapon.fireRate;
        }
    }

    void TryReload()
    {
        playerInfo.TryReload();
    }

    // -------------------- Public Methods -------------------- //

    public void SetPermitedAct(bool value)
    {
        permitedAct = value;
    }
}
