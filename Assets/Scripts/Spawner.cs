using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefab;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    [Range(0f, 1f)] private float obstacleSpawnTimeFactors = 0.1f;
    public float obstacleSpeed = 1f;
    [Range(0f, 1f)] private float obstacleSpeedFactors = 0.2f;
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeAlive;
    private float timeUntilObstacleSpawn;
   
   

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(ResetFators);
       
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;
            CaculateFactors();
            SpawnLoop();
        }
        
    }

    private void CaculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / MathF.Pow(timeAlive, obstacleSpawnTimeFactors);
        _obstacleSpeed = obstacleSpeed * MathF.Pow(timeAlive, obstacleSpeedFactors);
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles()
    {
        foreach(Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetFators()
    {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefab[UnityEngine.Random.Range(0, obstaclePrefab.Length)];
        GameObject spawnObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnObstacle.transform.parent = obstacleParent;
        Rigidbody2D obstacleRB = spawnObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * _obstacleSpeed;
    }
}
