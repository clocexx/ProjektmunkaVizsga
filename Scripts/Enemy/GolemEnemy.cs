using System.Collections;
using UnityEngine;

public class GolemEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 5f;
    [SerializeField] private int damage = 1;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;

    // References
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        if (isDead) return;

        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);

        int fireballIndex = FindAvailableFireball();
        if (fireballIndex >= 0)
        {
            fireballs[fireballIndex].transform.position = firepoint.position;
            fireballs[fireballIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
    }

    private int FindAvailableFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return -1; // no available fireball
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("die");

        if (enemyPatrol != null)
            enemyPatrol.enabled = false;

        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false; // prevent further logic

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // wait for death animation to finish
        Destroy(gameObject);
    }
}