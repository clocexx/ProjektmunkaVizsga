using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Játékos követése
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance; // Eddig is volt, ezt használjuk.
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // Játékos követése a bal oldalra pozicionálva
        transform.position = new Vector3(player.position.x - aheadDistance, transform.position.y, transform.position.z);

        // Nézet elõre változtatása (ahogy a játékos mozog)
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
