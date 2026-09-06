using UnityEngine;

// A attacher à la porte de fin de niveau, avec un Collider2D configuré en "Is Trigger"
public class EndLevelDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Win();
        }
    }
}
