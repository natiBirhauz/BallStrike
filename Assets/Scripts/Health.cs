using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Start()
    {
        if (healthText == null)
            healthText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHealthDisplay(int health)
    {
        healthText.SetText("Health: " + health);
    }
}
