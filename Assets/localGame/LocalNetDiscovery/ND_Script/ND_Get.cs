using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class ND_Get : GameNetworkinngManagerBase
{
    public Button Btu_FindServers;
    public Button Btu_StartHost;
    public Button Btu_StartServer;
    public GameObject BtuStarMenu;
    [SerializeField] bool bSetActive=false;
    private void Awake()
    {
        SetActive(true);
    }
    private void LateUpdate()
    {
        if (bSetActive&&!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
        {
            LocalLobbiesListManager.instance.PanelLobby.SetActive(true);
            LocalLobbiesListManager.instance.lobbiesButton.SetActive(true);
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }
    public void SetActive(bool b)
    {
        BtuStarMenu.SetActive(b);
    }
    public void b1SetActive(bool b)
    {
        bSetActive=b;
    }
}
