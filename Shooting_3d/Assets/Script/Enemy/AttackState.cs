using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyBehavior enemy;

    // -------------------- Constructor -------------------- //

    public AttackState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    // -------------------- IEnemyState -------------------- //

    public void Enter()
    {
        enemy.agent.ResetPath();
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.playerTransform.position);

        if (distanceToPlayer > enemy.attackDistance)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        // Face the player
        Vector3 direction = enemy.playerTransform.position - enemy.transform.position;
        direction.y = 0f;
        enemy.transform.rotation = Quaternion.LookRotation(direction);

        // Shoot
        enemy.ShootAtPlayer();
    }

    public void Exit()
    {
        // Nothing to clean up
    }
}
