using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceTile : MonoBehaviour
{
    // Variables
    public Resource resource;
    public SpriteRenderer spriteRenderer;

    public void SetTile(Resource resource)
    {
        this.resource = resource;
        spriteRenderer.sprite = SpritesManager.GetSprite(resource.name);
    }
}
