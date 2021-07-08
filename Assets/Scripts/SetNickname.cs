﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetNickname : MonoBehaviour
{
    public void ChangeName(string newName)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        localPlayer.NickName = newName;
    }
}
