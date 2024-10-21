using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkinngManager : GameNetworkinngManagerBase

{   public static GameNetworkinngManager instance;
    public NetworkDiscoveryUI discoveryUI;
    public LocalLobbiesListManager LLLM;
    public ND_Get nD_Get;
    public ND_Stop ND_Stop;
    private void Awake()
    {
        instance = this;
    }
}
