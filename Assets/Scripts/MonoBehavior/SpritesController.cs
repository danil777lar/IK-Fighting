using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesController : MonoBehaviour
{
    void Start()
    {
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

        foreach (SpriteRenderer sprite in FindObjectsOfType<SpriteRenderer>())
            if (sprite.tag != "Light") sprite.color = nearest.color; 

    }
}
