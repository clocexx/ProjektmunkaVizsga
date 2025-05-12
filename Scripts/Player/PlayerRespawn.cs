using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        if (currentCheckpoint == null)
        {

            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); // életerõ & animáció visszaállítás
        transform.position = currentCheckpoint.position;

        // Kamera mozgatása, ha kell
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;

            Animator checkpointAnim = collision.GetComponent<Animator>();
            if (checkpointAnim != null)
            {
                checkpointAnim.SetTrigger("appear");
            }
        }
    }
}
