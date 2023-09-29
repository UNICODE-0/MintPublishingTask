using UnityEngine;
using UnityEngine.UI;

public class EmployeeListController : MonoBehaviour
{
    [SerializeField] private Image[] image;
    private Sprite[] _employeeImages;
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
        for (int i = 0; i < 5; i++)
        {
            image[i].sprite = sprites[i];
        }
    }
}
