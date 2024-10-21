using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class localLobbyScene : MonoBehaviour
{
    public static localLobbyScene instance;

    //Callbacks;

    //Variables
    public ulong CurrentLobbyID;// serv ID
    private const string HostAddressKey = "HostAddress";
    private localhostGameUIManager CNManager;
    public string PlayerName;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        CNManager = GetComponent<localhostGameUIManager>();
    }
    public void HostLobby()
    {   if (LocalMainSceneUI.instance.inputField.text.ToString() == "")
        {
            PlayerName = "Er";
        }
        else
        {
            PlayerName = LocalMainSceneUI.instance.inputField.text.ToString();
        }
        if (NetworkServer.active)
        {
            Debug.Log("HostLobby NetworkServer.active:T");
            return;
        }
        Debug.Log("HostLobby");
        CNManager.StartHost();
    }
    public void ClientLobby()
    {
        PlayerName = LocalMainSceneUI.instance.inputField.text.ToString();
        if (NetworkServer.active)
        {
            Debug.Log("HostLobby NetworkServer.active:T");
            return;
        }
        Debug.Log("HostLobby");
        CNManager.StartClient();
    }
    public void SteamMod()
    {
       SceneManager.LoadScene("Steam34");
        Destroy(CNManager.gameObject);
    }
}
