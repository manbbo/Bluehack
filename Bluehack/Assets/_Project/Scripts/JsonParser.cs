using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonParser : MonoBehaviour {
    public CanvasHandler canvasHandler;
    public MidiaHandler midiaHandler;
    JsonData data = new JsonData();
    public static List<Database> databases = new List<Database>();
    public string path;
    //public string JsonPath;
    // Use this for initialization
    private void Start()
    {
        StartCoroutine(SetupJsonDirectory(path));
    }

    public IEnumerator SetupJsonDirectory(string JsonPath)
    {
        string path = JsonPath.Replace(@"\", "\\");
        if (Directory.Exists(path))
        {
            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();
            foreach (var file in fileInfo)
            {
                if (file.Name.Contains("json"))
                {
                    WWW www;
                    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        www = new WWW("file://" + path + "\\" + file.Name);
                    }
                    else
                    {
                        www = new WWW("file://" + path + "" + file.Name);
                    }
                    yield return www;
                    Debug.Log(file.Name);
                    string jsonData = www.text;
                    //string jsonData = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);  // Skip thr first 3 bytes (i.e. the UTF8 BOM)
                    databases.Add(new Database(file.Name, JsonMapper.ToObject(jsonData)));
                }
            }
        }
        else
        {
            Debug.Log("diretório inexistente");
        }
        parseJson();
    }
    public void parseJson() {
        foreach(var locations in databases) {
            Debug.Log((string)locations.localJson["name"]);
            foreach (JsonData item in databases[0].localJson["hotels"])
            {
                Debug.Log(item["name"]);
            }
        }
    }
    void manageEvent(string eventName, string localName=null,string hotelName=null) {
        switch (eventName) {
            case "LOCATION_FOUND":
                //midiaHandler.fade()
                foreach (var item in databases)
                {
                    if ((string)item.localJson["name"]==localName)
                    {
                        midiaHandler.fade((string)item.localJson["imageDirectory"], false,"location");
                    }
                }
                break;

            case "SHOW_HOTELS":
                foreach (var location in databases)
                {
                    if ((string)location.localJson["name"] == localName)
                    {
                        // midiaHandler.fade((string)location.localJson["imageDirectory"], false);
                        List<string> paths = new List<string>();
                        foreach (JsonData item in location.localJson["hotels"])
                        {
                            paths.Add((string)item["thumbnail"]);
                        }
                        canvasHandler.fade(paths,false);
                    }
                }
                break;

            case "HOTEL_FOUND":
                foreach (var location in databases)
                {
                    if ((string)location.localJson["name"] == localName)
                    {
                        // midiaHandler.fade((string)location.localJson["imageDirectory"], false);
                        foreach (JsonData item in location.localJson["hotels"])
                        {
                            if ((string)item["name"] == hotelName)
                            {
                                midiaHandler.fade((string)item["imageDirectory"],false,"hotel");
                            }
                         }
                        
                    }
                }
                break;
       
            case "OPEN_VIDEO":
                foreach (var item in databases)
                {
                    if ((string)item.localJson["name"] == localName)
                    {
                        midiaHandler.fade((string)item.localJson["videoDirectory"], false,"video");
                    }
                }
                break;

            case "SHOW_INFO":
                foreach (var item in databases)
                {
                    if ((string)item.localJson["name"] == localName)
                    {
                        midiaHandler.fadeUI((string)item.localJson["info"], false);
                    }
                }
                break;

            case "BACK":
                if (hotelName!=null)
                {
                    midiaHandler.fade(null, true, "hotel");
                }else
                {
                    midiaHandler.fade(null, true, "location");
                }
                break;
        }

    }

    public List<string> locations()
    {
        List<string> tempLocations = new List<string>();

        foreach (var item in databases)
        {
            var name = (string)item.localJson["name"];
            tempLocations.Add(name.Replace("_", " "));
        }

        return tempLocations;
    }

    public List<string> hotels(string location)
    {
        List<string> tempHotels = new List<string>();

        foreach (var item in databases)
        {
            if (item.local.Equals(location))
            {
                foreach (JsonData hotels in item.localJson["hotels"])
                {
                    var name = (string)hotels["name"];
                    tempHotels.Add(name.Replace("_", " "));
                }
            }
        }
        
        return tempHotels;
    }

    public class Database {
        public string local;
        public JsonData localJson;
        public Database(string newLocal, JsonData newLocalJson) {
            local = newLocal;
            localJson = newLocalJson;
        }
    }
}
