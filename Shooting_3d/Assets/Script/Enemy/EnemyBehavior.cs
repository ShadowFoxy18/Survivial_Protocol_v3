using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // -------------------- State Machine -------------------- //

    IEnemyState currentState;

    // -------------------- Patrol Settings -------------------- //

    [Header("Patrol Settings")]
    public Transform[] waypoints;
    public float waypointWaitTime = 2f;
    public float waypointReachDistance = 1.5f;

    // -------------------- Detection Settings -------------------- //

    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    public float losePlayerTime = 3f;
    public float attackDistance = 3f;
    public LayerMask obstacleMask;

    // -------------------- Attack Settings -------------------- //

    [Header("Attack Settings")]
    public float bulletForce = 500f;
    public Transform muzzlePoint;

    // -------------------- References -------------------- //

    [Header("References")]
    public Transform playerTransform;

    // -------------------- Components -------------------- //

    public NavMeshAgent agent;
    public EnemyHealth enemyHealth;

    // -------------------- Unity Methods -------------------- //

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        ChangeState(new PatrolState(this));
    }

    void Update()
    {
        currentState.Update();
    }

    // -------------------- State Control -------------------- //

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    // -------------------- Shared Methods -------------------- //

    public bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > detectionRadius)
        {
            return false;
        }

        Vector3 direction = playerTransform.position - transform.position;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, detectionRadius, obstacleMask))
        {
            return false;
        }

        return true;
    }

    public void ShootAtPlayer()
    {
        if (!enemyHealth.CanShoot())
        {
            return;
        }

        enemyHealth.OnShoot();

        GameObject bullet = BulletPool.instance.PopObject();
        bullet.transform.position = muzzlePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(playerTransform.position - muzzlePoint.position);
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(
            (playerTransform.position - muzzlePoint.position).normalized * bulletForce,
            ForceMode.Impulse
        );
    }
}
