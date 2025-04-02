using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 scaleIncrease = new Vector3(1.2f, 1.2f, 1.2f); 
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale; 
    }

    private void OnDisable()
    {
        transform.localScale = originalScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Increase the scale when the mouse enters the object.
        transform.DOScale(originalScale + scaleIncrease, 0.3f);
        GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.popSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the scale when the mouse exits the object.
        transform.DOScale(originalScale, 0.3f);
    }
}