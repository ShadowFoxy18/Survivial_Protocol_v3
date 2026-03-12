using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyBehavior enemy;

    int currentWaypointIndex = -1;
    int lastWaypointIndex = -1;
    float waitTimer = 0f;
    bool isWaiting = false;

    // -------------------- Constructor -------------------- //

    public PatrolState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    // -------------------- IEnemyState -------------------- //

    public void Enter()
    {
        GoToNextWaypoint();
    }

    public void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextWaypoint();
            }

            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.waypointReachDistance)
        {
            isWaiting = true;
            waitTimer = enemy.waypointWaitTime;
        }
    }

    public void Exit()
    {
        // Nothing to clean up
    }

    // -------------------- Waypoints -------------------- //

    void GoToNextWaypoint()
    {
        if (enemy.waypoints.Length == 0)
        {
            return;
        }

        int nextIndex = GetClosestWaypointIndex();
        lastWaypointIndex = currentWaypointIndex;
        currentWaypointIndex = nextIndex;

        enemy.agent.SetDestination(enemy.waypoints[currentWaypointIndex].position);
    }

    int GetClosestWaypointIndex()
    {
        int bestIndex = -1;
        float bestDistance = Mathf.Infinity;

        for (int i = 0; i < enemy.waypoints.Length; i++)
        {
            if (i == currentWaypointIndex || i == lastWaypointIndex)
            {
                continue;
            }

            float distance = Vector3.Distance(enemy.transform.position, enemy.waypoints[i].position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestIndex = i;
            }
        }

        // Fallback: solo 2 waypoints
        if (bestIndex == -1)
        {
            bestDistance = Mathf.Infinity;

            for (int i = 0; i < enemy.waypoints.Length; i++)
            {
                if (i == currentWaypointIndex)
                {
                    continue;
                }

                float distance = Vector3.Distance(enemy.transform.position, enemy.waypoints[i].position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }
        }

        return bestIndex;
    }
}
