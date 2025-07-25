using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AmmoSystem : MonoBehaviour
{
    [Header("Основные настройки")]
    public TMP_Text ammoText;
    public int currentAmmo = 30;
    public int maxAmmo = 120;
    public int ammoPerReload = 30;

    [Header("Визуальные эффекты")]
    public Image reloadProgressBar;
    public Color normalAmmoColor = Color.white;
    public Color lowAmmoColor = Color.red;
    public float lowAmmoThreshold = 5;
    public float shakeIntensity = 10f;

    [Header("Анимации")]
    public float ammoTextScaleMultiplier = 1.2f;
    public float animationDuration = 0.1f;

    [Header("Ссылка на прицел")]
    public CrosshairController crosshairController; // Добавленная ссылка

    private bool isReloading = false;

    void Start()
    {
        if (reloadProgressBar != null)
            reloadProgressBar.fillAmount = 0;
            
        UpdateAmmoUI();
    }

    public void OnShoot()
    {
        if (isReloading) return;

        if (currentAmmo > 0)
        {
            currentAmmo--;
            PlayShootEffects();
            
            // Вызов анимации прицела при стрельбе
            if (crosshairController != null)
                crosshairController.OnShoot();
        }
        else
        {
            HandleEmptyMagazine();
        }

        UpdateAmmoUI();
    }

    public void Reload()
    {
        if (isReloading || currentAmmo >= ammoPerReload || maxAmmo <= 0) return;

        StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        
        if (reloadProgressBar != null)
        {
            reloadProgressBar.DOFillAmount(1, 3f).From(0).SetEase(Ease.Linear);
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }

        int neededAmmo = ammoPerReload - currentAmmo;
        int availableAmmo = Mathf.Min(neededAmmo, maxAmmo);
        
        currentAmmo += availableAmmo;
        maxAmmo -= availableAmmo;

        if (reloadProgressBar != null)
            reloadProgressBar.fillAmount = 0;

        isReloading = false;
        UpdateAmmoUI();
    }

    private void PlayShootEffects()
    {
        ammoText.transform.DOScale(ammoTextScaleMultiplier, animationDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => ammoText.transform.DOScale(1, animationDuration));
    }

    private void HandleEmptyMagazine()
    {
        ammoText.transform.DOShakePosition(0.5f, shakeIntensity);
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
        ammoText.color = (currentAmmo <= lowAmmoThreshold) ? lowAmmoColor : normalAmmoColor;
    }

    public void AddAmmo(int amount)
    {
        maxAmmo = Mathf.Min(maxAmmo + amount, 999);
        UpdateAmmoUI();
    }
}