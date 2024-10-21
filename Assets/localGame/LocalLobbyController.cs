using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class LocalLobbyController : MonoBehaviour
{
    public static LocalLobbyController instance;

    //UI Elements
    public Text LobbyNameText;
    //Player Date
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    //Other Date
    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<LocalPlayerListItem> PlayerListItems = new List<LocalPlayerListItem>();
    public localPlayerController localPlayerController;

    //Ready
    public Button StartGameButton;
    public Text ReadyButtonText;

    //Manager NetWork
    private localhostGameUIManager manager;


    private localhostGameUIManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = localhostGameUIManager.singleton as localhostGameUIManager;
        }
    }

    bool bStop = false;
    private void OnApplicationQuit()
    {
        StopNetwork();
        bStop = true;
        Debug.Log("StopGame");
    }
    private void OnDestroy()
    {
        bStop = true;
        Debug.Log("StopGame");
    }
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void ReadyPlayer()
    {
        localPlayerController.ChangeReady();
    }
    public void StopNetwork()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
            Debug.Log("Net StopClient()");
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            manager.StopServer(); 
            Debug.Log("Net StopServer()");
        }
    }
    public void UpdateButton()
    {
        if (localPlayerController.Ready)
        {
            ReadyButtonText.text = "Unready";
        }
        else
        {
            ReadyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady()
    {
        bool AllReady = false;
        foreach (localPlayerController player in Manager.GamePlayers)
        {
            if (player.Ready)
            {
                AllReady = true;
            }
            else
            {
                AllReady = false;
                break;
            }
        }
        if (AllReady)
        {
            if (localPlayerController.PlayerIdNumber == 1)
            {
                StartGameButton.interactable = true;
            }
            else
            {
                StartGameButton.interactable = false;
            }
        }
        else
        {
            StartGameButton.interactable = false;
        }
    }
    //---
    public void UpdateLobbyName()
    {
        //CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;// ·í«e¤jÆUID
        //LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
        LobbyNameText.text = manager.networkAddress;
    }

    public void UpdatePlayerList()
    {
        if (bStop)
        {
            return;
        }
        if (!PlayerItemCreated) { CreateHostPlayerItem(); }//Host
        if (PlayerListItems.Count < Manager.GamePlayers.Count) { CreateClientPlayerItem(); }
        if (PlayerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem(); }
        if (PlayerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem(); }
    }

    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = LocalPlayerObject.GetComponent <localPlayerController>();

    }

    public void CreateHostPlayerItem()
    {
        foreach (localPlayerController player in Manager.GamePlayers)
        {
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            LocalPlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<LocalPlayerListItem>();

            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionID = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.bReady = player.Ready;
            NewPlayerItemScript.SetPlayerValues();

            NewPlayerItemScript.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItemScript.transform.localScale = Vector3.one;

            PlayerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (localPlayerController player in Manager.GamePlayers)
        {
            if (!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID))
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                LocalPlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<LocalPlayerListItem>();

                NewPlayerItemScript.PlayerName = player.PlayerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.bReady = player.Ready;
                NewPlayerItemScript.SetPlayerValues();

                NewPlayerItemScript.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItemScript.transform.localScale = Vector3.one;

                PlayerListItems.Add(NewPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (localPlayerController player in Manager.GamePlayers)
        {
            Debug.Log("UpdatePlayerItem"+player.PlayerName);
            foreach (LocalPlayerListItem playerListItemScript in PlayerListItems)
            {
                if (playerListItemScript.ConnectionID == player.ConnectionID)
                {
                    playerListItemScript.PlayerName = player.PlayerName;
                    playerListItemScript.bReady = player.Ready;
                    Debug.Log("LC_UPI:" + player.PlayerName);
                    playerListItemScript.SetPlayerValues();
                    if (player == localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<LocalPlayerListItem> playerListItemToRemove = new List<LocalPlayerListItem>();
        foreach (LocalPlayerListItem playerListItem in PlayerListItems)
        {
            if (!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.ConnectionID))
            {
                playerListItemToRemove.Add(playerListItem);
            }
        }
        if (playerListItemToRemove.Count > 0)
        {
            foreach (LocalPlayerListItem playerlistItemToRemove in playerListItemToRemove)
            {
                if (playerListItemToRemove == null)
                {
                    return;
                }
                GameObject ObjectToRemove = playerlistItemToRemove.gameObject;
                PlayerListItems.Remove(playerlistItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }

    }


    public void StartGame(string SceneName)
    {
        localPlayerController.CanStartGame(SceneName);
    }
}
