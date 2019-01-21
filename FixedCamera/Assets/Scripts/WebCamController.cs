using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamController : MonoBehaviour
{
    public int width = 960;
    public int height = 540;
    public int fps = 60;

	public Status status = Status.Idling;

    private WebCamTexture webcamTexture;
    public Color32[] colors = null;

    private static WebCamController _singleInstance;

    public enum Status{
		Idling,
		Sync
	}

    private void Awake()
    {
        if (_singleInstance == null)
        {
            _singleInstance = this;
        }
    }

    public static WebCamController GetInstance()
    {
        return _singleInstance;
	}

	IEnumerator Init()
	{
		while (true)
		{
			if (webcamTexture.width > 16 && webcamTexture.height > 16)
			{
				colors = new Color32[webcamTexture.width * webcamTexture.height];
				break;
			}
			yield return null;
		}
	}

    // Use this for initialization
    void Start()
    {
        if (_singleInstance == null)
        {
            _singleInstance = this;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        webcamTexture.Play();

        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {
		if (this.status == Status.Sync)
        {
            if (colors != null)
            {
                webcamTexture.GetPixels32(colors);
            }
        }
    }
}
