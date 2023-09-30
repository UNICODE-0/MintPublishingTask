using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class EmployeeListItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private Image _padding;
    [SerializeField] private Color _swapPaddingColor;

    public Image Image
    {
        get { return _image; }
    }
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }
    public string Mail { get; private set; }
    public string Ip { get; private set; }

    public void Initialize(Sprite image, string firstName, string secondName, string mail, string ip)
    {
        FirstName = firstName;
        SecondName = secondName;
        Mail = mail;
        Ip = ip;


        if(image != null) _image.sprite = image;
        _name.text = $"{firstName} {SecondName}";
        _info.text = $"{mail} <color=#D9D9D9>|</color> {ip}";
    }
    public void SwapPaddingColor()
    {
        _padding.color = _swapPaddingColor;
    }
}
