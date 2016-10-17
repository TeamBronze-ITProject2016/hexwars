using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    public GameObject yourScoreTextObj;
    public GameObject[] highScoresTextObjs;

    public void BackToMenu()
    {
        Application.LoadLevel(0);
    }
}
