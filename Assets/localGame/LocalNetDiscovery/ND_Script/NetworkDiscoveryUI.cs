using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;

public class UDPserver
{
    public string pw = "";
    public string serverName = "";
}
[DisallowMultipleComponent]
[AddComponentMenu("MPGame/Network/Network Discovery UI")]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
[RequireComponent(typeof(NewNetworkDiscovery))]
public class NetworkDiscoveryUI : MonoBehaviour
{
    readonly Dictionary<long, DiscoveryResponse> discoveredServers = new Dictionary<long, DiscoveryResponse>();
    Vector2 scrollViewPos = Vector2.zero;
    public static NetworkDiscoveryUI instance;
    public NewNetworkDiscovery networkDiscovery;
    public InputField inputFieldPwSer;
    public InputField inputFieldSerName;
    public string pw = "";
    public string serverName="";
    public UDPserver serverdata=new UDPserver();
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NewNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound2, OnDiscoveredServer2);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
    /*
    void OnGUI()
    {
        if (NetworkManager.singleton == null)
            return;

        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
            DrawGUI();

        if (NetworkServer.active || NetworkClient.active)
            StopButtons();
    }*/
    public void FindServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }
    public void StartHost()
    {
        pw = inputFieldPwSer.text.ToString();
        serverName = inputFieldSerName.text.ToString();
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
    }
    public void StartServer()
    {/*
        networkDiscovery.StopDiscovery();
        discoveredServers.Clear();
        NetworkManager.singleton.StartServer();
        networkDiscovery.AdvertiseServer();*/
    }
    public Dropdown ipSelector;
    List<DiscoveryResponse> h1 = new List<DiscoveryResponse>();
    List<string> h = new List<string>();
    public GameObject a1;
    public GameObject a2;

    private void Awake()
    {
        instance = this;
    }
    public List<DiscoveryResponse> UpdateH1()
    {
        Debug.Log($"Discovered Servers [{discoveredServers.Count}]:");
        ipSelector.ClearOptions();
        h1.Clear();
        h.Clear();
        foreach (DiscoveryResponse info in discoveredServers.Values)
        {
            Debug.Log(info.EndPoint.Address.ToString());
            Debug.Log(info.serverId.ToString());
            h1.Add(info);
            h.Add(info.EndPoint.Address.ToString());
        }
        ipSelector.AddOptions(h);
        return h1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateH1();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ipSelector.options[ipSelector.value].text.ToString();
            Connect(h1[ipSelector.value]);
        }
        //---StopButtons---



    }

    public void ND_StopHost()
    {
        NetworkManager.singleton.StopHost();
        networkDiscovery.StopDiscovery();
    }
    public void ND_StopClient()
    {
        NetworkManager.singleton.StopClient();
        networkDiscovery.StopDiscovery();
    }
    public void ND_StopServer()
    {
        NetworkManager.singleton.StopServer();
        networkDiscovery.StopDiscovery();
    }
    void DrawGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 500));
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Find Servers"))
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }

        // LAN Host
        if (GUILayout.Button("Start Host"))
        {
            networkDiscovery.StopDiscovery();
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        // Dedicated server
        if (GUILayout.Button("Start Server"))
        {
            networkDiscovery.StopDiscovery();
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }

        GUILayout.EndHorizontal();

        // show list of found server

        GUILayout.Label($"Discovered Servers [{discoveredServers.Count}]:");

        // servers
        scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);

        Debug.Log(discoveredServers.Count);
        foreach (DiscoveryResponse info in discoveredServers.Values)
            if (GUILayout.Button(info.EndPoint.Address.ToString()))
                Connect(info);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void StopButtons()
    {
        GUILayout.BeginArea(new Rect(10, 40, 100, 25));

        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Host"))
            {
                NetworkManager.singleton.StopHost();
                networkDiscovery.StopDiscovery();
            }
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Client"))
            {
                NetworkManager.singleton.StopClient();
                networkDiscovery.StopDiscovery();
            }
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            if (GUILayout.Button("Stop Server"))
            {
                NetworkManager.singleton.StopServer();
                networkDiscovery.StopDiscovery();
            }
        }

        GUILayout.EndArea();
    }

    public void Connect(DiscoveryResponse info)
    {
        GameNetworkinngManager.instance.nD_Get.b1SetActive(true);
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer2(DiscoveryResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        //if (info.v == 0)
        {
            discoveredServers[info.serverId] = info;
            Debug.Log("OnDiscoveredServer2" + info.EndPoint);
        }
    }
}