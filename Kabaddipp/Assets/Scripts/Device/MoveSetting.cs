using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class MoveSetting : MonoBehaviour
{
    public Button buttonSetting;

    // Start is called before the first frame update
    void Start()
    {
        buttonSetting.onClick.AddListener(MoveSettingScene);
    }

    void MoveSettingScene()
    {
        //XRSettings.enabled = false;
        SceneManager.LoadScene("SettingScene");
    }


    // Update is called once per frame
    void Update()
    {
    }
}
