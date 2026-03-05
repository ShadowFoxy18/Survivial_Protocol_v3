using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    // ------- Values ------- //
    Vector2 moveCharacter;
    Vector2 rotCharacter;
    bool permitedAct = true;
    bool isSprinting;

    [SerializeField]
    float characterVelocity = 10, incrementVelocity = 1.5f;

    [SerializeField]
    float mouseSensitivity = 100f;

    // -- Camera -- //
    [SerializeField]
    Transform camara;

    [SerializeField]
    float limiteVertical = 45f;
    [SerializeField]
    float limiteSprintLateral = 75f;
    [SerializeField]
    float cameraDelay = 5f;

    float rotVertical = 0f;
    float rotLateral = 0f;

    // Call GameObject inspector
    Rigidbody rb;
    NavMeshAgent ag;
    Animator animator;

    // --------------- InputsActions ----------------- //
    private GameplayBehavior inputActions;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    // ---------------------------------------------- //

    void Awake()
    {

        animator = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody>();
        ag = transform.parent.GetComponent<NavMeshAgent>();
        ag.speed = characterVelocity;

        inputActions = new GameplayBehavior();

        inputActions.Player.Move.performed += ctx => moveCharacter = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveCharacter = Vector2.zero;

        inputActions.Player.Lock.performed += ctx => rotCharacter = ctx.ReadValue<Vector2>();
        inputActions.Player.Lock.canceled += ctx => rotCharacter = Vector2.zero;

        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    void Update()
    {
        if (permitedAct)
        {
            MoveCharacter();
            RotCharacter();
            RotCamera();
        }
    }

    void MoveCharacter()
    {
        if (moveCharacter != Vector2.zero)
        {
            // Movemos el padre directamente por código
            Vector3 direccion = transform.parent.forward * moveCharacter.y
                              + transform.parent.right * moveCharacter.x;

            transform.parent.position += direccion * characterVelocity * Time.deltaTime;
        }
    }

    void RotCharacter()
    {

    }

    void RotCamera()
    {
        
    }
}