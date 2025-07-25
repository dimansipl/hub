using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PoseSettings
{
    public Sprite standIcon;
    public Sprite sitIcon;
    public Sprite lieIcon;
    [Tooltip("Максимальный размер иконки (ширина или высота)")]
    public float maxIconSize = 100f;
}
public class PoseIndicator : MonoBehaviour
{
    [Header("Основные настройки")]
    public Image poseImage;
    public PoseSettings poseSettings;

    [Header("Анимация")]
    [Tooltip("Длительность показа иконки в секундах")]
    public float showDuration = 1.5f;
    [Tooltip("Скорость плавного появления/исчезания")]
    public float fadeSpeed = 3f;
    private float _timer;
    private bool _isVisible;
    private readonly Color _visibleColor = new Color(1, 1, 1, 1);
    private readonly Color _hiddenColor = new Color(1, 1, 1, 0);
    void Start()
    {
        if (poseImage == null)
            poseImage = GetComponent<Image>();
        
        poseImage.color = _hiddenColor;
        poseImage.preserveAspect = true; 
    }

    void Update()
    {
        UpdateVisibility();
    }
    private void UpdateVisibility()
    {
        if (_isVisible)
        {
            _timer -= Time.deltaTime;
            
            if (_timer <= 0)
            {
                _isVisible = false;
            }
            else
            {
                poseImage.color = _visibleColor;
                return;
            }
        }
        if (poseImage.color.a > 0)
        {
            poseImage.color = Color.Lerp(poseImage.color, _hiddenColor, fadeSpeed * Time.deltaTime);
        }
    }
    private void SetPose(Sprite icon)
    {
        if (icon == null || poseImage == null) return;

        poseImage.sprite = icon;
        ResizeIcon(icon);
        
        poseImage.color = _visibleColor;
        _timer = showDuration;
        _isVisible = true;
    }
    private void ResizeIcon(Sprite icon)
    {
        if (icon == null) return;

        float width = icon.rect.width;
        float height = icon.rect.height;
        float ratio = width / height;
        if (ratio > 1) 
        {
            poseImage.rectTransform.sizeDelta = new Vector2(
                poseSettings.maxIconSize,
                poseSettings.maxIconSize / ratio
            );
        }
        else 
        {
            poseImage.rectTransform.sizeDelta = new Vector2(
                poseSettings.maxIconSize * ratio,
                poseSettings.maxIconSize
            );
        }
    }
    public void ShowStandingPose() => SetPose(poseSettings.standIcon);
    public void ShowSittingPose() => SetPose(poseSettings.sitIcon);
    public void ShowLyingPose() => SetPose(poseSettings.lieIcon);
    [ContextMenu("Test Standing Pose")]
    public void TestStanding()
    {
        SetPose(poseSettings.standIcon);
    }
}