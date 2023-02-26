using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timePerSpawn = 10.0f;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] GameObject alternateLocationObject;
    float currentTime = -1.0f;
    float totalTime = 0.0f;
    EnemySpecs[] specs;
    Transform[] alternateLocations;

    void Start()
    {
        // get our possible enemy variations
        specs = GetComponentsInChildren<EnemySpecs>();
        alternateLocations = alternateLocationObject.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < 0.0f || currentTime >= timePerSpawn)
        {
            
            // move to new location
            int locationIndex = Random.Range(1, alternateLocations.Length);
            if(locationIndex >= alternateLocations.Length)
            {
                locationIndex = alternateLocations.Length - 1;
            }
            Vector3 newPosition;
            Quaternion newRotation;
            Debug.Log("Locations length is " + alternateLocations.Length);
            alternateLocations[locationIndex].GetPositionAndRotation(out newPosition, out newRotation);
            Debug.Log("The new position x coordinate is " + newPosition.x);
            Debug.Log("Selected coordinate was " + locationIndex);
            this.gameObject.transform.SetPositionAndRotation(newPosition, newRotation);

            // spawn stuff
            int numberToSpawn = (int)(totalTime / timePerSpawn / 2.0f);

            if(numberToSpawn == 0)
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
            int specIndex = Random.Range(0, specs.Length);
            EnemySpecs specsToUse = specs[specIndex];

            if(enemy.GetComponent<EnemySpecs>())
            {
                enemy.GetComponent<EnemySpecs>().SetStats(specsToUse.sprite, specsToUse.speed, specsToUse.attack, specsToUse.health, specsToUse.attackedSound, specsToUse.deadSound);
                enemy.GetComponent<Animator>().runtimeAnimatorController = specsToUse.animator;
            }
            
        }

        yield return new WaitForSeconds(0f);
    }
}
