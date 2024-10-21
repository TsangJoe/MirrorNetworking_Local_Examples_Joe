using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugUIScript: MonoBehaviour
{
    public Text debugUI;
    public static DebugUIScript instance;
    private void Start()
    {
        instance = this;
    }
    private void LateUpdate()
    {
        if (LobbyScenesUI.instance != null)
        {
            LobbyScenesUI.instance.StatusLabelsUI();
        }
    }
}
