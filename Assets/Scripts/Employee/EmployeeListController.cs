using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class EmployeeListController : MonoBehaviour
{
    const string EMPLOYEE_JSON_NAME = "EmployeeData";

    [SerializeField] private DynamicListView _mainListView;
    
    private EmployeeData[] _employeeData;
    private EmployeeListItem[] _employeeListItems;
    private Sprite[] _employeeSprites;
    private int[] _employeeRandomImageIndices;
    private void OnEnable()
    {
        ScreenManager.OnScreenChange += HandleScreenChange;
        _mainListView.OnItemShowed += InitializeListItem;
    }

    private void OnDisable() 
    {
        ScreenManager.OnScreenChange -= HandleScreenChange;
        _mainListView.OnItemShowed -= InitializeListItem;
    }
    private void Start()
    {
        _employeeListItems = _mainListView.Items;

        LoadEmployeeData();
        _mainListView.Initialize(_employeeData.Length);
        ImageLoader.instance.LoadImages(LoadEmployeeImages);
    }
    private void LoadEmployeeData()
    {
        if(File.Exists(Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json"))
        {
            _employeeData = JsonHelper.ReadListFromJSON<EmployeeData>
            (Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json");
        }

        if(_employeeData is null || _employeeData.Length == 0) 
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
                _mainListView.gameObject.SetActive(true);
                _mainListView.Initialize(_employeeData.Length);
            break;
            case Screen.Favorite:
                _mainListView.gameObject.SetActive(true);
                List<int> favoriteEmployeeGlobalIndices = new List<int>();
                for (int i = 0; i < _employeeData.Length; i++)
                {
                    if(_employeeData[i].isFavorite) favoriteEmployeeGlobalIndices.Add(i);
                }
                _mainListView.Initialize(favoriteEmployeeGlobalIndices.Count, favoriteEmployeeGlobalIndices);
            break;
            case Screen.Profile:
                _mainListView.gameObject.SetActive(false);
            break;
        }
    }
    private void UpdateEmployeeData()
    {
        if(_employeeData?.Length is 0) return;

        JsonHelper.SaveToJSON<EmployeeData>(_employeeData, 
        Application.persistentDataPath + "/" + EMPLOYEE_JSON_NAME + ".json");
    }
    private void HandleOnFavoriteCallback(int index, bool status)
    {
        _employeeData[index].isFavorite = status;
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

        item.Initialize(data, sprite, HandleOnFavoriteCallback);
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