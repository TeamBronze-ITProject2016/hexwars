using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Net.NetworkInformation;

public class DBServerTest{

    /* Tests server high score reply*/
    [Test]
    public void TestServerStatus()
    {

        string url = "http://128.199.229.64/hexwars";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream stream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string responseString = reader.ReadToEnd();
            Assert.NotNull(responseString);
        }
    }
    
    /* Unit test to update the server with a dummy value*/
    [Test]
    public void TestUpdate()
    {
        string url = "http://128.199.229.64/hexwars/";
        string name = "UnitTest/";
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

    /* Unit test to delete value from server */
    [Test]
    public void TestRemove()
    {
        string url = "http://128.199.229.64/remove/";
        string name = "UnitTest";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + name);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream stream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string responseString = reader.ReadToEnd();
            Assert.True(responseString == "200");
        }
    }
}
