﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using TeamBronze.HexWars;
#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
using UnityEngine.Networking;
#endif

public class Scoreboard : MonoBehaviour {

    public int maxScores = 4;

	// Use this for initialization
	void Start () {
        UpdateScoresBoard();
    }

    void Update()
    {
        GameObject scoresDisplay = transform.FindChild("PlayerBar").transform.FindChild("PlayerPointScore").gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("LocalPlayer");
        Text text = scoresDisplay.GetComponent<Text>();
        text.text = "Points/Score: " + player.GetComponent<Player>().points + "/" + player.GetComponent<Player>().rb.mass.ToString();
    }

    void AddText(string text)
    {
        GameObject scoresDisplay = transform.FindChild("Scores").gameObject;
        GameObject textGO = new GameObject("myTextGO");
        textGO.transform.SetParent(scoresDisplay.transform);
        Text myText = textGO.AddComponent<Text>();

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        myText.font = ArialFont;
        myText.color = Color.black;
        myText.alignment = TextAnchor.MiddleCenter;
        myText.text = text;
    }



	// Update is called once per frame
    [PunRPC]
    IEnumerator UpdateScoresBoard () {
        // Remove previous scores
        GameObject scoresDisplay = transform.FindChild("Scores").gameObject;
        foreach (Transform child in scoresDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        string url = "http://128.199.229.64/hexwars";
        WWW www = new WWW(url);
        yield return www;
        // Format string into array
        // http://stackoverflow.com/questions/19178983/how-in-c-sharp-to-convert-a-string-of-comma-separated-bracket-enclosed-nested
        string s = www.text;
        var result = s
               .Split(']')
               .Select(i => i.Replace('[', ' '))
               .Select(i => i.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .ToList()).ToList();

        int scorecount = 0;
        // Display score
        foreach (var player in result)
        {
            if (scorecount == maxScores) break;
            try
            {
                AddText(player[0] + " : " + player[1]);
                scorecount++;
            }
            catch
            {

            }
        }
    }
}
