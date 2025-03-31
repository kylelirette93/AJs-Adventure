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

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Increase the scale when the mouse enters the object.
        transform.DOScale(transform.localScale + scaleIncrease, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the scale when the mouse exits the object.
        transform.DOScale(transform.localScale - scaleIncrease, 0.3f);
    }
}