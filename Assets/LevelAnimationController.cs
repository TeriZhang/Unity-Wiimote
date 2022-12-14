using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimationController : MonoBehaviour
{
    public float enemiesKilled;
    public Animator anim;
    public List<GameObject> enemyGroup;
    // Start is called before the first frame update
    void Start()
    {
        enemyGroup[0].SetActive(true);
        anim.GetComponent<Animator>();
        enemiesKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesKilled > 0 && enemiesKilled <= 1)
        {
            
            anim.SetBool("P1", true);
            
                enemyGroup[1].SetActive(true);
            
        }
        if (enemiesKilled == 3)
        {
            //enemyGroup[2].SetActive(true);
            anim.SetBool("P2", true);
        }
    }

    public void enemyDies()
    {
        enemiesKilled += 1;
    }
}
