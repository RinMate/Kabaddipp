using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Client : MonoBehaviour
{

    [SerializeField] private string _serverAddress;
    [SerializeField] private int _port;

    private WebCamController webCamController;
    private WebSocket ws;
    private bool connected = false;

    private void Awake()
    {
        if (webCamController == null)
        {
            webCamController = WebCamController.GetInstance();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        string ca = "ws://" + _serverAddress + ":" + _port.ToString();
        InitWebSocket(ca);
    }

    public void OnDestroy()
    {
        if(ws != null) ws.Close(); //Disconnect
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && connected == false)
        {
            InitWebSocket(IPShare.getURL());
        }
    }

    void InitWebSocket(string url)
    {
        ws = new WebSocket(url);

        //Add Events
        ws.OnOpen += (object sender, System.EventArgs e) => {
            Debug.Log("Connect to " + url);
            connected = true;
        };

        //On catch message event
        ws.OnMessage += (object sender, MessageEventArgs e) => {
            Debug.Log(e.Data);
            SkillDataJSON skillDataJSON = JsonUtility.FromJson<SkillDataJSON>(e.Data);
            if (skillDataJSON != null && skillDataJSON.type == "skill")
            {
                SkillData skillData = skillDataJSON.data;
                if (webCamController == null)
                {
                    webCamController = WebCamController.GetInstance();
                }
                webCamController.skillData = skillData;
            }
        };

        //On error event
        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        //On WebSocket close event
        ws.OnClose += (sender, e) => {
            Debug.Log("Disconnected Server");
            connected = false;
        };

        ws.Connect();
    }
}