using UnityEngine;
using UnityEngine.UI;

public class UIModalManager : MonoBehaviour
{
    public static UIModalManager Instance;
    public GameObject modalPrefab;
    public Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UIModal CreateModal(string title, string message, bool scrollable = false)
    {
        GameObject modalObject = Instantiate(modalPrefab, canvas.transform);
        UIModal modal = modalObject.GetComponent<UIModal>();
        modal.Initialize(title, message, scrollable);
        return modal;
    }
}

public class UIModal : MonoBehaviour
{
    public Text titleText;
    public Text messageText;
    public RectTransform modalTransform;
    public GameObject contentContainer;
    public ScrollRect scrollRect;
    public RectTransform contentTransform;
    public GameObject buttonPrefab;

    public void Initialize(string title, string message, bool scrollable)
    {
        titleText.text = title;
        messageText.text = message;
        scrollRect.gameObject.SetActive(scrollable);
        AdjustSize();
    }

    private void AdjustSize()
    {
        float width = Screen.width * 0.8f;
        float height = Screen.height * 0.7f;
        modalTransform.sizeDelta = new Vector2(width, height);
    }

    public void AddButton(string buttonText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObject = Instantiate(buttonPrefab, contentContainer.transform);
        Button button = buttonObject.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = buttonText;
        button.onClick.AddListener(onClick);
    }

    public void AddText(string textContent)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(contentContainer.transform, false);
        Text text = textObject.AddComponent<Text>();
        text.text = textContent;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
