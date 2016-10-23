/* FadeOnDestroy.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Creates a fade effect when the object is destroyed
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class FadeOnDestroy : Photon.PunBehaviour
    {
        void OnDestroy()
        {
            // Create a new object for the fade effect
            GameObject fadeObj = new GameObject();
            fadeObj.name = "FadeObj";
            
            // Set fadeObj position/rotation
            fadeObj.transform.position = transform.position;
            fadeObj.transform.rotation = transform.rotation;

            // Set fadeObj's sprite to this object's sprite
            SpriteRenderer oldSpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer spriteRenderer = fadeObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = oldSpriteRenderer.sprite;
            spriteRenderer.color = oldSpriteRenderer.color;
            spriteRenderer.material = oldSpriteRenderer.material;

            // Set fadeObj's collider to be inactive but have the same path as this object's collider.
            // Needed to draw outlines with OutlineRenderer
            PolygonCollider2D oldCollider = GetComponent<PolygonCollider2D>();
            PolygonCollider2D collider = fadeObj.AddComponent<PolygonCollider2D>();
            collider.enabled = false;
            collider.SetPath(0, oldCollider.GetPath(0));
            fadeObj.AddComponent<OutlineRenderer>();

            // Begin fade
            fadeObj.AddComponent<ObjectFader>();
        }
    }
}