using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStats : MonoBehaviour
{
    public float hp,maxhp;

    public Image overlay;

    public SmokeSliderScript player1Smoke;

    public AudioSource dialogue1,dialogue2;
    public float dialogueTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        maxhp = 500f;
        hp = maxhp;
        dialogue1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(dialogueTimer >= 5)
        {
            dialogue2.enabled = true;
        }
        else
        {
            dialogueTimer += Time.deltaTime;
        }
        overlay.color = new Color (overlay.color.r, overlay.color.g, overlay.color.b, (maxhp - hp)/maxhp);

        player1Smoke.smokeLevel = 500 - hp;
        //Death

        if (hp <= 0)
        {
            Debug.Log("player dead");
            SceneManager.LoadScene("EndScene");
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDmg();

        }
    }

    public void TakeDmg()
    {
        hp -= 25f * Time.deltaTime;
    }
}
