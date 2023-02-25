using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // Enemy Information
    Rigidbody enemyRB;
    NavMeshAgent navMeshAgent;
    Attack enemyAttack;
    Health enemyHealth;

    // Character Information
    Rigidbody mainCharacterRB;
    Attack mainAttack;
    Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] float enemySpeed_normal = .10f;
    [SerializeField] float enemySpeed_inRange = .10f;
    [SerializeField] float speedUpRange = .001f;
    [SerializeField] int reactionTimeInFrames = 5;
    int currentFrame = -1;

    void Start()
    {
        mainCharacterRB = GameObject.FindGameObjectWithTag("main character").GetComponent<Rigidbody>();
        if (!mainCharacterRB)
        {
            Debug.Log("Could not find main character");
        }

        mainAttack = GameObject.FindGameObjectWithTag("main character").GetComponent<Attack>();
        if (!mainAttack)
        {
            Debug.Log("Could not find main character attack");
        }

        enemyRB = GetComponent<Rigidbody>();
        if (!enemyRB)
        {
            Debug.Log("Failed to get enemy rigidbody");
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (!navMeshAgent)
        {
            Debug.Log("Failed to get enemy rigidbody");
        }

        enemyAttack = GetComponent<Attack>();
        if(!enemyAttack)
        {
            Debug.Log("Failed to get enemy attack component");
        }

        enemyHealth = GetComponent<Health>();
        if(!enemyHealth)
        {
            Debug.Log("Failed to get enemy health");
        }
    }

    bool inRange()
    {
        if ((targetPosition - enemyRB.transform.position).magnitude < speedUpRange)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    //// TODO: Add nav mesh
    void Update()
    {
        // update the target every so many frames
        if (currentFrame == -1 || currentFrame == reactionTimeInFrames)
        {
            targetPosition = mainCharacterRB.transform.position;
            navMeshAgent.destination = targetPosition;
        }
        else
        {
            currentFrame++;
        }

        if (inRange())
        {
            navMeshAgent.speed = enemySpeed_inRange;
        }
        else
        {
            navMeshAgent.speed = enemySpeed_normal;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "main character")
        {
            // check to make sure the main character isn't attacking
            Animator mainCharacterAnimatior = collision.gameObject.GetComponent<Animator>();
            if(!mainCharacterAnimatior.GetCurrentAnimatorStateInfo(0).IsName("attacking"))
            {
                enemyAttack.setCurrentAttackTarget(collision.gameObject);
                enemyAttack.DoAttack();
            } 

            // TODO: might need to make it easier to take damage
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        enemyAttack.setCurrentAttackTarget(null);
    }
}
