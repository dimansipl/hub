using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Основные настройки")]
    public Slider healthSlider;
    public TMP_Text healthText;
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Эффект трещины")]
    public Image crackEffect;
    public float crackDuration = 0.5f;
    private float crackTimer;
    private bool isCrackVisible;

    [Header("Экран смерти")]
    public Image blackScreen;
    public float fadeDuration = 2f;
    public TMP_Text gameOverText; 
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();    
        crackEffect.color = new Color(1, 0, 0, 0);
        blackScreen.color = new Color(0, 0, 0, 0);
    }
    void Update()
    {
        if (isCrackVisible)
        {
            crackTimer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(crackTimer / crackDuration) * 0.8f;
            crackEffect.color = new Color(1, 0, 0, alpha);

            if (crackTimer <= 0f) 
                isCrackVisible = false;
        }

        if (currentHealth <= 0 && blackScreen.color.a < 1)
        {
            float newAlpha = blackScreen.color.a + (Time.deltaTime / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, Mathf.Clamp01(newAlpha));

            if (blackScreen.color.a >= 0.95f)
            {
                if (gameOverText != null) gameOverText.gameObject.SetActive(true);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            crackTimer = crackDuration;
            crackEffect.color = new Color(1, 0, 0, 0.8f);
            isCrackVisible = true;
        }
    }
    void Die()
    {
        Debug.Log("Игрок умер!");
    }
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth / maxHealth;
        healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{maxHealth}";
    }
}