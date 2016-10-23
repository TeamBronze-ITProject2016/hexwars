/* NicknameRenderer.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Renders a nickname over the gameobject based on its PhotonView owner
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class NicknameRenderer : Photon.MonoBehaviour
    {
        private GameObject textObj;
        private GameObject canvas;

        void Start()
        {
            // Find the canvas object in the scene
            canvas = GameObject.FindGameObjectWithTag("Canvas");

            // Create a new object to display the nickname
            textObj = new GameObject("NicknameText");
            textObj.transform.SetParent(canvas.transform);

            // Add text
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = photonView.owner.name;

            // Style text
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 14;
            textComponent.fontStyle = FontStyle.Bold;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;

            // Add outline
            Outline outlineComponent = textObj.AddComponent<Outline>();
            outlineComponent.effectDistance *= 0.75f;
            outlineComponent.effectColor = Color.black;
            outlineComponent.useGraphicAlpha = false;

            // Double up the outline (looks better)
            Outline outlineComponent2 = textObj.AddComponent<Outline>();
            outlineComponent2.effectDistance *= 0.75f;
            outlineComponent2.effectColor = Color.black;
            outlineComponent2.useGraphicAlpha = false;
        }

        void LateUpdate()
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

            // Update on-screen position of nickname
            textObj.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
        }

        void OnDestroy()
        {
            // Destroy nickname when this game object is destroyed
            Destroy(textObj);
        }
    }
}