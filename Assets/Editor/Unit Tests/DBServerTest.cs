using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using NUnit.Framework;

public class DBServerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Test]
    public void TestUpdate()
    {
        string url = "http://128.199.229.64/hexwars/";
        string name = "william/";
        string score = "98";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + name + score);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream stream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string responseString = reader.ReadToEnd();
            Assert.True(responseString == "200");
        }
    }
}
