using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LocalLobbiesListManager : GameNetworkinngManagerBase
{

    public static LocalLobbiesListManager instance;
    //Lobbies List Variables
    public GameObject lobbiesMenu;
    public GameObject lobbyDataItemPrefab;
    public GameObject LobbylContent;
    public GameObject PanelLobby;

    public GameObject pwUI;
    public InputField InputFieldpw;
    public GameObject lobbiesButton, hostButton;

    public List<GameObject> listOfLobbies = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void GetListOfLobbies()
    {
        UpdateServersCount();
        lobbiesButton.SetActive(false);
        //hostButton.SetActive(false);
        lobbiesMenu.SetActive(true);
    }
    public void UpdateServersCount()
    {
        if (NetworkDiscoveryUI.instance == null)
        {
            Debug.LogError("NetworkDiscoveryUI.instance == null");
        }
        UpdateDRlist(NetworkDiscoveryUI.instance.UpdateH1());
    }

    public void UpdateDRlist(List<DiscoveryResponse> DR)
    {
        h1 = DR;
        if (listOfLobbies.Count > 0)
        {
            DestroyLobbies();
        }
        DisaplayLobbiesDR();
    }
    List<DiscoveryResponse> h1 = new List<DiscoveryResponse>();
    List<string> h = new List<string>();

    int h1Count;
    public void DisaplayLobbiesDR()
    {
        Debug.Log(h1.Count);
        for (int i = 0; i < h1.Count; i++)
        {
            GameObject createdItem = Instantiate(lobbyDataItemPrefab);

            //createdItem.GetComponent<LocalLobbyDataEntry>().lobbyID = h1[i].serverId;

            //createdItem.GetComponent<LocalLobbyDataEntry>().lobbyName = "";
            //Debug.LogFormat("{0},{1}", lobbyIDs[i].m_SteamID, SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "AppID"));

            if (h1[i].serverName != "")
            {
                createdItem.GetComponent<LocalLobbyDataEntry>().lobbyName = h1[i].serverName;
            }
            else
            {
                createdItem.GetComponent<LocalLobbyDataEntry>().lobbyName = h1[i].EndPoint.ToString();
            }
            createdItem.GetComponent<LocalLobbyDataEntry>().sid = h1[i].serverId; 
            createdItem.GetComponent<LocalLobbyDataEntry>().SetLobbyData();
            createdItem.transform.SetParent(LobbylContent.transform);
            createdItem.transform.localScale = Vector3.one;
            listOfLobbies.Add(createdItem);
        }
    }
    public void DestroyLobbies()
    {
        foreach (GameObject lobbyItem in listOfLobbies)
        {
            Destroy(lobbyItem);
        }
        listOfLobbies.Clear();
        GameNetworkinngManager.instance.nD_Get.b1SetActive(true);
        //hostButton.SetActive(true);
        pwUI.SetActive(false);
        lobbiesButton.SetActive(true);
        lobbiesMenu.SetActive(false);

    }

    DiscoveryResponse temp;
    public long sid;
    public void chekSid(long sid)
    {
        temp=new DiscoveryResponse();
        foreach (DiscoveryResponse info in h1)
        {
            Debug.Log(info.EndPoint.Address.ToString());
            Debug.Log(info.serverId.ToString());
            if (sid == info.serverId)
            {
                temp = info;
                if (info.pw != "")
                {
                    Debug.LogWarning("pw?");
                    pwUI.SetActive(true);
                    return;
                }
                else if(info.pw=="")
                {
                    NetworkDiscoveryUI.instance.Connect(temp);
                    SetActiveF();
                }
            }
        }
    }
    public void pwUIIn()
    {
        if (temp.pw == InputFieldpw.text.ToString())
        {
            Debug.Log("pw=uipw");
            NetworkDiscoveryUI.instance.Connect(temp);

            SetActiveF();
            return;
        }
        else
        {
            Debug.LogWarning("[ERROR]Pw er!!");
        }

    }
    void SetActiveF()
    {
        pwUI.SetActive(false);
        lobbiesButton.SetActive(false);
        //hostButton.SetActive(false);
        lobbiesMenu.SetActive(false);
        PanelLobby.SetActive(false);
    }

}
