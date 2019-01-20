using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NegateTracking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = -InputTracking.GetLocalPosition(XRNode.CenterEye); 
        transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.CenterEye));
    }
}
