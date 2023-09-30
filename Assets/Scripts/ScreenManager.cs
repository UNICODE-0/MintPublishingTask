using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private GameObject _favoriteScreen;
    [SerializeField] private GameObject _profileScreen;

    public static ScreenManager instance;
    private void Awake() 
    {
        if (instance == null) instance = this;
        else if(instance == this) Destroy(gameObject);
    }
    public void OpenMainScreen()
    {
        _mainScreen.SetActive(true);
        _favoriteScreen.SetActive(false);
        _profileScreen.SetActive(false);
    }
    public void OpenFavoriteScreen()
    {
        _mainScreen.SetActive(false);
        _favoriteScreen.SetActive(true);
        _profileScreen.SetActive(false);
    }
    public void OpenProfileScreen()
    {
        _mainScreen.SetActive(false);
        _favoriteScreen.SetActive(false);
        _profileScreen.SetActive(true);
    }
}
