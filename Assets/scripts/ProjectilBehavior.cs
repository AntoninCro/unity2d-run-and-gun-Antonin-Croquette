using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectilBehavior : MonoBehaviour
{
    public float Speed = 1.0f;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Update()
    {
        transform.position += transform.right * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
