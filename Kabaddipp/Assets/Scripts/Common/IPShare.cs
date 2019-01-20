using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IPShare
{
    public static string address = "";
    public static int port = 0;

    public static string getURL()
    {
        return "ws://" + address + ":" + port.ToString();
    }
}
