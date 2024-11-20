using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VerticalScroller : MonoBehaviour
{
    [SerializeField][TextArea(4, 8)] private string _textContent;

    public string TextContent
    {
        get => _textContent;
        set { _textContent = value; Refresh(); }
    }

    public TMP_Text text;

    private void Awake()
    {
        Refresh();
    }

    private void Refresh()
    {
        text.text = _textContent;

        Debug.Log($"[{GetType().Name}]text height is {text.preferredHeight}");

        RectTransform rt = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        
        Debug.Log($"[{GetType().Name}]rt is null?{(rt == null ? "yes":"no")}");

        rt.sizeDelta = new Vector2(rt.sizeDelta.x, text.preferredHeight);
    }
    
    private void OnValidate()
    {
        Refresh();
    }
}
