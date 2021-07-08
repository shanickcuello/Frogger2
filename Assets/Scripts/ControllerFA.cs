using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControllerFA : MonoBehaviourPun
{
    Player playerId;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        playerId = PhotonNetwork.LocalPlayer;

        StartCoroutine(SendPackages());
    }

    IEnumerator SendPackages()
    {
        while (true)
        {            
            yield return new WaitForSeconds(1 / MyServer.Instance.PackagesPerSecond);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            MyServer.Instance.RequestMove(playerId, EJumpDir.UP);

        if (Input.GetKeyUp(KeyCode.S))
            MyServer.Instance.RequestMove(playerId, EJumpDir.DOWN);

        if (Input.GetKeyUp(KeyCode.A))
            MyServer.Instance.RequestMove(playerId, EJumpDir.LEFT);

        if (Input.GetKeyUp(KeyCode.D))
            MyServer.Instance.RequestMove(playerId, EJumpDir.RIGHT);
    }
}

public enum EJumpDir
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}
