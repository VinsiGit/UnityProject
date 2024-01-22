using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerManager playerManager; // Reference to the PlayerManager

    public int damageAmount = 10;
    public AudioClip damageAudio;
    private float damageTimer = 0; // Timer for damage
    private bool isInvincible = false; // Invincibility status

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        isInvincible = false;
    }

    void Update()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime; // decrease timer by the time since last frame
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    void TakeDamage()
    {
        playerManager.Damage(damageAmount); // decrease health by 10
        isInvincible = true;
        if (damageAudio != null)
        {
            AudioSource.PlayClipAtPoint(damageAudio, transform.position);
        }
        damageTimer = 2; // set invincibility duration
    }
    void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.CompareTag("Enemy") && !isInvincible)

                TakeDamage();
        }
    }


}