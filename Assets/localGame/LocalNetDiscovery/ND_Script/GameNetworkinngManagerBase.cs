using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNetworkinngManagerBase : MonoBehaviour
{
    public void ButtonSetActive(Button go, bool b)
    {
        go.gameObject.SetActive(b);
    }
    public void GameObjectSetActive(GameObject go, bool b)
    {
        go.SetActive(b);
    }
}
