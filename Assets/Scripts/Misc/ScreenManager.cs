using System;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private GameObject _favoriteScreen;
    [SerializeField] private ProfileInitializer _profileScreen;

    public static ScreenManager instance;

    private Screen _lastScreen = Screen.Main;
    private Screen _currentScreen = Screen.Main;
    
    public static event Action<Screen> OnScreenChange;

    private void Awake() 
    {
        if (instance == null) instance = this;
        else if(instance == this) Destroy(gameObject);
    }
    public void OpenMainScreen()
    {
        _lastScreen = _currentScreen;
        _currentScreen = Screen.Main;

        _mainScreen.SetActive(true);
        _favoriteScreen.SetActive(false);
        _profileScreen.gameObject.SetActive(false);

        if(OnScreenChange is not null) OnScreenChange(Screen.Main);
    }
    public void OpenFavoriteScreen()
    {
        _lastScreen = _currentScreen;
        _currentScreen = Screen.Favorite;

        _mainScreen.SetActive(false);
        _favoriteScreen.SetActive(true);
        _profileScreen.gameObject.SetActive(false);

        if(OnScreenChange is not null) OnScreenChange(Screen.Favorite);
    }
    public void OpenProfileScreen(EmployeeListItem item)
    {
        _lastScreen = _currentScreen;
        _currentScreen = Screen.Profile;

        _mainScreen.SetActive(false);
        _favoriteScreen.SetActive(false);
        _profileScreen.gameObject.SetActive(true);
        _profileScreen.Initialize(item);

        if(OnScreenChange is not null) OnScreenChange(Screen.Profile);
    }

    public void ReturnFromProfile()
    {
        switch (_lastScreen)
        {
            case Screen.Main:
                OpenMainScreen();
            break;
            case Screen.Favorite:
                OpenFavoriteScreen();
            break;
        }
    }
}

public enum Screen
{
    Main,
    Favorite,
    Profile
}
