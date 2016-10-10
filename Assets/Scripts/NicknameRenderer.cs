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
            canvas = GameObject.FindGameObjectWithTag("Canvas");

            textObj = new GameObject("NicknameText");
            textObj.transform.SetParent(canvas.transform);

            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = photonView.owner.name;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 14;
            textComponent.fontStyle = FontStyle.Bold;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;

            Outline outlineComponent = textObj.AddComponent<Outline>();
            outlineComponent.effectDistance *= 0.75f;
            outlineComponent.effectColor = Color.black;
            outlineComponent.useGraphicAlpha = false;

            Outline outlineComponent2 = textObj.AddComponent<Outline>();
            outlineComponent2.effectDistance *= 0.75f;
            outlineComponent2.effectColor = Color.black;
            outlineComponent2.useGraphicAlpha = false;
        }

        void LateUpdate()
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

            textObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
        }

        void OnDestroy()
        {
            Destroy(textObj);
        }
    }
}