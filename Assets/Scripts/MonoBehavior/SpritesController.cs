using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesController : MonoBehaviour
{
    private List<SpriteRenderer> sprites;

    void Start()
    {
        sprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer sprite in FindObjectsOfType<SpriteRenderer>())
            sprites.Add(sprite);

        SetSpriteColors();
    }

    private void SetSpriteColors() 
    {
        Light nearest= null;
        foreach (Light l in FindObjectsOfType<Light>())
        {
            if (nearest == null) 
                nearest = l;
            if (Vector3.Distance(nearest.transform.position, transform.position) > Vector3.Distance(l.transform.position, transform.position))
                nearest = l;
        }

        foreach (SpriteRenderer sprite in sprites.FindAll((s) => s.tag != "Light")) 
        {
            Color.RGBToHSV(nearest.color, out float hl, out float sl, out float vl);
            Color.RGBToHSV(sprite.color, out float hs, out float ss, out float vs);
            sprite.color = Color.HSVToRGB(hl, sl, vl - (vl - vs));
        }
    }
}
