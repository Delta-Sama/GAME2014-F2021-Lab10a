using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject MinimapMarker;

    public GameObject Player;
    public Transform spawnPointTransform;

    public List<EnemyController> enemies;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetPlayer();

        GameObject Marker = Instantiate(MinimapMarker);
        Marker.GetComponent<MinimapIconController>().SetTarget(Player, MinimapEntityEnum.PLAYER);

        foreach (var enemy in enemies)
        {
            GameObject EnemyMarker = Instantiate(MinimapMarker);
            EnemyMarker.GetComponent<MinimapIconController>().SetTarget(enemy.gameObject, MinimapEntityEnum.ENEMY_OPOSSUM);
        }
    }

    public void ResetPlayer()
    {
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Player.transform.position = spawnPointTransform.position;
    }
}
