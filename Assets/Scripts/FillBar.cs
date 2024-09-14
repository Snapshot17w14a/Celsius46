using UnityEngine;

public class FillBar : MonoBehaviour
{
    private enum Orientation { Horizontal, Vertical }

    [Range(0f, 1f)]
    [SerializeField] private float fillAmount = 1f;  // Fill amount of the bar [0, 1]
    public float FillAmount => fillAmount;

    private float width;
    private float height;
    private RectTransform fillObjectRectTransform;

    [SerializeField] private Orientation orientation = Orientation.Horizontal;

    private void Awake()
    {
        width = GetComponent<RectTransform>().sizeDelta.x;
        height = GetComponent<RectTransform>().sizeDelta.y;
        fillObjectRectTransform = transform.Find("Fill Bar").GetComponent<RectTransform>();
    }

    public void SetFill(float fill)
    {
        fillAmount = Mathf.Clamp(fill, 0f, 1f);
        if (orientation == Orientation.Horizontal) fillObjectRectTransform.sizeDelta = new Vector2(width * fillAmount, fillObjectRectTransform.sizeDelta.y);
        else fillObjectRectTransform.sizeDelta = new Vector2(fillObjectRectTransform.sizeDelta.x, height * fillAmount);
    }

    private void OnValidate()
    {
        Awake();
        SetFill(fillAmount);
    }
}