using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class localPlayerController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(playerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;

    private localhostGameUIManager manager;
    private localhostGameUIManager Manager
    {
        get
        {
            if (manager != null)
            { return manager; }

            return manager = localhostGameUIManager.singleton as localhostGameUIManager;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer) { this.Ready = newValue; }
        if (isClient) { LocalLobbyController.instance.UpdatePlayerList(); }
    }
    [Command]
    private void CMdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }
    //check Authority
    public void ChangeReady()
    {
        if (hasAuthority) { CMdSetPlayerReady(); }
    }

    public override void OnStartAuthority()
    {
        //CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        CmdSetPlayerName(localLobbyScene.instance.PlayerName + PlayerIdNumber);
        gameObject.name = "LocalGamePlayer";
        LocalLobbyController.instance.FindLocalPlayer();
        LocalLobbyController.instance.UpdateLobbyName();

    }


    public override void OnStartClient()
    {
        Debug.Log("OnStartClient()");
        Manager.GamePlayers.Add(this);
        LocalLobbyController.instance.UpdateLobbyName();
        LocalLobbyController.instance.UpdatePlayerList();

    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LocalLobbyController.instance.UpdatePlayerList();

    }
    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        Debug.Log("CmdSetPlayerName");
        this.playerNameUpdate(this.PlayerName, PlayerName);
    }

    public void playerNameUpdate(string OldValue, string NewValue)
    {
        //PlayerName = OldValue;

        Debug.Log(OldValue + "playerNameUpdate:" + NewValue);
        if (isServer)
        {
            this.PlayerName = NewValue;
            Debug.Log("POC_PNU:" + NewValue);
        }
        if (isClient)//Client
        {
            LocalLobbyController.instance.UpdatePlayerList();
        }
    }

    public void CanStartGame(string SceneName) 
    {
        if (hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }
    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
}
