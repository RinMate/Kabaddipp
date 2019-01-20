using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public InputField if_ip1, if_ip2, if_ip3, if_ip4, if_port;
    public Button buttonOK;

    // Start is called before the first frame update
    void Start()
    {
        if_ip1.onValueChanged.AddListener(TextValueCheck);
        if_ip2.onValueChanged.AddListener(TextValueCheck);
        if_ip3.onValueChanged.AddListener(TextValueCheck);
        if_ip4.onValueChanged.AddListener(TextValueCheck);
        if_port.onValueChanged.AddListener(TextValueCheck);
        buttonOK.onClick.AddListener(MoveMainScene);
    }

    void MoveMainScene()
    {
        IPShare.address = if_ip1.text + '.' + if_ip2.text + '.' + if_ip3.text + '.' + if_ip4.text;
        IPShare.port = int.Parse(if_port.text);
        XRSettings.enabled = !XRSettings.enabled;
        SceneManager.LoadScene("MainScene");
    }


    void TextValueCheck(string arg0)
    {
        if(if_ip1.text.Length > 0 && if_ip2.text.Length > 0 && if_ip3.text.Length > 0 && 
            if_ip4.text.Length > 0 && if_port.text.Length > 0)
        {
            buttonOK.interactable = true;
        }
        else
        {
            buttonOK.interactable = false;
        }
    }

}
