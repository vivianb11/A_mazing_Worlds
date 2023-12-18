using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroFeedback : MonoBehaviour
{
    public float scale = 1;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 position = GameInput.instance.GetGyroPercent() * scale;
        // clamp it to a circle
        if (position.magnitude > scale)
            position = position.normalized * scale;

        rectTransform.anchoredPosition = position;
    }
}
