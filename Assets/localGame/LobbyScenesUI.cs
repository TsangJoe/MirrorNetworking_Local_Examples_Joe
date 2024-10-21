using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class LobbyScenesUI : MonoBehaviour
{
    NetworkManager manager;

    public static LobbyScenesUI instance;
    public int offsetX;
    public int offsetY;
   public void Awake()
    {
        instance = this;
        manager = GetComponent<NetworkManager>();
    }

    public void StartLobbyHost()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartButtonsUI();
            Debug.Log("StatusLabels ok");
        }
        else
        {
            StatusLabelsUI();

        }
    }
    void StartButtonsUI()
    {
        if (!NetworkClient.active)
        {
            Debug.Log("StartButtonsUI");
            // Server + Client
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                    manager.StartHost();// StartHost
                    Debug.Log("manager.StartHost();");
            }

            // Client + IP
            //GUILayout.BeginHorizontal();
            /* if (GUILayout.Button("Client"))
             {
                manager.StartClient(); 
                 Debug.Log("manager.StartHost();");
             }
             // This updates networkAddress every frame from the TextField
             manager.networkAddress = GUILayout.TextField(manager.networkAddress);
             GUILayout.EndHorizontal();*/
        }
        else
        {
            // Connecting
            /*GUILayout.Label($"Connecting to {manager.networkAddress}..");
            if (GUILayout.Button("Cancel Connection Attempt"))
            {
                
            }*/
            manager.StopClient();
            Debug.LogWarning("manager.StopClient();");
        }
    }

  public  void StatusLabelsUI()
    {
        // host mode
        // display separately because this always confused people:
        //   Server: ...
        //   Client: ...
        if (NetworkServer.active && NetworkClient.active)
        {
            DebugUIScript.instance.debugUI.text=$"<b>Host</b>: running via {Transport.active}";
        }
        // server only
        else if (NetworkServer.active)
        {
            DebugUIScript.instance.debugUI.text = ($"<b>Server</b>: running via {Transport.active}");
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            DebugUIScript.instance.debugUI.text = ($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.active}");
        }
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 250, 9999), new GUIStyle());
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartButtons();//ok
        }
        else
        {
            StatusLabels();
        }

        // client ready
        if (NetworkClient.isConnected && !NetworkClient.ready)
        {
            if (GUILayout.Button("Client Ready"))
            {
                NetworkClient.Ready();
                if (NetworkClient.localPlayer == null)
                {
                    NetworkClient.AddPlayer();
                }
            }
        }

        StopButtons();

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (!NetworkClient.active)
        {
            // Server + Client
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                if (GUILayout.Button("Host (Server + Client)"))
                {
                    manager.StartHost();
                }
            }

            // Client + IP
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Client"))
            {
                Debug.LogError("GUI");
                manager.StartClient();
            }
            // This updates networkAddress every frame from the TextField
            manager.networkAddress = GUILayout.TextField(manager.networkAddress);
            GUILayout.EndHorizontal();

            // Server Only
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // cant be a server in webgl build
                GUILayout.Box("(  WebGL cannot be server  )");
            }
            else
            {
                if (GUILayout.Button("Server Only")) manager.StartServer();
            }
        }
        else
        {
            // Connecting
            GUILayout.Label($"Connecting to {manager.networkAddress}..");
            if (GUILayout.Button("Cancel Connection Attempt"))
            {
                manager.StopClient();
            }
        }
    }

    void StatusLabels()
    {
        // host mode
        // display separately because this always confused people:
        //   Server: ...
        //   Client: ...
        if (NetworkServer.active && NetworkClient.active)
        {
            GUILayout.Label($"<b>Host</b>: running via {Transport.active}");
        }
        // server only
        else if (NetworkServer.active)
        {
            GUILayout.Label($"<b>Server</b>: running via {Transport.active}");
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.active}");
        }
    }

    void StopButtons()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stop Host"))
            {
                manager.StopHost();
            }
            if (GUILayout.Button("Stop Client"))
            {
                manager.StopClient();
            }
            GUILayout.EndHorizontal();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Client"))
            {
                manager.StopClient();
            }
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            if (GUILayout.Button("Stop Server"))
            {
                manager.StopServer();
            }
        }
    }
}
