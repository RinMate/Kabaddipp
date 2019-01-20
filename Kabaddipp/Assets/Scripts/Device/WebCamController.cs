using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamController : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public int fps = 60;
    public SkillData skillData;

    private static WebCamController _singleInstance;

    private Texture2D texture;
    private WebCamTexture webcamTexture;
    private Color32[] colors = null;
    private Skills prevSkill;

    IEnumerator Init()
    {
        while (true)
        {
            if (webcamTexture.width > 16 && webcamTexture.height > 16)
            {
                colors = new Color32[webcamTexture.width * webcamTexture.height];
                texture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
                GetComponent<Renderer>().material.mainTexture = texture;
                break;
            }
            yield return null;
        }
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
        if (this.skillData == null || (this.skillData.skill != Skills.Color_BW) &&
            (this.skillData.skill != Skills.Color_Nega) && (this.skillData.skill != Skills.Fix_Camera))
        {
            if (colors != null)
            {
                webcamTexture.GetPixels32(colors);
                texture.SetPixels32(colors);
                texture.Apply();
            }
        }
        if (this.skillData.skill != Skills.None && prevSkill == Skills.None)
        {
            prevSkill = this.skillData.skill;
            switch (this.skillData.skill)
            {
                case Skills.Flip_Horizontal:
                    IEnumerator fh = FlipHorizontal(this.skillData.time);
                    StartCoroutine(fh);
                    break;
                case Skills.Flip_Vertical:
                    IEnumerator fv = FlipVertical(this.skillData.time);
                    StartCoroutine(fv);
                    break;
                case Skills.Flip_HandV:
                    IEnumerator fhv = FlipHandV(this.skillData.time);
                    StartCoroutine(fhv);
                    break;
                case Skills.Color_BW:
                    IEnumerator cbw = ColorBW(this.skillData.time);
                    StartCoroutine(cbw);
                    break;
                case Skills.Color_Nega:
                    IEnumerator cng = ColorNega(this.skillData.time);
                    StartCoroutine(cng);
                    break;
                case Skills.Fix_Camera:
                    break;
            }
        }
    }

    IEnumerator FlipHorizontal(int time)
    {
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
        // 指定した秒数維持
        for (int i = 0; i < 60*time; i++)
        {
            yield return null;
        }
        scale.x *= -1;
        this.transform.localScale = scale;
        skillReset();
    }
    IEnumerator FlipVertical(int time)
    {
        Vector3 scale = this.transform.localScale;
        scale.z *= -1;
        this.transform.localScale = scale;
        // 指定した秒数維持
        for (int i = 0; i < 60 * time; i++)
        {
            yield return null;
        }
        scale.z *= -1;
        this.transform.localScale = scale;
        skillReset();
    }
    IEnumerator FlipHandV(int time)
    {
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        scale.z *= -1;
        this.transform.localScale = scale;
        // 指定した秒数維持
        for (int i = 0; i < 60 * time; i++)
        {
            yield return null;
        }
        scale.x *= -1;
        scale.z *= -1;
        this.transform.localScale = scale;
        skillReset();
    }
    IEnumerator ColorBW(int time)
    {
        // 指定した秒数維持
        for (int i = 0; i < 60 * time; i++)
        {
            if (colors != null)
            {
                webcamTexture.GetPixels32(colors);

                int width = webcamTexture.width;
                int height = webcamTexture.height;
                Color32 rc = new Color32();

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color32 c = colors[x + y * width];
                        byte gray = (byte)(0.1f * c.r + 0.7f * c.g + 0.2f * c.b);
                        rc.r = rc.g = rc.b = gray;
                        colors[x + y * width] = rc;
                    }
                }

                texture.SetPixels32(colors);
                texture.Apply();
            }
            yield return null;
        }
        skillReset();
    }
    IEnumerator ColorNega(int time)
    {
        // 指定した秒数維持
        for (int i = 0; i < 60 * time; i++)
        {
            if (colors != null)
            {
                webcamTexture.GetPixels32(colors);

                int width = webcamTexture.width;
                int height = webcamTexture.height;
                Color32 rc = new Color32();

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color32 c = colors[x + y * width];
                        rc.r = (byte)(255 - c.r);
                        rc.g = (byte)(255 - c.g);
                        rc.b = (byte)(255 - c.b);
                        colors[x + y * width] = rc;
                    }
                }

                texture.SetPixels32(colors);
                texture.Apply();
            }
            yield return null;
        }
        skillReset();
    }

    private void skillReset()
    {
        this.skillData.skill = Skills.None;
        this.prevSkill = Skills.None;
    }
}
