using UnityEngine;

public class Enemymovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 3;
    private Rigidbody2D rb;
    private Collider2D col;
    private float direction = 1f;
    private int health;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (direction > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Détecte les obstacles devant l'ennemi avec OverlapBox
        Vector2 boxPosition = (Vector2)transform.position + (Vector2.right * direction) * 0.5f;
        Vector2 boxSize = new Vector2(0.8f, 1f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxPosition, boxSize, 0);

        // Cherche un collider qui n'est pas l'ennemi et n'est pas le joueur
        bool wallDetected = false;
        foreach (Collider2D hit in hits)
        {
            if (hit != col && !hit.CompareTag("Player") && !hit.CompareTag("PlayerProjectil"))
            {
                wallDetected = true;
                break;
            }
        }

        if (wallDetected)
        {
            direction *= -1;
        }
    }

    // Détecte quand le projectile touche l'ennemi
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
