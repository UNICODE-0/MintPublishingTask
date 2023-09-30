using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class ImageLoader : MonoBehaviour
{
    private const uint IMAGE_COUNT = 5;

    [SerializeField] private string _imageFolderName = "EmployeeImages";
    [SerializeField] private string _imageName = "img";
    
    private Sprite[] _sprites = new Sprite[IMAGE_COUNT];
    private uint _spritesCount = 0;
    private Action<Sprite[]> _onLoadImagesAction;
    
    public static ImageLoader instance;

    private void Awake() 
    {
        if (instance == null) instance = this;
        else if(instance == this) Destroy(gameObject);
    }
    public void LoadImages(Action<Sprite[]> OnLoadImagesAction)
    {
        _onLoadImagesAction = OnLoadImagesAction;

        string directoryPath = $"{Application.dataPath}/{_imageFolderName}";
        Directory.CreateDirectory(directoryPath);
        for (int i = 0; i < IMAGE_COUNT; i++)
        {
            string imagePath = $"{directoryPath}/{_imageName}{i}.png";

            if(File.Exists(imagePath))
            {
                Texture2D texture = new Texture2D(200, 200);
                texture.LoadImage(File.ReadAllBytes(imagePath));
                AddSpriteToArray(texture.ConvertToSprite(), i);
            } else
            { 
                // Chnage https://picsum.photos/200 -> https://loremflickr.com/200/200 due to difficulties with downloading via VPN
                StartCoroutine(DownloadImage("https://loremflickr.com/200/200", imagePath, i));
            }
        }
    }
    private void AddSpriteToArray(Sprite sprite, int index)
    {
        _sprites[index] = sprite;
        _spritesCount++;

        if(_spritesCount == IMAGE_COUNT 
        && _onLoadImagesAction is not null) _onLoadImagesAction(_sprites);
    }
    private IEnumerator DownloadImage(string url, string path, int arrayIndex) 
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) 
        {
            Debug.Log(www.error);
            AddSpriteToArray(null, arrayIndex);
        }
        else 
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            File.WriteAllBytes(path, texture.EncodeToPNG());
            AddSpriteToArray(texture.ConvertToSprite(), arrayIndex);
        }
    }
}
