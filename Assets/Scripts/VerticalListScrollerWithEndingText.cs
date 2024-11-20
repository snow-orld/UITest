using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VerticalListScrollerWithEndingText : MonoBehaviour
{
    public GameObject itemPrefabGo;
    public GameObject endingTextPrefabGo;
    [SerializeField] private int itemCount = 5;
    
    private readonly string contentPath = "Viewport/Content";
    private GameObject contentRoot;
    private readonly string viewportPath = "Viewport";
    private GameObject viewportRoot;
    private GameObject scrollViewRoot;
    private float itemHeight;
    private List<GameObject> itemList = new List<GameObject>();

    private void Awake()
    {
        contentRoot = transform.Find(contentPath).gameObject;
        viewportRoot = transform.Find(viewportPath).gameObject;
        scrollViewRoot = gameObject;
        InitList(itemCount);
    }

    private void OnDestroy()
    {
        DestroyList();
    }

    private void InitList(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            Debug.Log($"[{GetType().Name}]Initiating {i + 1}/{itemCount}-th item");
            GameObject go = Instantiate(itemPrefabGo, contentRoot.transform);
            itemList.Add(go);

            if (itemHeight == 0)
            {
                itemHeight = go.GetComponent<LayoutElement>().preferredHeight;
                Debug.Log($"[{GetType().Name}]itemHeight = {itemHeight}");
            }
        }
        
        AddEndingText();
    }

    private void DestroyList()
    {
        for (int i = 0; i < itemCount; i++)
        {
            Debug.Log($"[{GetType().Name}]Destroying {i + 1}/{itemCount}-th item");
            Destroy(itemList[i]);
        }
        itemList.Clear();
    }

    /// <summary>
    /// 在Scroll List尾部加入文字。
    /// 需要判断当前List是否超出屏幕范围，如果是，需要将元素加到Content的Layout中；否则直接加到Viewport的底部。
    /// </summary>
    private void AddEndingText()
    {
        Debug.Log($"[{GetType().Name}]AddEndingText");
        
        // Get vertical layout spacing
        float verticalSpacing = contentRoot.GetComponent<VerticalLayoutGroup>().spacing;
        
        // Get scrollview actual height
        float scrollViewHeight = scrollViewRoot.GetComponent<RectTransform>().sizeDelta.y;
        
        // Get content actual height
        // float contentHeight = contentRoot.GetComponent<RectTransform>().sizeDelta.y; // returns 0
        float contentHeight = itemHeight * itemCount +
                              verticalSpacing * (itemCount - 1);

        // Instantiate the ending UI element
        GameObject endingText = Instantiate(endingTextPrefabGo);
        
        // Get endingText's actual height
        float endingTextHeight = endingText.GetComponent<RectTransform>().sizeDelta.y;

        // Decide which parent to add this endingText
        bool isOverHigh = contentHeight + verticalSpacing + endingTextHeight > scrollViewHeight;
        Debug.Log($"[{GetType().Name}]contentHeight={contentHeight}, verticalSpacing={verticalSpacing}," +
                  $"endingTextHeight={endingTextHeight}, scrollViewHeight={scrollViewHeight}." +
                  $"isOverHigh={isOverHigh}");
        endingText.transform.SetParent(isOverHigh ? contentRoot.transform : viewportRoot.transform);

        // Adjust ending text's rect transform
        RectTransform rt = endingText.GetComponent<RectTransform>();
        SetUIAtBottomStretch(endingTextHeight, rt);
    }

    private void SetUIAtBottomStretch(float height, RectTransform rt)
    {
        // 设置UI元素的宽度为父容器的宽度
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
 
        // 设置UI元素的锚点到底部并横向居中
        rt.pivot = new Vector2(0.5f, 0);
 
        // 设置UI元素的锚点位置到底部居中
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
 
        // 设置UI元素的位置到底部
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, height);
    }
}
