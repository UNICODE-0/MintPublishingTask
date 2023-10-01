using System;
using System.Collections.Generic;
using UnityEngine;
 
public class DynamicListView : MonoBehaviour
{
    [SerializeField] private int _itemHeight;
    [SerializeField] private int _spacing;
    [SerializeField] private int _topPadding;
    [SerializeField] private int _topBottom;
    [SerializeField] private EmployeeListItem[] _items;
    [SerializeField] private RectTransform _content;
    
    public event Action<int, EmployeeListItem> OnItemShowed;
    
    private List<int> _referenceGlobalIndices;
    private int _count;
    private int _oldGlobalIndex = -1;
    private RectTransform _item;

    public EmployeeListItem[] Items
    {
        get { return _items; }
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
                int localIndex = (currentIndex % _items.Length) - 1;
                if (localIndex < 0) localIndex = _items.Length - 1;
                int viewIndex = currentIndex + _items.Length - 1;

                if (viewIndex < _count) 
                {
                    _item = _items[localIndex].RectTransform;

                    Vector2 itemPos = _item.anchoredPosition;
                    itemPos.y = -(_topPadding + viewIndex * _spacing + viewIndex * _itemHeight);
                    _item.anchoredPosition = itemPos;

                    if(_referenceGlobalIndices is null)
                        OnItemShowed(viewIndex, _items[localIndex]);
                    else
                        OnItemShowed(_referenceGlobalIndices[viewIndex], _items[localIndex]);
                }
            }
            else 
            {
                var localIndex = currentIndex % _items.Length;
                _item = _items[localIndex].RectTransform;
    
                Vector2 itemPos = _item.anchoredPosition;
                itemPos.y = -(_topPadding + currentIndex * _spacing + currentIndex * _itemHeight);
                _item.anchoredPosition = itemPos;

                if(_referenceGlobalIndices is null)
                    OnItemShowed(currentIndex, _items[localIndex]);
                else
                    OnItemShowed(_referenceGlobalIndices[currentIndex], _items[localIndex]);
            }
        }
        
        _oldGlobalIndex = globalItemIndex;
    }
 
    public void Initialize(int count, List<int> referenceGlobalIndices = null)
    {
        _oldGlobalIndex = 0;
        _count = count;
        _referenceGlobalIndices = referenceGlobalIndices;
    
        float contentHeight = _itemHeight * count * 1f + _topPadding + _topBottom + (count == 0 ? 0 : ((count - 1) * _spacing));
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, contentHeight);

        var contentPos = _content.anchoredPosition;
        contentPos.y = 0;
        _content.anchoredPosition = contentPos;

        int offset = _topPadding;
 
        for (int i = 0; i < _items.Length; i++) 
        {
            if (i < count) 
            {
                _items[i].gameObject.SetActive(true);

                contentPos = _items[i].RectTransform.anchoredPosition;
                contentPos.y = -offset;
                _items[i].RectTransform.anchoredPosition = contentPos;
                offset += _spacing + _itemHeight;

                if(_referenceGlobalIndices is null)
                    OnItemShowed(i, _items[i]);
                else
                    OnItemShowed(_referenceGlobalIndices[i], _items[i]);
            } else _items[i].gameObject.SetActive(false);
        }
    }
}