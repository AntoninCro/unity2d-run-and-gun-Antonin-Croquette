using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private PlayerControllers player;

    void Start()
    {
        // Récupère le composant Slider attaché à cet objet
        healthSlider = GetComponent<Slider>();

        // Trouve le joueur dans la scène
        player = FindObjectOfType<PlayerControllers>();

        if (healthSlider != null && player != null)
        {
            // Configure le slider avec la santé max du joueur
            healthSlider.maxValue = player.GetMaxHealth();
            healthSlider.value = player.GetHealth();
        }
        else
        {
            Debug.LogError("HealthBar: Slider ou Player non trouvé!");
        }
    }

    void Update()
    {
        // Met à jour le slider avec la santé actuelle du joueur
        if (healthSlider != null && player != null)
        {
            healthSlider.value = player.GetHealth();
        }
    }
}
