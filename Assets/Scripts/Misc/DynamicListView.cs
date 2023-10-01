using System;
using UnityEngine;
 
public class DynamicListView : MonoBehaviour
{
    [SerializeField] private int _itemHeight;
    [SerializeField] private int _spacing;
    [SerializeField] private int _topPadding;
    [SerializeField] private int _topBottom;
    [SerializeField] private EmployeeListItem[] _views;
    [SerializeField] private RectTransform _content;
    [SerializeField] private int _itemsCount;
    
    public event Action<int, EmployeeListItem> OnItemShowed;
 
    private int _count;
    private int _oldGlobalIndex = -1;
    private RectTransform _item;

    private void Start()
    {
        Initialize(_itemsCount);
    }
    public void UpdateContnet()
    {
        float contentY = _content.anchoredPosition.y - _spacing;
 
        if (contentY < 0)
            return;
        
        int globalItemIndex = Mathf.FloorToInt(contentY / (_itemHeight + _spacing));
 
        if (_oldGlobalIndex == globalItemIndex)
            return;

        int rangeStart = _oldGlobalIndex > globalItemIndex ? globalItemIndex : _oldGlobalIndex + 1;
        int rangeEnd = _oldGlobalIndex > globalItemIndex ? _oldGlobalIndex - 1 : globalItemIndex;
        for (int currentIndex = rangeStart; currentIndex <= rangeEnd; currentIndex++)
        {
            if (currentIndex > _oldGlobalIndex) 
            {
                int localIndex = (currentIndex % _views.Length) - 1;
                if (localIndex < 0) localIndex = _views.Length - 1;
                int viewIndex = currentIndex + _views.Length - 1;

                if (viewIndex < _count) {
                    
                    _item = _views[localIndex].RectTransform;

                    Vector2 itemPos = _item.anchoredPosition;
                    itemPos.y = -(_topPadding + viewIndex * _spacing + viewIndex * _itemHeight);
                    _item.anchoredPosition = itemPos;
    
                    OnItemShowed(viewIndex, _views[localIndex]);
                }
            }
            else 
            {
                var localIndex = currentIndex % _views.Length;
                _item = _views[localIndex].RectTransform;
    
                Vector2 itemPos = _item.anchoredPosition;
                itemPos.y = -(_topPadding + currentIndex * _spacing + currentIndex * _itemHeight);
                _item.anchoredPosition = itemPos;
    
                OnItemShowed(currentIndex, _views[localIndex]);
            }
        }
        
        _oldGlobalIndex = globalItemIndex;
    }
 
    private void Initialize(int count)
    {
        _oldGlobalIndex = 0;
        _count = count;
    
        float contentHeight = _itemHeight * count * 1f + _topPadding + _topBottom + (count == 0 ? 0 : ((count - 1) * _spacing));
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, contentHeight);

        var contentPos = _content.anchoredPosition;
        contentPos.y = 0;
        _content.anchoredPosition = contentPos;

        int offset = _topPadding;
 
        for (int i = 0; i < _views.Length; i++) 
        {
            if (i < count) 
            {
                _views[i].gameObject.SetActive(true);

                contentPos = _views[i].RectTransform.anchoredPosition;
                contentPos.y = -offset;
                _views[i].RectTransform.anchoredPosition = contentPos;
                offset += _spacing + _itemHeight;
 
                OnItemShowed(i, _views[i]);
            }
        }
    }
}