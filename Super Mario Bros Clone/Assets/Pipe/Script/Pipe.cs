using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public bool activePipe;
    public PipeType pipeType;

    public Transform secretZoneEntranceTransform;
    public Transform secretZoneCameraTransform;

    public GameObject exitPipeGameObject;

    public enum PipeType
    {
        EntrancePipeExitSecretZone = 0,
        EntrancePipeExitPipe = 1,
        ExitPipe = 2
    }

    private GameObject player;

    private Vector2 from, to;

    private bool marioOnPipe = false;
    private bool circleColliderEnabled = true;

    private PlayerController playerController;

    private readonly float horizontalPipeY = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        if (!activePipe)
        {
            this.enabled = false;
            return;
        }
       // SetPipeColliders();
    }
    private void Update()
    {
        if (marioOnPipe && pipeType == PipeType.EntrancePipeExitSecretZone)
        {
           if (Input.GetKeyDown(KeyCode.DownArrow) && playerController.IsPlayerStop())
            {
                SetPositionsAndStartAnimate();
            }
        }
    }
    private void SetPipeColliders()
    {
        switch (pipeType)
        {
            case PipeType.EntrancePipeExitSecretZone:
            case PipeType.EntrancePipeExitPipe:
                GetComponent<CircleCollider2D>().enabled = true;
                circleColliderEnabled = true;
                break;
            case PipeType.ExitPipe:
                GetComponent<CircleCollider2D>().enabled = false;
                circleColliderEnabled = false;
                break;
        }
    }
    private void SetPositionsAndStartAnimate()
    {
        AudioManager.instance.Pipe();
        switch (pipeType)
        {
            case PipeType.EntrancePipeExitSecretZone:
                from = player.transform.position;
                to = transform.position;
                break;
            case PipeType.EntrancePipeExitPipe:
                from = player.transform.position;
                to = new Vector2(transform.position.x, player.transform.position.y + horizontalPipeY);
                break;
            default:
                //Nothing...
                break;
        }
        StartCoroutine(Animate());
    }
    private void AnimationOver()
    {
        switch (pipeType)
        {
            case PipeType.EntrancePipeExitSecretZone:
                Camera.main.transform.position = secretZoneCameraTransform.position;
                Camera.main.gameObject.GetComponent<MainCamera>().canMove = false;
                player.transform.position = secretZoneEntranceTransform.position;
                playerController.IsControllerActive = true;
                AudioManager.instance.PlayUnderGroundMusic();
                break;
            case PipeType.EntrancePipeExitPipe:
                exitPipeGameObject.GetComponent<Pipe>().ExitPipe(player);
                break;
            case PipeType.ExitPipe:
                Camera.main.gameObject.GetComponent<MainCamera>().canMove = true;
                playerController.IsControllerActive = true;
                GetComponent<BoxCollider2D>().enabled = true;
                this.enabled = false;
                AudioManager.instance.PlayLevelMusic();
                break;
        }
    }
    private IEnumerator Animate()
    {
        playerController.IsControllerActive = false;
        playerController.ResetPlayerVelocity();

        GetComponent<BoxCollider2D>().enabled = false;

        float ellapsed = 0f;
        float duration = 1f;

        while (ellapsed < duration)
        {
            float t = ellapsed / duration;
            player.transform.position = Vector2.Lerp(from,to,t);

            ellapsed += Time.deltaTime;

            yield return null;
        }
        player.transform.position = to;

        AnimationOver();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            player = other.gameObject;
            marioOnPipe = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            marioOnPipe = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (pipeType == PipeType.EntrancePipeExitPipe && other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            SetPositionsAndStartAnimate();
        }
    }

    public void ExitPipe(GameObject player)
    {
        this.player = player;
        playerController = player.GetComponent<PlayerController>();
        player.transform.position = transform.position;
        Camera.main.GetComponent<MainCamera>().SetCameraPositionOneFrame();

        from = transform.position;
        to = transform.Find("Pipe Exit Transform").position;

        StartCoroutine(Animate());
    }
    private float GetRotationValue() => transform.eulerAngles.z;
}
