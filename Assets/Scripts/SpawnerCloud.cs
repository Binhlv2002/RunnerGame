using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class SpawnerCloud : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudPrefab;
    [SerializeField] private Transform cloudParent;
    public float cloudSpawnTime = 2f;
    [Range(0f, 1f)] private float cloudSpawnTimeFactors = 0.1f;
    public float cloudSpeed = 1f;
    [Range(0f, 1f)] private float cloudSpeedFactors = 0.2f;
    private float _cloudSpawnTime;
    private float _cloudSpeed;
    private float timeAlive;
    private float timeUntilCloudSpawn;



    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearCloud);
        GameManager.Instance.onPlay.AddListener(ResetCloudFators);

    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;
            CaculateCloudFactors();
            SpawnLoop();
        }

    }

    private void CaculateCloudFactors()
    {
        _cloudSpawnTime = cloudSpawnTime / MathF.Pow(timeAlive, cloudSpawnTimeFactors);
        _cloudSpeed = cloudSpeed * MathF.Pow(timeAlive, cloudSpeedFactors);
    }

    private void SpawnLoop()
    {
        timeUntilCloudSpawn += Time.deltaTime;

        if (timeUntilCloudSpawn >= _cloudSpawnTime)
        {
            Spawn();
            timeUntilCloudSpawn = 0f;
        }
    }

    private void ClearCloud()
    {
        foreach (Transform child in cloudParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetCloudFators()
    {
        timeAlive = 1f;
        _cloudSpawnTime = cloudSpawnTime;
        _cloudSpeed = cloudSpeed;
    }

    private void Spawn()
    {
        GameObject cloudToSpawn = cloudPrefab[UnityEngine.Random.Range(0, cloudPrefab.Length)];
        GameObject spawnCloud = Instantiate(cloudToSpawn, transform.position, Quaternion.identity);
        spawnCloud.transform.parent = cloudParent;
        Rigidbody2D cloudRB = spawnCloud.GetComponent<Rigidbody2D>();
        cloudRB.velocity = Vector2.left * _cloudSpeed;
    }
}
