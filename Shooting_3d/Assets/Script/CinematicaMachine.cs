using UnityEngine;
using UnityEngine.Playables;

public class CinematicaMachine : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] GameObject player;
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject scene;

    [Header("UIs")]
    [SerializeField] GameObject infoUI;

    float cinematicDuration;
    bool finished = false;

    void Awake()
    {
        player.SetActive(false);
        infoUI.SetActive(false);

        director.Play();

        cinematicDuration = (float)director.duration;
    }

    void Update()
    {
        if (finished)
        {
            return;
        }

        cinematicDuration -= Time.deltaTime;

        if (cinematicDuration < 0f)
        {
            finished = true;
            Destroy(scene);
            player.SetActive(true);
            infoUI.SetActive(true);
        }
    }
}
