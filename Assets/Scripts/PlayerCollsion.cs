using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollsion : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ActivePlayer);
    }

    private void ActivePlayer()
    {
        gameObject.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !GameManager.Instance.IsGameEnded())
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
            Debug.Log("Die");
        }
    }
}
