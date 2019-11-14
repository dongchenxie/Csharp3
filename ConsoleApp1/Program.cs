using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {


        const string BASE_URL = "https://swapi.co/api/";
        static void Main(string[] args)
        {
            const string PLANETS = "planets/";
           
            const string FILMS = "films/";
            JObject p = CallRestMethod(new Uri(BASE_URL + PLANETS));
            JObject f = CallRestMethod(new Uri(BASE_URL + FILMS));
            while (true)
            {
                printAll(p.GetValue("results"), f.GetValue("results"));
                if (!string.IsNullOrEmpty(p.GetValue("next").ToString()))
                {
                p = CallRestMethod(new Uri(p.GetValue("next").ToString()));
                }
                else
                {
                    break;
                }
            }
        }
        static void printAll(JToken planets,JToken films)
        {
            foreach(var p in planets)
            {
                Console.WriteLine("=============================\nPlanets:\n"+p["name"]+"\n\nMovies:\n");
                foreach(var f in p["films"])
                {
                    var s =parseInt(f.ToString().Split("/")[5])-1;
                    Console.WriteLine(films[s]["title"]);
                }
                Console.WriteLine();
            }   
        }
        static int parseInt(string s)
        {
            int i = 0;
            if (!Int32.TryParse(s, out i))
            {
                i = -1;
            }
            return i;
        }
        static JObject CallRestMethod(Uri uri)
        {
            try
            {
                // Create a web request for the given uri
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                // Get the web response from the api
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get a stream to read the reponse
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                // Read the response and write it to the console
                JObject result = JObject.Parse(responseStream.ReadToEnd());
                // Close the connection to the api and the stream reader
                response.Close();
                responseStream.Close();
                return result;
            }
            catch (Exception e)
            {
                string result = $"{{'Error':'An error has occured. Could not get {uri.LocalPath}', 'Message': '{e.Message}'}}";
                return JObject.Parse(result);
            }
        }
    }


}
