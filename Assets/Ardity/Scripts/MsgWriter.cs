using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class MsgWriter : MonoBehaviour
{


    //private OVRSkeleton skeleton;
    // Start is called before the first frame update
    //private void Start()
    //{
    //    skeleton = GetComponent<OVRSkeleton>();
    //}
    void OnMessageArrived(string msg)
    {
    }

    // Update is called once per frame
    void OnConnectionEvent(bool success)
    {
        if (success)
        {
            Debug.Log("Success");
        }
    }
}
