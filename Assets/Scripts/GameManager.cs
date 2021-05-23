using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TileGenerator tileGenerator;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private KillPlane killPlane;


    private void Awake()
    {
        killPlane.OnPlayerKilled += HandleOnPlayerKilled;
    }

    private void HandleOnPlayerKilled()
    {
        SpawnPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab);
        player.transform.position = spawnPoint.position;
        playerCamera.Follow = player.transform;
        playerCamera.LookAt = player.transform;
    }
    
    
}
