using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class UIButtonManager
{
    public static GameObject CreateButton(
        Transform parent,
        string buttonText,
        UnityAction onClick,
        Sprite buttonImage = null,
        Color? textColor = null,
        Font font = null,
        int fontSize = 14,
        TextAnchor alignment = TextAnchor.MiddleCenter,
        Vector2? buttonSize = null,
        Color? buttonColor = null,
        Sprite pressedSprite = null,
        Sprite highlightedSprite = null,
        Sprite disabledSprite = null)
    {
        GameObject buttonObject = new GameObject("Button");
        buttonObject.transform.SetParent(parent, false);

        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();

        if (buttonImage != null)
        {
            image.sprite = buttonImage;
        }
        else
        {
            image.color = buttonColor ?? Color.gray; // Default color if no sprite is provided
        }

        button.targetGraphic = image;

        if (pressedSprite != null || highlightedSprite != null || disabledSprite != null)
        {
            SpriteState spriteState = new SpriteState
            {
                pressedSprite = pressedSprite,
                highlightedSprite = highlightedSprite,
                disabledSprite = disabledSprite
            };
            button.spriteState = spriteState;
        }

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);
        Text text = textObject.AddComponent<Text>();
        text.text = buttonText;
        text.font = font ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = textColor ?? Color.black;

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = buttonSize ?? new Vector2(160, 60); // Default size

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        button.onClick.AddListener(onClick);
        return buttonObject;
    }
}

