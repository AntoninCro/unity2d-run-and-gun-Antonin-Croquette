using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public EnemyProjectil projectilePrefab;
    public int maxHealth = 3;
    public float detectionRange = 5f; // Distance à laquelle l'ennemi détecte le joueur
    public float shootCooldown = 2f; // Délai entre chaque tir
    public float projectileSpeed = 3f;

    private int health;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private PlayerControllers player;
    private float shootTimer = 0f;

    void Start()
    {
        // Trouve le joueur dans la scène
        player = FindObjectOfType<PlayerControllers>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Décrémente le timer de tir
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }

        // Vérifie si le joueur existe
        if (player != null)
        {
            // Calcule la distance avec le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // Si le joueur est dans la range de détection, tire
            if (distanceToPlayer <= detectionRange && shootTimer <= 0)
            {
                Shoot();
                shootTimer = shootCooldown; // Réinitialise le cooldown
            }
        }
    }

    private void Shoot()
    {
        // Calcule la direction vers le joueur
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Instancie le projectile à la position de l'ennemi avec décalage pour éviter de se toucher
        Vector3 spawnPosition = transform.position + (Vector3)directionToPlayer * 0.5f;
        EnemyProjectil projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Configure la vitesse du projectile via le script EnemyProjectil
        projectile.speed = projectileSpeed;

        // Oriente le projectile dans la direction du tir
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Ennemi volant tire vers le joueur!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet qui touche a le tag "PlayerProjectil"
        if (collision.CompareTag("PlayerProjectil"))
        {
            health--;

            // Assombrir l'ennemi : réduit la luminosité à chaque tir
            float darknessFactor = 1f - (float)(maxHealth - health) / maxHealth * 0.1f;
            spriteRenderer.color = originalColor * darknessFactor;

            Debug.Log("Ennemi touché! Santé restante: " + health);

            // Si la santé atteint 0, détruire l'ennemi
            if (health <= 0)
            {
                Debug.Log("Ennemi détruit!");
                Destroy(gameObject);
            }
        }
    }
}
