using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public static AnimatorController instance;

    // -- Components -- //
    Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
    }

    // -- Movement -- //
    public void SetWalking(float value)
    {
        animator.SetFloat("Walking", value);
    }

    public void SetSprint(bool value)
    {
        animator.SetBool("Sprint", value);
    }

    // Position: -1 = left, 0 = center, 1 = right
    public void SetPosition(float value)
    {
        animator.SetFloat("Position", value);
    }

    // -- Combat -- //
    public void SetShooting(bool value)
    {
        animator.SetBool("Shooting", value);
    }

    public void SetReload(bool value)
    {
        animator.SetBool("Reload", value);
    }

    // -- Death -- //
    public void SetDead(bool value)
    {
        animator.SetBool("Dead", value);
    }
}
