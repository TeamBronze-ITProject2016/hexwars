using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class OutlineRenderer : MonoBehaviour
    {
        public float outlineWidth = 0.02f;

        private LineRenderer outline;
        private Vector3[] outlinePath;

        void Start()
        {
            GameObject outlineObj = new GameObject("Outline");
            outlineObj.transform.SetParent(transform);
            outlineObj.transform.localPosition = new Vector3(0, 0, -1);

            Vector2[] colliderPath = GetComponent<PolygonCollider2D>().GetPath(0);

            outline = outlineObj.AddComponent<LineRenderer>();
            outline.SetVertexCount(colliderPath.Length + 1);
            outline.useWorldSpace = false;
            outline.SetWidth(outlineWidth, outlineWidth);
            outline.SetColors(Color.black, Color.black);
            outline.material = new Material(Shader.Find("Sprites/Default"));

            outlinePath = new Vector3[colliderPath.Length + 1];

            for (int i = 0; i < colliderPath.Length; i++)
            {
                outlinePath[i] = transform.rotation * new Vector3(colliderPath[i].x, colliderPath[i].y, 0.0f);
            }

            outlinePath[colliderPath.Length] = transform.rotation * new Vector3(colliderPath[0].x, colliderPath[0].y, 0.0f);

            outline.SetPositions(outlinePath);
        }

        void Update()
        {
            float adjust = Camera.main.orthographicSize;
            outline.SetWidth(outlineWidth * adjust, outlineWidth * adjust);
        }
    }
}
