using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOBridge : MonoBehaviour
{
    public static string[] TransformReceivedData(string serialMessage)
    {
        return serialMessage.Split(',');
    }
}
