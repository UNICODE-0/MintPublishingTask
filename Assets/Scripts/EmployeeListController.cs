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
            _employeeData[i].first_name, _employeeData[i].last_name,
            _employeeData[i].email, _employeeData[i].ip_address);

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
    private void OnApplicationQuit() 
    {
        JsonHelper.SaveToJSON<EmployeeData>(_employeeData, 
        Application.dataPath + "/StreamingAssets/test_task_mock_data.json");
    }
}

[Serializable]
public class EmployeeData
{
    public int id;
    public string first_name;
    public string last_name;
    public string email;
    public string gender;
    public string ip_address;
    public int imageIndex = -1;
    public bool isFavorite;

    public EmployeeData(int id, string firstName, string lastName, string email, string gender, string ipAddress)
    {
        this.id = id;
        this.first_name = firstName;
        this.last_name = lastName;
        this.email = email;
        this.gender = gender;
        this.ip_address = ipAddress;
    }
}