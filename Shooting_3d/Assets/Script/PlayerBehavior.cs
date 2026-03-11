using UnityEngine;
using UnityEngine.AI;

public class PlayerBehavior : MonoBehaviour
{
    // ------- Values ------- //
    Vector2 moveInput;
    Vector2 lookInput;
    bool permitedAct = true;
    bool isSprinting;

    [Header("Mouse Sensibility & Move speed")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float moveLerpSpeed = 5f;
    [SerializeField] float mouseSensitivity = 100f;

    [Header("Camera")]
    [SerializeField] Transform playerCamera;
    [SerializeField] float verticalLimit = 45f;
    [SerializeField] float cameraDelay = 5f;

    [Header("Vertical Aim")]
    [SerializeField] bool enableVerticalAim = false;

    [Header("Horizontal Rotation")]
    [SerializeField] float rotationLerpSpeed = 10f;

    [Header("Camera Inertia")]
    [SerializeField] float inertiaThreshold = 5f;
    [SerializeField] float inertiaMaxAngle = 15f;

    [Header("Weapon")]
    [SerializeField] WeaponController weapon;

    float verticalRot = 0f;
    float currentInertiaAngle = 0f;

    // -- Movement -- //
    Vector3 currentVelocity = Vector3.zero;
    float targetRotY = 0f;

    // -- Components -- //
    Rigidbody rb;
    NavMeshAgent agent;
    Camera cam;

    // --------------- Input Actions ----------------- //
    private GameplayBehavior inputActions;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    // ----------------------------------------------- //

    void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
        cam = playerCamera.GetComponent<Camera>();
        agent.speed = moveSpeed;
        rb.isKinematic = true;

        targetRotY = transform.parent.eulerAngles.y;

        inputActions = new GameplayBehavior();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        inputActions.Player.Lock.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Lock.canceled  += ctx => lookInput = Vector2.zero;

        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled  += ctx => isSprinting = false;

        inputActions.Player.Shoot.performed += ctx => TryShoot();
        inputActions.Player.Shoot.canceled  -= ctx => TryShoot();

        inputActions.Player.Reload.performed += ctx => TryReload();
        inputActions.Player.Reload.canceled  -= ctx => TryReload();

        // Block/unblock movement during reload
        weapon.OnReloadStart += () => permitedAct = false;
        weapon.OnReloadEnd   += () => permitedAct = true;
    }

    void Update()
    {
        if (permitedAct)
        {
            MoveCharacter();
            Sprint();
            RotateCharacter();
            RotateCamera();
        }
    }

    void MoveCharacter()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 direction = transform.parent.forward * moveInput.y
                              + transform.parent.right   * moveInput.x;

            currentVelocity = Vector3.Lerp(currentVelocity, direction.normalized * moveSpeed, moveLerpSpeed * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.zero;
        }

        agent.velocity = currentVelocity;

        // Update movement animations
        AnimatorController.instance.SetWalking(currentVelocity.magnitude > 1f ? 1f : 0f);
    }

    void Sprint()
    {
        if (isSprinting && moveInput.y > 0f)
        {
            Vector3 direction = transform.parent.forward;
            currentVelocity = Vector3.Lerp(currentVelocity, direction * moveSpeed * speedMultiplier, moveLerpSpeed * Time.deltaTime);
            agent.velocity = currentVelocity;

            AnimatorController.instance.SetSprint(true);
        }
        else
        {
            AnimatorController.instance.SetSprint(false);
        }
    }

    void RotateCharacter()
    {
        if (lookInput.x != 0f)
        {
            float delta = lookInput.x * mouseSensitivity * Time.deltaTime;

            targetRotY += delta;
            float currentY = transform.parent.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentY, targetRotY, rotationLerpSpeed * Time.deltaTime);
            transform.parent.eulerAngles = new Vector3(0f, newY, 0f);

            if (Mathf.Abs(delta) > inertiaThreshold)
            {
                currentInertiaAngle = Mathf.Clamp(-delta * 2f, -inertiaMaxAngle, inertiaMaxAngle);
            }
            else
            {
                currentInertiaAngle = 0f;
            }

            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                currentInertiaAngle,
                transform.localEulerAngles.z
            );
        }
    }

    void RotateCamera()
    {
        if (enableVerticalAim)
        {
            float deltaY = lookInput.y * mouseSensitivity * Time.deltaTime;
            verticalRot -= deltaY;
            verticalRot = Mathf.Clamp(verticalRot, -verticalLimit, verticalLimit);

            playerCamera.localEulerAngles = new Vector3(verticalRot, 0f, 0f);
        }
    }

    void TryShoot()
    {
        if (permitedAct && !isSprinting)
        {
            weapon.TryShoot();
        }
    }

    void TryReload()
    {
        if (!isSprinting)
        {
            weapon.TryReload();
        }
    }

    public void SetPermitedAct(bool value)
    {
        permitedAct = value;
    }
}
