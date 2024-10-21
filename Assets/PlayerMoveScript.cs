using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class PlayerMoveScript : NetworkBehaviour
{
    public float speed = 0.1f;
    public GameObject PlayerModel;
    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (PlayerModel.activeSelf == false)
            {
                //SetPosition();
                PlayerModel.SetActive(true);
            }
            if (hasAuthority)
            {
                MovePlayer();
            }
        }
    }
    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-15, 7));
    }
    public void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, y, 0f);
        transform.position += move * speed;
    }
}
