using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WebSocketSharp;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
	public InputField if_ip, if_port;
	public Button btnConnect, btnStart, btnStop;

	private WebCamController webCamController;
	private WebSocket ws;
    private WebCamData webCamData;

    // Start is called before the first frame update
    void Start()
	{
		if (webCamController == null)
		{
			webCamController = WebCamController.GetInstance();
		}

		if_ip.onValueChanged.AddListener(TextValueCheck);
		if_port.onValueChanged.AddListener(TextValueCheck);
		btnConnect.onClick.AddListener(OnConnect);
		btnStart.onClick.AddListener(beginSend);
	}

	void TextValueCheck(string arg0)
	{
		if(if_ip.text.Length > 0 && if_port.text.Length > 0)
		{
			btnConnect.interactable = true;
		}
		else
		{
			btnConnect.interactable = false;
		}
	}

	void OnConnect()
	{
		string url = "ws://" + if_ip.text + ":" + if_port.text;
		InitWebSocket (url);
	}

	void beginSend()
	{
		if (webCamController == null)
		{
			webCamController = WebCamController.GetInstance();
		}
		webCamController.status = WebCamController.Status.Sync;
        IEnumerator sendWebCam = SendWebCam();
        StartCoroutine(sendWebCam);
        btnStart.interactable = false;
	}

	IEnumerator SendWebCam()
	{
		for (int i = 0; i < 60 * 5; i++)
		{
            if (webCamController.colors != null)
            {
                byte[] bytes = Color32ArrayToByteArray(webCamController.colors);
                if (webCamData == null)
                {
                    webCamData = new WebCamData("webcam", bytes);
                }
                else
                {
                    webCamData.data = bytes;
                }
                string json = JsonUtility.ToJson(webCamData);
                ws.Send(json);
            }
            yield return null;
		}
	}

	void InitWebSocket(string url)
	{
		ws = new WebSocket(url);

		//Add Events
		ws.OnOpen += (object sender, System.EventArgs e) => {
			Debug.Log("Connect to " + url);
			btnStart.interactable = true;
			string str = "{\"type\":\"register\", \"device\":\"webcam\"}";
			ws.Send(str);
		};

		//On catch message event
		ws.OnMessage += (object sender, MessageEventArgs e) => {
			Debug.Log(e.Data);
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

	public void OnDestroy()
	{
		if(ws != null) ws.Close(); //Disconnect
	}

    private static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }
}
