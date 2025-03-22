using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.VolumeComponent;
using System.Linq;

public class MarqueeFuckers : MonoBehaviour
{
    [SerializeField] private string[] thingsWeHate;
    public float scrollSpeed = 50;
    public TextMeshProUGUI messageRect;
    public TextMeshProUGUI messageRect2;

    string message;

    private void Start()
    {
        message = string.Concat(Enumerable.Repeat($"FUCK {thingsWeHate[Random.Range(0, thingsWeHate.Length)]}. ", 4));
        messageRect.text = messageRect2.text = message;

        messageRect2.rectTransform.anchoredPosition = new Vector2(messageRect.rectTransform.anchoredPosition.x - messageRect2.preferredWidth, messageRect2.rectTransform.anchoredPosition.y);
    }

    private void Update()
    {
        messageRect.rectTransform.anchoredPosition = messageRect.rectTransform.anchoredPosition + new Vector2(Time.deltaTime * scrollSpeed, 0);
        if (messageRect.rectTransform.anchoredPosition.x > Screen.width)
        {
            string thing = thingsWeHate[Random.Range(0, thingsWeHate.Length)];
            message = string.Concat(Enumerable.Repeat($"FUCK {thing}. ", (int)(30 / thing.Length)));
            messageRect.text = messageRect2.text = message;
            messageRect.rectTransform.anchoredPosition = new Vector2(messageRect2.rectTransform.anchoredPosition.x - messageRect.preferredWidth, messageRect.rectTransform.anchoredPosition.y);
        }
    }

    void LateUpdate()
    {        
        messageRect2.rectTransform.anchoredPosition = messageRect2.rectTransform.anchoredPosition + new Vector2(Time.deltaTime * scrollSpeed, 0);
        if (messageRect2.rectTransform.anchoredPosition.x > Screen.width)
        {
            messageRect2.rectTransform.anchoredPosition = new Vector2(messageRect.rectTransform.anchoredPosition.x - messageRect2.preferredWidth, messageRect2.rectTransform.anchoredPosition.y);
        }
    }
}
