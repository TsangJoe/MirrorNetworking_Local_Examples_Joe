using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalLobbyDataEntry : MonoBehaviour
{

    //Data
    public long sid;
    public string lobbyName;
    public Text lobbyNameText;
    public string GameID;
    public void SetLobbyData()
    {
        if (lobbyName == "")
        {
            lobbyNameText.text = "Empty";
        }
        else
        {
            lobbyNameText.text = lobbyName;
        }
    }

    public void JoinLobby()
    {
        LocalLobbiesListManager.instance.chekSid(sid);
        //SteamLobby.instance.JoinLobby(lobbyID);
    }
}
