using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class MyServer : MonoBehaviourPun
{
    #region CodigoProfe
    public static MyServer Instance;

    Player server;

    public CharacterFA characterPrefab;

    Dictionary<Player, CharacterFA> _dicModels = new Dictionary<Player, CharacterFA>();

    Dictionary<Player, CharacterViewFA> _dicViews = new Dictionary<Player, CharacterViewFA>();
    public int PackagesPerSecond { get; private set; }
    #endregion 

    public Spawner spawnPos;


    private void Start()
    {
        Singleton();
    }

    private void Singleton()
    {
        DontDestroyOnLoad(this.gameObject);


        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, 1);
            }
        }
    }

    [PunRPC]
    void SetServer(Player serverPlayer, int sceneIndex = 1)
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        server = serverPlayer;

        PackagesPerSecond = 60;

        PhotonNetwork.LoadLevel(sceneIndex);

        var playerLocal = PhotonNetwork.LocalPlayer;

        if (playerLocal != server)
        {
            photonView.RPC("AddPlayer", server, playerLocal);
        }
    }

    [PunRPC]
    void AddPlayer(Player player)
    {
        StartCoroutine(WaitForLevel(player));
    }

    IEnumerator WaitForLevel(Player player)
    {
        while (PhotonNetwork.LevelLoadingProgress > 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        spawnPos = FindObjectOfType<Spawner>();

        Vector3 playerPos = spawnPos.GetSpawnPos(PhotonNetwork.PlayerList.Length);
        playerPos.z = 0;

        CharacterFA newCharacter = PhotonNetwork.Instantiate(characterPrefab.name, playerPos, Quaternion.identity)
                                   .GetComponent<CharacterFA>().SetInitialParameters(player);

        _dicModels.Add(player, newCharacter);
        _dicViews.Add(player, newCharacter.GetComponent<CharacterViewFA>());
    }


    /* REQUESTS QUE LE LLEGAN AL SERVER AVATAR */
    internal void RequestIWin(Player playerId)
    {
        photonView.RPC("Win", server, playerId);
        Debug.Log("Soy server me piudieron ganar");

    }

    internal void RequestGetBack(Player playerId)
    {
        photonView.RPC("GetBack", server, playerId);
    }

    public void RequestMove(Player player, EJumpDir dir)
    {
        photonView.RPC("Move", server, player, dir);
    }

    public void RequestShoot(Player player)
    {
        photonView.RPC("Shoot", server, player);
    }
    
    public void PlayerDisconnect(Player player)
    {
        PhotonNetwork.Destroy(_dicModels[player].gameObject);
        _dicModels.Remove(player);
        _dicViews.Remove(player);
    }

    /* FUNCIONES DEL SERVER ORIGINAL QUE LE LLEGAN DEL AVATAR */
    [PunRPC]
    void Move(Player player, EJumpDir dir)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Move(dir);
        }
    }

    [PunRPC]
    void GetBack(Player player)
    {
        if (_dicModels.ContainsKey(player))
        {
            int index = GetIndexFromDic(player);
            Vector3 backPos = spawnPos.GetSpawnPos(index);
            _dicModels[player].GetBack(backPos);

        }
    }

    [PunRPC]
    void Win(Player player)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Win(player);
            Debug.Log("Le digo a mis PUNgas que gane");

        }
    }

    int GetIndexFromDic(Player playerId)
    {
        int index = 0;
        foreach (var item in _dicModels)
        {
            if (item.Key == playerId)
                return index;
            index++;
        }
        return 0;
        Debug.LogError("NO ENCONTRE EL INDEX DEL PLAYER");
    }


    //[PunRPC]
    //void Shoot(Player player)
    //{
    //    if (_dicModels.ContainsKey(player))
    //    {
    //        _dicModels[player].Shoot();
    //    }
    //}
}
