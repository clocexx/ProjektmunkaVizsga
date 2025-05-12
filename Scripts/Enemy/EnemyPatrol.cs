using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private bool isDead = false;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (isDead) return;

        float nextX = enemy.position.x + Time.deltaTime * speed * (movingLeft ? -1 : 1);

        if (movingLeft && nextX >= leftEdge.position.x)
        {
            MoveInDirection(-1);
        }
        else if (!movingLeft && nextX <= rightEdge.position.x)
        {
            MoveInDirection(1);
        }
        else
        {
            DirectionChange();
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(-Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("moving", false);
        anim.SetTrigger("die");

        // Optional: destroy after animation ends
        Destroy(gameObject, 2f);
    }
}
