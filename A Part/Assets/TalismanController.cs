using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalismanController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform mainCharacterPosition;
    public float maxTime = 2.0f;
    float currentTime = 0.0f;

    void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > maxTime)
        {
            Object.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyMovement>())
        {
            Attack attackComponent = this.GetComponent<Attack>();
            attackComponent.setCurrentAttackTarget(collision.gameObject);
            attackComponent.DoAttack();
            Object.Destroy(this.gameObject);
        }

        
    }
}
