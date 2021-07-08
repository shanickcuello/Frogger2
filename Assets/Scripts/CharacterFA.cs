﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class CharacterFA : MonoBehaviourPun
{
    Player playerId;
    public bool alive;
    public float speedMovement;
    public Vector3 spawnPos;
    WinTrigger winManager;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Move(EJumpDir dir)
    {
        switch (dir)
        {
            case EJumpDir.UP:
                transform.Translate(Vector3.up * Time.deltaTime * speedMovement);
                break;
            case EJumpDir.DOWN:
                transform.Translate(Vector3.down * Time.deltaTime * speedMovement);
                break;
            case EJumpDir.RIGHT:
                transform.Translate(Vector3.right * Time.deltaTime * speedMovement);
                break;
            case EJumpDir.LEFT:
                transform.Translate(Vector3.left * Time.deltaTime * speedMovement);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        Debug.LogWarning("Mi player es: " + playerId);
    }

    public CharacterFA SetInitialParameters(Player localPlayer)
    {
        playerId = localPlayer;
        photonView.RPC("SetLocalParams", playerId, playerId);
        return this;
    }

    [PunRPC]
    void SetLocalParams(Player playerId)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        this.playerId = playerId;
    }


    [PunRPC]
    void DisconnectOwner()
    {
        PhotonNetwork.Disconnect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Car>())
            MyServer.Instance.RequestGetBack(playerId);

        if (collision.gameObject.GetComponent<WinTrigger>())
        {
            MyServer.Instance.RequestIWin(playerId);
            Debug.Log("requestee ganar" + " soy: " + playerId);
        }
    }

    internal void GetBack(Vector3 backPos)
    {
        transform.position = backPos;
    }

    internal void Win(Player player)
    {
        Debug.Log("Entre en el metodo win");
        winManager = FindObjectOfType<WinTrigger>();
        if (player == playerId)
        {
            Debug.Log( "if (" + player + " == + " + playerId + ")");
            winManager.IWin();
            Debug.Log("Yo gane");
        }
        else
        {
            winManager.ILoose();
            Debug.Log("Yo perdi");
        }
        Time.timeScale = 0;
    }
}
