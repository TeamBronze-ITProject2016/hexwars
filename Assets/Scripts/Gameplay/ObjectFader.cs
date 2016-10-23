/* ObjectFader.cs
 * Adapted from FadeObjectInOut.cs (http://wiki.unity3d.com/index.php/FadeObjectInOut)
 * Original author: Hayden Scott-Baron (Dock) - http://starfruitgames.com
 * Adapted by: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * 
 * Description: Allows an object to fade out over time
*/

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class ObjectFader : Photon.MonoBehaviour
    {
        [Tooltip("Length of fade effect in seconds")]
        public float fadeTime = 0.5f;

        [Tooltip("Whether or not to fade out on start")]
        public bool fadeOutOnStart = true;

        // Store colors
        private Color[] colors;

        // Allow automatic fading on the start of the scene
        void Start()
        {
            if (fadeOutOnStart)
                FadeOut(fadeTime);
        }

        // Check the alpha value of most opaque object
        private float MaxAlpha()
        {
            float maxAlpha = 0.0f;
            Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
            foreach (Renderer item in rendererObjects)
            {
                maxAlpha = Mathf.Max(maxAlpha, item.material.color.a);
            }
            return maxAlpha;
        }

        // Fade sequence
        IEnumerator FadeSequence(float fadingOutTime)
        {
            // Log fading direction, then precalculate fading speed as a multiplier
            bool fadingOut = (fadingOutTime < 0.0f);
            float fadingOutSpeed = 1.0f / fadingOutTime;

            // Grab all child objects
            Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
            if (colors == null)
            {
                // Create a cache of colors if necessary
                colors = new Color[rendererObjects.Length];

                // Store the original colours for all child objects
                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    colors[i] = rendererObjects[i].material.color;
                }
            }

            // Make all objects visible
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                rendererObjects[i].enabled = true;
            }

            // Get current max alpha
            float alphaValue = MaxAlpha();

            // Iterate to change alpha value 
            while ((alphaValue >= 0.0f && fadingOut) || (alphaValue <= 1.0f && !fadingOut))
            {
                alphaValue += Time.deltaTime * fadingOutSpeed;

                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
                    newColor.a = Mathf.Min(newColor.a, alphaValue);
                    newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                    rendererObjects[i].material.SetColor("_Color", newColor);
                }

                yield return null;
            }

            // Turn objects off after fading out
            if (fadingOut)
            {
                for (int i = 0; i < rendererObjects.Length; i++)
                {
                    rendererObjects[i].enabled = false;
                }
            }

            // Destroy at end of fade out
            Destroy(gameObject);
        }

        public void FadeOut()
        {
            FadeOut(fadeTime);
        }

        void FadeOut(float newFadeTime)
        {
            StopAllCoroutines();
            StartCoroutine("FadeSequence", -newFadeTime);
        }
    }
}
