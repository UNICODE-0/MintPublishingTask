using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class EmployeeListItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private Image _favoriteIcon;
    [SerializeField] private Sprite _favoriteSprite;
    [SerializeField] private Sprite _unfavoriteSprite;
    [SerializeField] private Image _padding;
    [SerializeField] private Color _swapPaddingColor;
    [SerializeField] private RectTransform _rectTransform;

    private bool _isFavorite;

    public RectTransform RectTransform
    {
        get { return _rectTransform; }
    }
    
    public Image Image
    {
        get { return _image; }
    }
    public Sprite CurrentFavoriteIcon
    {
        get { return _favoriteIcon.sprite; }
    }
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }
    public string Gender { get; private set; }
    public string Mail { get; private set; }
    public string Ip { get; private set; }
    public bool IsFavorite
    {
        get { return _isFavorite; }
        private set 
        { 
            _isFavorite = value;
            if(value)
            {
                _favoriteIcon.sprite = _favoriteSprite; 
            } else
            {
                _favoriteIcon.sprite = _unfavoriteSprite; 
            }
        }
    }
    public void Initialize(Sprite image, string firstName, string secondName, string gender, string mail, string ip, bool isFavorite)
    {
        FirstName = firstName;
        SecondName = secondName;
        Gender = gender;
        Mail = mail;
        Ip = ip;
        IsFavorite = isFavorite;

        if(image != null) _image.sprite = image;
        _name.text = $"{FirstName} {SecondName}";
        _info.text = $"{Mail} <color=#D9D9D9>|</color> {Ip}";
    }
    public void Initialize(EmployeeListItem item)
    {
        FirstName = item.FirstName;
        SecondName = item.SecondName;
        Mail = item.Mail;
        Ip = item.Ip;
        IsFavorite = item.IsFavorite;

        if(item.Image != null) _image.sprite = item.Image.sprite;
        _name.text = $"{FirstName} {SecondName}";
        _info.text = $"{Mail} <color=#D9D9D9>|</color> {Ip}";
    }
    public void SwapPaddingColor()
    {
        _padding.color = _swapPaddingColor;
    }
    public void ChangeFavoriteStatus()
    {
        if(IsFavorite) IsFavorite = false;
        else IsFavorite = true;
    }
    public void OpenProfile()
    {
        ScreenManager.instance.OpenProfileScreen(this);
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