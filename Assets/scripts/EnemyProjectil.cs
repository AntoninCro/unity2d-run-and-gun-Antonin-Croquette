using UnityEngine;

public class EnemyProjectil : MonoBehaviour
{
    public float speed = 3f; // Vitesse du projectile
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        // Récupère le Rigidbody2D et le Collider2D
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Configure le projectile comme trigger pour détecter les collisions
        if (col != null)
        {
            col.isTrigger = true;
        }

        // Configure le Rigidbody2D en kinematic (pas de physique)
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        // Déplace le projectile en fonction de sa direction locale
        transform.position += transform.right * speed * Time.deltaTime;
    }

    // Détecte les collisions avec le joueur
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le projectile touche le joueur
        if (collision.CompareTag("Player"))
        {
            // Obtient le composant PlayerControllers pour infliger des dégâts
            PlayerControllers player = collision.GetComponent<PlayerControllers>();
            if (player != null)
            {
                Debug.Log("Projectile ennemi touche le joueur! Dégâts infligés.");
                player.TakeDamage(1); // Inflige 1 dégât au joueur
                Destroy(gameObject); // Détruit le projectile
            }
        }
        // Détruit le projectile s'il touche autre chose (murs, sol, etc.)
        else if (!collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
