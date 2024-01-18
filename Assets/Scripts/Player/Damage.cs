using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerManager playerManager; // Reference to the PlayerManager

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
        playerManager.Damage(10); // decrease health by 10
        isInvincible = true;

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