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
    public AudioClip wasDamagedNoise;
    public AudioClip didAttackNoise;
    public AudioSource footstepsSource;

    // Audio source
    [SerializeField] AudioSource sourceFootsteps;

    // Character alts
    [SerializeField] GameObject alternateBuilds;
    MainCharacterBuild[] builds;
    int currentBuildIndex = 0;
    string currentBuild;

    // Last forward vector
    Vector3 lastForward;
    [SerializeField] GameObject projectilePrefab;

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

        attack.attackAudioClip = didAttackNoise;
        health.damageSound = wasDamagedNoise;

        // setup alternate builds
        builds = alternateBuilds.GetComponentsInChildren<MainCharacterBuild>();
        if(builds.Length > 0)
        {
            SetBuild(builds[0]);
            currentBuildIndex = 0;
        }
    }

    private void SetBuild(MainCharacterBuild newBuild)
    {
        // Setup battle stats
        attack.SetAttack(newBuild.attack);
        characterSpeed = newBuild.speed;
        health.SetDefense(newBuild.defense);
        maxVelocity = newBuild.maxSpeed;

        // Set visuals
        animator.runtimeAnimatorController = newBuild.animator;
        GetComponent<SpriteRenderer>().sprite = newBuild.sprite;

        // Set sounds
        wasDamagedNoise = newBuild.hitClip;
        didAttackNoise = newBuild.attackClip;
        attack.attackAudioClip = didAttackNoise;
        health.damageSound = wasDamagedNoise;

        // Set admin stuff
        currentBuild = newBuild.title;
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
        if(horizontalComponent != 0 || verticalComponent != 0)
        {
            Vector3 direction = new Vector3(horizontalComponent, 0, verticalComponent);
            lastForward = direction.normalized;
        }
        
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
        if (Input.GetButtonDown("Attack"))
        {
            if (currentBuild != "cleric")
            {
                attack.DoAttack();
             }

            else
            {
                Debug.Log("Spawn projectile");
                EnemyMovement target = FindObjectOfType<EnemyMovement>();
                Vector3 projectileDirection = (target.transform.position - this.transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, this.transform.position + .1f * projectileDirection, this.transform.rotation) as GameObject;
                projectile.GetComponent<Rigidbody>().AddForce(projectileDirection);
                projectile.GetComponent<TalismanController>().mainCharacterPosition = this.transform;
                projectile.GetComponent<Attack>().SetAttack(attack.GetAttack());
            }
        }

        // Do Swaps
        if(Input.GetButtonDown("Swap"))
        {
            currentBuildIndex++;
            currentBuildIndex %= builds.Length;
            SetBuild(builds[currentBuildIndex]);
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
