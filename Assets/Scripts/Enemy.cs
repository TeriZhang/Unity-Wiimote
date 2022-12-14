using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target; //the enemy's target
    public Transform myTransform;

    public float moveSpeed; //move speed
    //public float rotationSpeed = 3; //speed of turning
    //public int range = 10; //Range within target will be detected

    public GameObject self;
    public Animator animator;

    public float HP;
    public bool Dead;

    public LevelAnimationController levelcontroller;

    public void Awake()
    {
        target = GameObject.FindWithTag("Player").transform; //target the player
        myTransform = transform;
        levelcontroller = GameObject.Find("LevelManager").GetComponent<LevelAnimationController>();

    }

    public void Start()
    {
        animator = self.GetComponentInChildren<Animator>();
        Dead = false;
    }

    public void Update()
    {
        Chase();
        if (HP <= 0)
        {
            animator.SetBool("Dead", true);
            Dead = true;
            levelcontroller.enemyDies();
            Destroy(self);
            //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            //{
            //    Destroy(self);
            //}

        }
    }

    public void Chase()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        Debug.Log("chasing");
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveSpeed = 0f;

            if (!Dead)
            {
                animator.SetInteger("battle", 1);
            }

            else
            {
                animator.SetInteger("battle", 4);
            }
            
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            
            Chase();
        }
    }

    public void takedamage(int damage)
    {
        HP -= damage * Time.deltaTime;
    }
}
