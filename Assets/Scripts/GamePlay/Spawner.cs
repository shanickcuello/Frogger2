using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviourPun
{
    [SerializeField] List<GameObject> spawnsPositions;

    public Vector3 GetSpawnPos(int index) => spawnsPositions[index].transform.position;
}
