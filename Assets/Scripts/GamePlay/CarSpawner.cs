using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviourPun
{
    public float timer, minSpawnDelay, maxSpawnDelay;
    public List<GameObject> carsToSpawn;

    void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnCar();
            timer = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void SpawnCar ()
    {        
        PhotonNetwork.Instantiate(carsToSpawn[Random.Range(0, carsToSpawn.Count)].name, transform.position, transform.rotation);
    }
}
