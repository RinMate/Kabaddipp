using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using UnityEngine.XR;

public class Client : MonoBehaviour
{

    private WebCamController webCamController;
    private WebSocket ws;
    private bool streaming = false;

    private void Awake()
    {
        if (IPShare.port == 0)
        {
            //XRSettings.enabled = false;
            SceneManager.LoadScene("SettingScene");
        }

        if (webCamController == null)
        {
            webCamController = WebCamController.GetInstance();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        InitWebSocket(IPShare.getURL());
    }

    public void OnDestroy()
    {
        if(ws != null) ws.Close(); //Disconnect
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
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
            if (webCamController == null)
            {
                webCamController = WebCamController.GetInstance();
            }
            string regist_str = "{\"type\":\"register\",\"device\":\"hmd\"}";
            ws.Send(regist_str);
        };

        //On catch message event
        ws.OnMessage += (object sender, MessageEventArgs e) => {
            Debug.Log(e.Data);
            if (webCamController == null)
            {
                webCamController = WebCamController.GetInstance();
            }
            if (streaming)
            {
                WebCamJSON webCamJSON = JsonUtility.FromJson<WebCamJSON>(e.Data);
                if (webCamController.webcamColors == null)
                {
                    webCamController.webcamColors = new Color32[webCamJSON.data.Length];
                }
            }
            else
            {
                SkillDataJSON skillDataJSON = JsonUtility.FromJson<SkillDataJSON>(e.Data);
                if (skillDataJSON != null && skillDataJSON.type == "skill")
                {
                    SkillData skillData = skillDataJSON.data;
                    webCamController.skillData = skillData;
                    if (skillData.skill == Skills.Fix_Camera)
                    {
                        streaming = true; 
                        string webcam_init_str = "{\"type\":\"webcam_init\"," +
                                            "\"width\":" + webCamController.webcamTexture.width +
                                            "\"height\":" + webCamController.webcamTexture.height + "}";
                        ws.Send(webcam_init_str);
                    }
                }
            }
        };

        //On error event
        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        //On WebSocket close event
        ws.OnClose += (sender, e) => {
            Debug.Log("Disconnected Server");
        };

        ws.Connect();
    }
}