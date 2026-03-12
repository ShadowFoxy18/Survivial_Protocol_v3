using UnityEngine;

public class GameManager : MonoBehaviour
{
    // -------------------- Singleton -------------------- //

    public static GameManager instance;

    // -------------------- Game Settings -------------------- //

    [Header("UI")]
    [SerializeField] GameObject victoryCanvas;

    int enemiesAlive = 0;

    // -------------------- Unity Methods -------------------- //

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

        victoryCanvas.SetActive(false);
    }

    // -------------------- Enemy Registry -------------------- //

    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            Victory();
        }
    }

    // -------------------- Victory -------------------- //

    void Victory()
    {
        victoryCanvas.SetActive(true);
    }
}