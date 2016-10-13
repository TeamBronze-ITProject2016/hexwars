using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class FadeOnDestroy : Photon.PunBehaviour
    {
        void OnDestroy()
        {
            GameObject fadeObj = new GameObject();
            fadeObj.name = "AIFade";

            fadeObj.transform.position = transform.position;
            fadeObj.transform.rotation = transform.rotation;

            SpriteRenderer oldSpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer spriteRenderer = fadeObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = oldSpriteRenderer.sprite;
            spriteRenderer.color = oldSpriteRenderer.color;
            spriteRenderer.material = oldSpriteRenderer.material;

            PolygonCollider2D oldCollider = GetComponent<PolygonCollider2D>();
            PolygonCollider2D collider = fadeObj.AddComponent<PolygonCollider2D>();
            collider.enabled = false;
            collider.SetPath(0, oldCollider.GetPath(0));
            fadeObj.AddComponent<OutlineRenderer>();

            fadeObj.AddComponent<LocalObjectFader>();
        }
    }
}