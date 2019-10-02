using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{

    private float maxHealth = 100;
    private float currentHealth = 100;
    private float maxArmour = 100;
    private float currentArmour = 100;
    private float maxStamina = 100;
    private float currentStamina = 100;

    private float barWidth = 100;
    private float barHeight = 100;

    private float canHeal = 0.0f;
    private float canRegenerate = 0.0f;

    private CharacterController chCont;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsC;
    private Vector3 lastPosition;

    public Texture2D healthTexture;
    public Texture2D armourTexture;
    public Texture2D staminaTexture;

    public float walkSpeed = 10.0f;
    public float runSpeed = 20.0f;

    public GUITexture hitTexture;
   


    void Awake()
    {
        barHeight = Screen.height * 0.02f;
        barWidth = barHeight * 10.0f;

        chCont = GetComponent<CharacterController>();
        fpsC = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        lastPosition = transform.position;
    }

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

    void Start()
    {
        Rect currentRes = new Rect(-Screen.width * 0.5f,
                                   -Screen.height * 0.5f,
                                   Screen.width,
                                   Screen.height);
        hitTexture.pixelInset = currentRes;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            takeHit(30);
        }

        if (canHeal > 0.0f)
        {
            canHeal -= Time.deltaTime;
        }
        if (canRegenerate > 0.0f)
        {
            canRegenerate -= Time.deltaTime;
        }

        if (canHeal <= 0.0f && currentHealth < maxHealth)
        {
            regenerate(ref currentHealth, maxHealth);
        }
        if (canRegenerate <= 0.0f && currentStamina < maxStamina)
        {
            regenerate(ref currentStamina, maxStamina);
        }

    }

    void FixedUpdate()
    {
        float speed = walkSpeed;

        if (chCont.isGrounded && Input.GetKey(KeyCode.LeftShift) && lastPosition != transform.position && currentStamina > 0)
        {
            lastPosition = transform.position;
            speed = runSpeed;
            currentStamina -= 1;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            canRegenerate = 5.0f;
        }

        if (currentStamina > 0)
        {
            fpsC.CanRun = true;
        }
        else
        {
            fpsC.CanRun = false;
        }
    }

    void takeHit(float damage)
    {
        Destroy(Instantiate(hitTexture).gameObject, 0.15f);
        Destroy(Instantiate(hitTexture), 0.15f);

        if (currentArmour > 0)
        {
            currentArmour = currentArmour - damage;
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

        if (currentHealth < maxHealth)
        {
            canHeal = 5.0f;
        }

        currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void regenerate(ref float currentStat, float maxStat)
    {
        currentStat += maxStat * 0.005f;
        Mathf.Clamp(currentStat, 0, maxStat);
    }

}