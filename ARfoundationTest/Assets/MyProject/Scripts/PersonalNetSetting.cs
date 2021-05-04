using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


#pragma warning disable 618
public class PersonalNetSetting : NetworkManager
{

    public override void OnClientConnect(NetworkConnection conn)
    {
        
        base.OnClientConnect(conn);
        if (conn.lastError == NetworkError.Ok)
        {
            Debug.Log("Successfully connected to server.");
        }
        else
        {
            Debug.LogError("Connected to server with error: " + conn.lastError);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        if (conn.lastError == NetworkError.Ok)
        {
            Debug.Log("Successfully disconnected from the server.");
        }
        else
        {
            Debug.LogError("Disconnected from the server with error: " + conn.lastError);
        }
    }

    private void Update()
    {
        
    }


}
#pragma warning restore 618