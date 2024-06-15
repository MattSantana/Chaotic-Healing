using UnityEngine;
using System.Collections;

public class Areas : MonoBehaviour
{
    [SerializeField] private Transform dungeonArea;
    [SerializeField] private Transform enemiesArea;
    [SerializeField] private Transform clinicAfterDungeonArea;
    [SerializeField] private Transform clinicAfterEnemiesArea;
    [SerializeField] private float playerCanMove = 2f;

    [HideInInspector] public bool inEnemyArea;
    [HideInInspector] public bool inDungeonArea;

    [SerializeField] private AudioClip dungeonSound;
    [SerializeField] private AudioClip enemiesSound;
    [SerializeField] private AudioClip clinic;

    private CameraFollow cameraFollow;
    private CameraFade cameraFade;
    private Movement playerMovement;

    private AudioSource audioSource;

    private void Awake()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>(); // Obtém a referência ao script CameraFollow anexado à câmera principal
        cameraFade = FindObjectOfType<CameraFade>(); // Obtém a referência ao script CameraFade

        playerMovement = GetComponent<Movement>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySound(clinic);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WithoutEnemies"))
        {
            StartCoroutine(ChangeArea(dungeonArea, true));
            inEnemyArea = true;
            PlaySound(dungeonSound);
            PlayerMove();
        }

        if (collision.CompareTag("WithEnemies"))
        {
            StartCoroutine(ChangeArea(enemiesArea, true));
            inDungeonArea = true;
            PlaySound(enemiesSound);
            PlayerMove();
        }

        if (collision.CompareTag("ClinicEnemies"))
        {
            StartCoroutine(ChangeArea(clinicAfterEnemiesArea, false));
            inEnemyArea = false;
            PlaySound(clinic);
            PlayerMove();
        }

        if (collision.CompareTag("ClinicDungeon"))
        {
            StartCoroutine(ChangeArea(clinicAfterDungeonArea, false));
            inDungeonArea = false;
            PlaySound(clinic);
            PlayerMove();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private IEnumerator ChangeArea(Transform targetArea, bool shouldFollow)
    {
        yield return StartCoroutine(cameraFade.FadeIn());

        transform.position = targetArea.position;

        if (shouldFollow)
        {
            cameraFollow.StartFollowing();
        }
        else
        {
            cameraFollow.StopFollowing();
        }

        yield return StartCoroutine(cameraFade.FadeOut());
    }

    private IEnumerator PlayerMove()
    {
        playerMovement.canMove = false;

        yield return new WaitForSeconds(playerCanMove);

        playerMovement.canMove = true;
    }
}