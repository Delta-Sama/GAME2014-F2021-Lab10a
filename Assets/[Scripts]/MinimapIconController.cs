using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconController : MonoBehaviour
{
    public Transform target;

    public Sprite PlayerIcon;
    public Sprite EnemyIcon;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(GameObject Target, MinimapEntityEnum enemyType)
    {
        target = Target.transform;

        switch (enemyType)
        {
            case MinimapEntityEnum.PLAYER:
                {
                    spriteRenderer.sprite = PlayerIcon;
                    break;
                }
            case MinimapEntityEnum.ENEMY_OPOSSUM:
                {
                    spriteRenderer.sprite = EnemyIcon;
                    break;
                }
        }
    }

    void Update()
    {
        transform.position = target.position;
        transform.localScale = new Vector3(Mathf.Sign(target.localScale.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}

public enum MinimapEntityEnum
{
    PLAYER,
    ENEMY_OPOSSUM
}