using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyBehavior enemy;

    float losePlayerTimer = 0f;

    // -------------------- Constructor -------------------- //

    public ChaseState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    // -------------------- IEnemyState -------------------- //

    public void Enter()
    {
        enemy.agent.SetDestination(enemy.playerTransform.position);
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.playerTransform.position);

        if (distanceToPlayer <= enemy.attackDistance)
        {
            enemy.ChangeState(new AttackState(enemy));
            return;
        }

        if (distanceToPlayer > enemy.detectionRadius)
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer >= enemy.losePlayerTime)
            {
                enemy.ChangeState(new PatrolState(enemy));
            }
        }
        else
        {
            losePlayerTimer = 0f;
            enemy.agent.SetDestination(enemy.playerTransform.position);
        }
    }

    public void Exit()
    {
        losePlayerTimer = 0f;
    }
}
