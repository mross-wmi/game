using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth = 100;
    private float maxArmour = 100;
    private float currentArmour = 100;
    private float maxStamina = 100;
    private float currentStamina = 100;

    public Texture2D healthTexture;
    public Texture2D armourTexture;
    public Texture2D staminaTexture;

    private float barWidth;
    private float barHeight;

    // Funkcja przy powracaniu do życia
    void Awake()
    {
        barHeight = Screen.height * 0.02f;
        barWidth = barHeight * 10.0f;

    }

    // Funkcja wyswietlajaca elementy interfejsu
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                                 Screen.height - barHeight - 10,
                                 currentHealth * barWidth / maxHealth,
                                 barHeight),
                        healthTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                                 Screen.height - barHeight * 2 - 20,
                                 currentArmour * barWidth / maxArmour,
                                 barHeight),
                        armourTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                                 Screen.height - barHeight * 3 - 30,
                                 currentStamina * barWidth / maxStamina,
                                 barHeight),
                        staminaTexture);
    }
    // Funkcja odpowiedzialna za poniesione obrazenia
    void takeHit(float damage)
    {
        if (currentArmour > 0)
        {
            currentArmour -= damage;
            if (currentArmour < 0)
            {
                currentHealth += currentArmour;
                currentArmour = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            takeHit(30);
        }
    }

   
}
