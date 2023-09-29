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
    private Sprite[] _employeeImages;
    private void Start() 
    {
        _employeeData = JsonHelper.ReadListFromJSON<EmployeeData>
        (Application.dataPath + "/StreamingAssets/test_task_mock_data.json");
        
        for (int i = 0; i < _employeeLoadCount && i < _employeeData.Count; i++)
        {
            _employeeListItems.Add(Instantiate(_employeeItemPrefab, _itemsParent));
            _employeeListItems[i].Initialize(_employeeImages[(i + 5) % 5], _employeeData[i].first_name, _employeeData[i].last_name,
            _employeeData[i].email, _employeeData[i].ip_address);
        }
    }
    private void OnEnable() 
    {
        ImageLoader.ImagesLoaded += SetEmployeeImages;
    }
    private void OnDisable() 
    {
        ImageLoader.ImagesLoaded -= SetEmployeeImages;
    }
    private void SetEmployeeImages(Sprite[] sprites)
    {
        _employeeImages = sprites;
        for (int i = 0; i < _employeeListItems.Count; i++)
        {
            _employeeListItems[i].Image.sprite = sprites[i];
        }
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