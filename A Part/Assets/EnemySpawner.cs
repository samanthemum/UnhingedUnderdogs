using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timePerSpawn = 10.0f;
    [SerializeField] GameObject enemyToSpawn;
    float currentTime = -1.0f;
    float totalTime = 0.0f;
    EnemySpecs[] specs;

    void Start()
    {
        // get our possible enemy variations
        specs = GetComponentsInChildren<EnemySpecs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < 0.0f || currentTime >= timePerSpawn)
        {
            

            // spawn stuff
            int numberToSpawn = (int)(totalTime / timePerSpawn);

            if(currentTime < 0)
            {
                numberToSpawn = 1;
            }

            // reset timer
            currentTime = 0.0f;

            // create enemies from prefab
            StartCoroutine(SpawnEnemies(numberToSpawn));
        }
        
        // increment timer
        else
        {
            currentTime += Time.deltaTime;
        }

        totalTime += Time.deltaTime;
    }

    IEnumerator SpawnEnemies(int enemyCount)
    {
        GameObject enemy;
        //Debug.Log("Spawning " + enemyCount + " enemies");
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log(i);
            Transform ts = GetComponent<Transform>();

            enemy = Instantiate(enemyToSpawn, ts.position, enemyToSpawn.transform.rotation) as GameObject;

            //// get enemy stats from child
            int specIndex = (int)Mathf.Round(Random.Range(0.0f, specs.Length - 1));
            EnemySpecs specsToUse = specs[specIndex];

            if(enemy.GetComponent<EnemySpecs>())
            {
                enemy.GetComponent<EnemySpecs>().SetStats(specsToUse.sprite, specsToUse.speed, specsToUse.attack, specsToUse.health, specsToUse.animator, specsToUse.attackedSound, specsToUse.deadSound);
            }
            
        }

        yield return new WaitForSeconds(0f);
    }
}
