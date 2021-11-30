using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraBehaviour : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        transform.position = player.position + new Vector3(0.0f, 0.0f, -10.0f);
    }
}
