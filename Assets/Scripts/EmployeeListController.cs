using System;
using System.Collections.Generic;
using UnityEngine;
public class EmployeeListController : MonoBehaviour
{
    [SerializeField] private EmployeeListItem _employeeItemPrefab;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private uint _employeeLoadCount; 

    private List<EmployeeData> _employeeData = new List<EmployeeData>();
    private List<EmployeeListItem> _employeeListItems = new List<EmployeeListItem>();
    private void OnEnable() 
    {
        ScreenManager.OnScreenChange += HandleScreenChange;
    }
    private void OnDisable() 
    {
        ScreenManager.OnScreenChange -= HandleScreenChange;
    }
    private void Start() 
    {
        if(_employeeListItems.Count == 0)
        {
            DisplayEmployeeList();
            ImageLoader.instance.LoadImages(LoadEmployeeImages);
        }
    }
    private void DisplayEmployeeList()
    {
        _employeeData = JsonHelper.ReadListFromJSON<EmployeeData>
        (Application.dataPath + "/StreamingAssets/test_task_mock_data.json");
        
        for (int i = 0; i < _employeeLoadCount && i < _employeeData.Count; i++)
        {
            _employeeListItems.Add(Instantiate(_employeeItemPrefab, _itemsParent));

            _employeeListItems[i].Initialize(null, 
            _employeeData[i].first_name, _employeeData[i].last_name, _employeeData[i].gender,
            _employeeData[i].email, _employeeData[i].ip_address, _employeeData[i].isFavorite);

            if(i % 2 != 0) _employeeListItems[i].SwapPaddingColor();
        }
    }
    private void LoadEmployeeImages(Sprite[] sprites)
    {
        List<int> indexes = null;
        for (int i = 0; i < _employeeListItems.Count; i++)
        {
            if(_employeeData[i].imageIndex == -1)
            {
                if(indexes == null) 
                    indexes = RandomSequenceGenerator.Generate(_employeeListItems.Count, 0, sprites.Length - 1);

                _employeeListItems[i].Image.sprite = sprites[indexes[i]];
                _employeeData[i].imageIndex = indexes[i];
            }
            else
            {                  
                _employeeListItems[i].Image.sprite = sprites[_employeeData[i].imageIndex];
            }
        }
    }
    private void HandleScreenChange(Screen currentScreen)
    {
        switch (currentScreen)
        {
            case Screen.Main:
                for (int i = 0; i < _employeeListItems.Count; i++)
                {
                    _employeeListItems[i].gameObject.SetActive(true);
                }
            break;
            case Screen.Favorite:
                for (int i = 0; i < _employeeListItems.Count; i++)
                {
                    if(!_employeeListItems[i].IsFavorite)
                        _employeeListItems[i].gameObject.SetActive(false);
                    else
                        _employeeListItems[i].gameObject.SetActive(true);
                }
            break;
            case Screen.Profile:
                for (int i = 0; i < _employeeListItems.Count; i++)
                {
                    _employeeListItems[i].gameObject.SetActive(false);
                }
            break;
        }
    }
    private void UpdateEmployeeData()
    {
        for (int i = 0; i < _employeeListItems.Count; i++)
        {
            _employeeData[i].isFavorite = _employeeListItems[i].IsFavorite;
        }

        JsonHelper.SaveToJSON<EmployeeData>(_employeeData, 
        Application.dataPath + "/StreamingAssets/test_task_mock_data.json");
    }
    private void OnApplicationQuit() 
    {
        UpdateEmployeeData();
    }
}