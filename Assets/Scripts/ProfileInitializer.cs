using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileInitializer : MonoBehaviour
{
    [SerializeField] private Image _profileIcon;
    [SerializeField] private Image _favoriteIcon;
    [SerializeField] private TMP_Text _fullName;
    [SerializeField] private TMP_Text _gender;
    [SerializeField] private TMP_Text _mail;
    [SerializeField] private TMP_Text _ip;

    public void Initialize(EmployeeListItem item)
    {
        _profileIcon.sprite = item.Image.sprite;
        _favoriteIcon.sprite = item.CurrentFavoriteIcon;
        _fullName.text = item.FirstName + " " + item.SecondName;
        _gender.text = item.Gender;
        _mail.text = item.Mail;
        _ip.text = item.Ip;
    }
}
