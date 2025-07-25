using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CrosshairController : MonoBehaviour
{
    [Header("Настройки прицела")]
    public Image crosshairImage;
    public float jumpHeight = 10f;   
    public float jumpDuration = 0.1f; 
    public float returnDuration = 0.3f; 

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = crosshairImage.rectTransform.localPosition;
    }

    public void OnShoot()
    {
        crosshairImage.rectTransform.DOKill();
        
        crosshairImage.rectTransform.DOLocalMoveY(
            originalPosition.y + jumpHeight, 
            jumpDuration
        ).SetEase(Ease.OutQuad)
         .OnComplete(() => {

             crosshairImage.rectTransform.DOLocalMoveY(
                 originalPosition.y, 
                 returnDuration
             ).SetEase(Ease.OutElastic);
         });
    }
}