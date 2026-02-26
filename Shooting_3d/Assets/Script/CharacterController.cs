using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
        // ------- Values ------- //
    // -- Move set Character ----
    Vector2 moveCharacter;
    Vector2 rotCharacter;
    // ---

    [SerializeField]
    float characterVelocity = 10, incrementVelocity = 1.5f;
    
    [SerializeField]
    bool permitedAct = true;

    // Call GameObject inspector
    Rigidbody rb;
    CharacterController controler;
    Animator animator;


    // --------------- InputsActions ----------------- //
    private GameplayBehavior inputActions;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable(); 
    }
    // ---------------------------------------------- //
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody>();
        controler = transform.parent.GetComponent<CharacterController>();

        // INPUTS ACTIONS
        inputActions.Player.Move.performed += context => moveCharacter = context.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += context => moveCharacter = Vector2.zero;

        inputActions.Player.Lock.performed += context => rotCharacter = context.ReadValue<Vector2>();
        inputActions.Player.Lock.canceled += context => rotCharacter = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (permitedAct)
        {
            MoveCharacter();
            RotCharacter();
        }  
    }

    void MoveCharacter()
    {
        //controler.Move
    }

    void RotCharacter()
    {

    }
}
