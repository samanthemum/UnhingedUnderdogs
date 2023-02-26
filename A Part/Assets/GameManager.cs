using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainCharacter; 
    bool isStillGoing = true;
    float score = 0.0f;
    float timeInSeconds = 0.0f;
    [SerializeField] float scorePerEnemy = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeInSeconds += Time.deltaTime;
    }

    public void ScoreByEnemyKill()
    {
        score += scorePerEnemy;
    }

    public float GetScore()
    {
        float currentScore = (int)(timeInSeconds / 10 * 100) + score;
        return currentScore;
    }

    public float GetTimeInSeconds()
    {
        return timeInSeconds;
    }
}
