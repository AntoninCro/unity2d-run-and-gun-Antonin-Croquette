using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllers : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxHealth = 3;
    [SerializeField] private Animator animator;
    public ProjectilBehavior ProjectilPrefab;
    public Transform LaunchOffser;
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private int groundContactCount = 0;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private float shootingCooldown = 0f;
    private float shootingAnimationDuration = 0.3f;
    public int health;
    private int damageContactCount = 0;
    private float invincibilityCooldown = 0f;
    private float invincibilityDuration = 0.5f;

    void Start()
    {
        // Récupère le composant Rigidbody2D attaché au joueur
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        CheckGrounded();
        HandleMovement();
        FlipCharacter();
        Shoot();

        // Réinitialiser isShooting après le délai d'animation
        if (shootingCooldown > 0)
        {
            shootingCooldown -= Time.deltaTime;
            if (shootingCooldown <= 0)
            {
                animator.SetBool("isShooting", false);
            }
        }

        // Décrémenter le timer d'invincibilité
        if (invincibilityCooldown > 0)
        {
            invincibilityCooldown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Applique le mouvement au Rigidbody2D
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void FlipCharacter()
    {
        // Vérifie la direction du mouvement et retourne le personnage si nécessaire
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Face à droite
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); // Face à gauche
        }
    }

    private void CheckGrounded()
    {
        // Met à jour isGrounded en fonction du nombre de contacts au sol
        isGrounded = groundContactCount > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Incrementer les contacts au sol
        groundContactCount++;
        // Réinitialiser les sauts disponibles quand on retouche le sol
        jumpCount = 0;
        // Animation de saut terminée
        animator.SetBool("isJumping", false);

        // Vérifie si l'objet qui touche a le tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy") && invincibilityCooldown <= 0)
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Décrémenter les contacts au sol
        groundContactCount--;
        groundContactCount = Mathf.Max(0, groundContactCount);
    }

    private void HandleMovement()
    {
        // Récupère l'entrée du joueur pour le mouvement horizontal (gauche/droite)
        moveInput = Input.GetAxis("Horizontal");

        // Vérifie si le joueur peut sauter (au sol ou en l'air avec sauts restants)
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }
        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Instancie le projectile à la position de lancement et oriente vers la droite
            ProjectilBehavior projectile = Instantiate(ProjectilPrefab, LaunchOffser.position, LaunchOffser.rotation);
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            animator.SetBool("isShooting", true);
            shootingCooldown = shootingAnimationDuration;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Instancie le projectile à la position de lancement et oriente vers la gauche
            ProjectilBehavior projectile = Instantiate(ProjectilPrefab, LaunchOffser.position, LaunchOffser.rotation);
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            animator.SetBool("isShooting", true);
            shootingCooldown = shootingAnimationDuration;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Instancie le projectile à la position de lancement et oriente vers le haut
            ProjectilBehavior projectile = Instantiate(ProjectilPrefab, LaunchOffser.position, LaunchOffser.rotation);
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            animator.SetBool("isShooting", true);
            shootingCooldown = shootingAnimationDuration;
        }
    }

    // Inflige des dégâts au joueur
    public void TakeDamage(int damage)
    {
        health -= damage;
        damageContactCount++;
        invincibilityCooldown = invincibilityDuration; // Activer l'invincibilité
        Debug.Log("Joueur touché! Santé restante: " + health);

        // Si la santé atteint 0, détruire le joueur
        if (health <= 0)
        {
            Debug.Log("Joueur mort!");
            GameManager.Instance.GameOver();
            transform.DetachChildren(); // évite de détruire la caméra, enfant du joueur
            Destroy(gameObject);
        }
    }

    // Getter pour accéder à la santé du joueur (pour la barre de vie)
    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
