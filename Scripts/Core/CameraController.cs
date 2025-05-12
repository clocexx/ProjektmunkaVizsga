using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //J�t�kos k�vet�se
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance; // Eddig is volt, ezt haszn�ljuk.
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // J�t�kos k�vet�se a bal oldalra pozicion�lva
        transform.position = new Vector3(player.position.x - aheadDistance, transform.position.y, transform.position.z);

        // N�zet el�re v�ltoztat�sa (ahogy a j�t�kos mozog)
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
