using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealthPoints = 5f;
    [SerializeField] float difficultyRamp = 1f;
    [SerializeField] float currentHealthPoints = 0f;
    Enemy enemy;

    void OnEnable()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHealthPoints--;
        if(currentHealthPoints<=0)
        {
            gameObject.SetActive(false);
            maxHealthPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
