/* OutlineRenderer.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Renders outlines based on the path of this object's PolygonCollider2D
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class OutlineRenderer : MonoBehaviour
    {
        [Tooltip("Base width of the outline")]
        public float outlineWidth = 0.02f;

        private LineRenderer outline;
        private Vector3[] outlinePath;

        void Start()
        {
            // Create new obj for the outline
            GameObject outlineObj = new GameObject("Outline");
            outlineObj.transform.SetParent(transform);
            outlineObj.transform.localPosition = new Vector3(0, 0, -1);

            // Get path for the outline
            Vector2[] colliderPath = GetComponent<PolygonCollider2D>().GetPath(0);

            // Add LineRenderer
            outline = outlineObj.AddComponent<LineRenderer>();
            outline.SetVertexCount(colliderPath.Length + 1);
            outline.useWorldSpace = false;
            outline.SetWidth(outlineWidth, outlineWidth);
            outline.SetColors(Color.black, Color.black);
            outline.material = new Material(Shader.Find("Sprites/Default"));

            outlinePath = new Vector3[colliderPath.Length + 1];

            // Set outline path to the path from PolygonCollider2D
            for (int i = 0; i < colliderPath.Length; i++)
            {
                outlinePath[i] = transform.rotation * new Vector3(colliderPath[i].x, colliderPath[i].y, 0.0f);
            }

            // Add final connection from end point to start point
            outlinePath[colliderPath.Length] = transform.rotation * new Vector3(colliderPath[0].x, colliderPath[0].y, 0.0f);

            // Set the LineRenderer's path
            outline.SetPositions(outlinePath);
        }

        void Update()
        {
            // Adjust outline width so that it appears to be the same width regardless of camera zoom
            float adjust = Camera.main.orthographicSize;
            outline.SetWidth(outlineWidth * adjust, outlineWidth * adjust);
        }
    }
}
