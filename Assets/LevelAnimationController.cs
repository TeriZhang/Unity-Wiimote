using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelAnimationController : MonoBehaviour
{
    public float enemiesKilled;
    public Animator anim;
    public List<GameObject> enemyGroup;
    public GameObject enemyPrefab;
    public List<GameObject> spawnPoints;
    public int spawnamount,spawnLoca;
    public float spawntime, spawntimer;
    public bool canSpawn;

    public TextMeshProUGUI playerScore;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyGroup[0].SetActive(true);
        anim.GetComponent<Animator>();
        enemiesKilled = 0;
        canSpawn = false;
        spawntimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        playerScore.text ="P1 Score: " + enemiesKilled * 5;
        if(enemiesKilled > 0 && enemiesKilled <= 1)
        {
            
            anim.SetBool("P1", true);
            
                enemyGroup[1].SetActive(true);
            
        }
        if (enemiesKilled == 3)
        {
            enemyGroup[2].SetActive(true);
            anim.SetBool("P2", true);
        }

        if (enemiesKilled == 5)
        {
            enemyGroup[3].SetActive(true);
            anim.SetBool("P3", true);
        }

        if (enemiesKilled == 9)
        {
            canSpawn = true;
            anim.SetBool("P4", true);
        }

        if(canSpawn == true)
        {
            spawntime += 1 * Time.deltaTime;

            if (spawntime >= spawntimer)
            {
                spawnamount = Random.Range(1, 3);
                for (int i = 0; i < spawnamount; i++)
                {


                    

                    spawnLoca = Random.Range(0, 8);
                    Instantiate(enemyPrefab, spawnPoints[spawnLoca].transform.position, Quaternion.identity);


                }
                spawntime = 0;
            }
                
            
        }
    }

    public void enemyDies()
    {
        enemiesKilled += 1;
    }
}
