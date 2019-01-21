using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WebCamData
{
    public string type;
    public byte[] data;

    public WebCamData(string type, byte[] data)
    {
        this.type = type;
        this.data = data;
    }
}
