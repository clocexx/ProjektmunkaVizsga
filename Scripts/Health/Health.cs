using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private PlayerRespawn playerRespawn;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        playerRespawn = GetComponent<PlayerRespawn>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetTrigger("die");
                anim.SetBool("Dead", true);

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            currentHealth = 0;      
            dead = true;            
            Respawn();              
        }
    }


    public void Respawn()
    {
        if (!dead)
            return;

        dead = false;
        currentHealth = startingHealth;

        anim.ResetTrigger("die");
        anim.SetTrigger("respawn");
        anim.Play("idle");

        StartCoroutine(Invunerability());

        foreach (Behaviour component in components)
            component.enabled = true;

        playerRespawn.CheckRespawn();
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
