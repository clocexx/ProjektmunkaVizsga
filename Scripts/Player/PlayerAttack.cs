using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireprojectiles;
    [SerializeField] private AudioClip projectileSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(projectileSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        int projectileIndex = FindFireProjectiles(); 

        fireprojectiles[projectileIndex].transform.position = firePoint.position;
        fireprojectiles[projectileIndex].SetActive(true); 
        fireprojectiles[projectileIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireProjectiles()
    {
        for (int i = 0; i < fireprojectiles.Length; i++)
        {
            if (!fireprojectiles[i].activeInHierarchy)
                return i;
        }
        return 0; 
    }
}
