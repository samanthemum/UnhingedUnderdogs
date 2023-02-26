using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{

    // Update is called once per frame
     [SerializeField] float characterSpeed = 1.0f;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Attack attack;
    [SerializeField] float maxVelocity = 10.0f;
    Animator animator;
    Health health;
    GameManager manager;

    // Audio clips
    public AudioClip attackedNoise;
    public AudioClip hitNoise;
    public AudioSource footstepsSource;

    // Audio source
    [SerializeField] AudioSource sourceFootsteps;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            Debug.Log("Could not set rigid body");
        }

        manager = FindObjectOfType<GameManager>();
        if(!manager)
        {
            Debug.Log("Couldn't grab manager");
        }

        attack = GetComponent<Attack>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        attack.attackAudioClip = hitNoise;
        health.damageSound = attackedNoise;
    }

    void Update()
    {

        // check to see if we should die
        if(health.GetHealth() <= 0)
        {
            Debug.Log("You died!");
        }

        float horizontalComponent = Input.GetAxis("Horizontal");
        float verticalComponent = Input.GetAxis("Vertical");
        
        if(rigidbody)
        {
            //Vector3 currentPosition = rigidbody.transform.position;
            //Vector3 targetPosition = new Vector3(currentPosition.x + horizontalComponent * characterSpeed, currentPosition.y, currentPosition.z + verticalComponent * characterSpeed);
            // GetComponent<Rigidbody>().velocity = characterSpeed * (new Vector3(horizontalComponent, GetComponent<Rigidbody>().velocity.y, verticalComponent));
            if(rigidbody.velocity.magnitude < maxVelocity)
            {
                GetComponent<Rigidbody>().AddForce(characterSpeed * (new Vector3(horizontalComponent, GetComponent<Rigidbody>().velocity.y, verticalComponent)));
            } else
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
            }
            

        }

        // Do attacks
        if(Input.GetAxis("Attack") > 0)
        {
            attack.DoAttack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<EnemyMovement>())
        {
            attack.setCurrentAttackTarget(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attack.setCurrentAttackTarget(null);
    }

}
