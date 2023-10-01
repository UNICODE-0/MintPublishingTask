using System.IO;
using UnityEngine;
public class EmployeeListController : MonoBehaviour
{
    const string EMPLOYEE_JSON_NAME = "EmployeeData";

    [SerializeField] private EmployeeListItem _employeeItemPrefab;
    [SerializeField] private DynamicListView _dynamicListView;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private uint _employeeLoadCount;
    
    private EmployeeData[] _employeeData;
    private EmployeeListItem[] _employeeListItems;
    private Sprite[] _employeeSprites;
    private int[] _employeeRandomImageIndices;
    private void OnEnable()
    {
        ScreenManager.OnScreenChange += HandleScreenChange;
        _dynamicListView.OnItemShowed += InitializeListItem;
    }

    private void OnDisable() 
    {
        ScreenManager.OnScreenChange -= HandleScreenChange;
        _dynamicListView.OnItemShowed -= InitializeListItem;
    }
    private void Start()
    {
        _employeeListItems = _dynamicListView.Items;

        LoadEmployeeData();
        _dynamicListView.Initialize(_employeeData.Length);
        ImageLoader.instance.LoadImages(LoadEmployeeImages);
    }
    private void LoadEmployeeData()
    {
        if(File.Exists(Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json"))
        {
            _employeeData = JsonHelper.ReadListFromJSON<EmployeeData>
            (Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json");
        }

        if(_employeeData is null) 
        {
            _employeeData = JsonHelper.ReadListFromJSONString<EmployeeData>
            ((Resources.Load(EMPLOYEE_JSON_NAME) as TextAsset).text);
        }
    }
    private void LoadEmployeeImages(Sprite[] sprites)
    {
        _employeeSprites = sprites;

        for (int i = 0; i < _employeeListItems.Length; i++)
        {
            if(_employeeData[i].imageIndex == -1)
            {
                if(_employeeRandomImageIndices is null) 
                    _employeeRandomImageIndices = RandomSequenceGenerator.Generate(_employeeData.Length, 0, sprites.Length - 1);

                _employeeListItems[i].Image.sprite = sprites[_employeeRandomImageIndices[i]];
                _employeeData[i].imageIndex = _employeeRandomImageIndices[i];
            }
            else
            {   
                int globalIndex = _employeeListItems[i].globalIndex;     
                int imageIndex =  _employeeData[globalIndex].imageIndex;
                _employeeListItems[i].Image.sprite = sprites[imageIndex];
            }
        }
    }
    private void HandleScreenChange(Screen currentScreen)
    {
        switch (currentScreen)
        {
            case Screen.Main:
                for (int i = 0; i < _employeeListItems.Length; i++)
                {
                    _employeeListItems[i].gameObject.SetActive(true);
                }
            break;
            case Screen.Favorite:
                for (int i = 0; i < _employeeListItems.Length; i++)
                {
                    if(_employeeListItems[i].IsFavorite)
                        _employeeListItems[i].gameObject.SetActive(true);
                    else
                        _employeeListItems[i].gameObject.SetActive(false);
                }
            break;
            case Screen.Profile:
                for (int i = 0; i < _employeeListItems.Length; i++)
                {
                    _employeeListItems[i].gameObject.SetActive(false);
                }
            break;
        }
    }
    private void UpdateEmployeeData()
    {
        JsonHelper.SaveToJSON<EmployeeData>(_employeeData, 
        Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json");
    }
    private void InitializeListItem(int index, EmployeeListItem item)
    {
        EmployeeData data = _employeeData[index];
        Sprite sprite = null;
        if(_employeeSprites is not null)
        {
            if(_employeeData[index].imageIndex == -1)
            {
                if(_employeeRandomImageIndices is null) 
                    _employeeRandomImageIndices = RandomSequenceGenerator.Generate(_employeeData.Length, 0, _employeeSprites.Length - 1);

                int imageIndex = _employeeRandomImageIndices[index];
                sprite = _employeeSprites[imageIndex];
                _employeeData[index].imageIndex = imageIndex;
            } else sprite = _employeeSprites[_employeeData[index].imageIndex];
        }

        item.Initialize(data, sprite, (int index, bool status) => _employeeData[index].isFavorite = status);
        item.globalIndex = index;
    }
    private void OnApplicationPause(bool pauseStatus) 
    {
        if(pauseStatus) UpdateEmployeeData();
    }
    private void OnApplicationQuit() 
    {
        UpdateEmployeeData();
    }
}