using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class localhostGameUIManager : NetworkManager
{
    [SerializeField] private localPlayerController localGamePlayerPrefab;
    public List<localPlayerController> GamePlayers { get; } = new List<localPlayerController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "LobbyScenes")
        {
            localPlayerController localGamePlayerInstance = Instantiate(localGamePlayerPrefab);
            localGamePlayerInstance.ConnectionID = conn.connectionId;
            localGamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            NetworkServer.AddPlayerForConnection(conn, localGamePlayerInstance.gameObject);
        }
        else
        {
            Debug.LogError("[ERROR] LocalhostGameUIManager:OnServerAddPlayer: False");
        }
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
