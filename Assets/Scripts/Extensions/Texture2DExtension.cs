using UnityEngine;

public static class Texture2DExtension
{
    public static Sprite ConvertToSprite(this Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
