using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVisualizer : MonoBehaviour
{
    public List<GameObject> listGameObject = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject gameObject in listGameObject)
        {
            string name = gameObject.name;
            Vector3 pos = gameObject.transform.position;
            Quaternion quaternion = gameObject.transform.rotation;
            Debug.Log("name: " + name + "pos: " + pos + ", rot: (" + quaternion.eulerAngles.x + ", " + quaternion.eulerAngles.y + ", " + quaternion.eulerAngles.z + ")");
        }
    }
}
