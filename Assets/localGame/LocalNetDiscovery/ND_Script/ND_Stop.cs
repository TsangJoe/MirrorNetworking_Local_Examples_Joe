using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class ND_Stop : GameNetworkinngManagerBase
{
    public Button Btu_StopClient;
    public Button Btu_StopHost;
    public Button Btu_StopServer;
    // Start is called before the first frame update

    // Update is called once per frame
    private void LateUpdate()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Btu_StopHost.gameObject.SetActive(true);
            Btu_StopClient.gameObject.SetActive(false);
            Btu_StopServer.gameObject.SetActive(false);
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            Btu_StopHost.gameObject.SetActive(false);
            Btu_StopClient.gameObject.SetActive(true);
            Btu_StopServer.gameObject.SetActive(false);
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            Btu_StopHost.gameObject.SetActive(false);
            Btu_StopClient.gameObject.SetActive(false);
            Btu_StopServer.gameObject.SetActive(true);
        }
        else if (!NetworkServer.active && !NetworkClient.isConnected)
        {
            Btu_StopHost.gameObject.SetActive(false);
            Btu_StopClient.gameObject.SetActive(false);
            Btu_StopServer.gameObject.SetActive(false);
        }
    }
}
