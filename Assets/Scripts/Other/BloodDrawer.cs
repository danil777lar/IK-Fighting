using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDrawer
{
    public static void Draw(SpriteRenderer rend, Vector2 point) 
    {
        float rad = Random.Range(1, DataGameMain.Default.bloodMaxRad);
        float paintForce = Random.Range(DataGameMain.Default.bloodMinPaintForce, 1f);

        Vector2Int nearestPixel = Vector2Int.zero;
        Texture2D texture = new Texture2D(rend.sprite.texture.width, rend.sprite.texture.height);
        texture.SetPixels(rend.sprite.texture.GetPixels());
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < rend.sprite.rect.width; x++) 
        {
            for (int y = 0; y < rend.sprite.rect.height; y++)
            {
                int xp = x + (int)rend.sprite.rect.x;
                int yp = y + (int)rend.sprite.rect.y;

                if (texture.GetPixel(xp, yp).a != 0f) 
                {
                    float nearestDist = Vector2.Distance(point, PixelPosition(rend, nearestPixel.x, nearestPixel.y));
                    float curentDist = Vector2.Distance(point, PixelPosition(rend, xp, yp));
                    if (curentDist < nearestDist)
                        nearestPixel = new Vector2Int(xp, yp);
                }
            }
        }

        for (int x = 0; x < rend.sprite.rect.width; x++)
        {
            for (int y = 0; y < rend.sprite.rect.height; y++)
            {
                int xp = x + (int)rend.sprite.rect.x;
                int yp = y + (int)rend.sprite.rect.y;

                if (texture.GetPixel(xp, yp).a != 0f)
                {
                    Color color = texture.GetPixel(xp, yp);
                    if (nearestPixel == new Vector2(xp, yp))
                        color.a = 0f;
                    else if (Vector2.Distance(nearestPixel, new Vector2(xp, yp)) < rad / 2f)
                        color = Color.Lerp(color, new Color(0.2f, 0f, 0f), paintForce);
                    else if (Vector2.Distance(nearestPixel, new Vector2(xp, yp)) < rad)
                        color = Color.Lerp(color, new Color(0.4f, 0f, 0f), paintForce);
                    texture.SetPixel(xp, yp, color);
                }
            }
        }

        texture.Apply();
        float ppu = rend.sprite.pixelsPerUnit;
        Vector2 pivot = rend.sprite.pivot / new Vector2(rend.sprite.rect.width, rend.sprite.rect.height);
        rend.sprite = Sprite.Create(texture, rend.sprite.rect, pivot, ppu);
    }

    private static Vector2 PixelPosition(SpriteRenderer rend, float x, float y, bool debug = false) 
    {
        float pixelSize = 1f / rend.sprite.pixelsPerUnit;
        Vector2 pivot = rend.sprite.pivot + new Vector2(rend.sprite.rect.x, rend.sprite.rect.y);
        Vector2 position = (new Vector2(x, y) * pixelSize) - (pivot * pixelSize);
        return rend.transform.TransformPoint(position);
    }
}
