using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStats : MonoBehaviour
{
    public float hp,maxhp;

    public Image overlay;

    
    // Start is called before the first frame update
    void Start()
    {
        maxhp = 500f;
        hp = maxhp;

    }

    // Update is called once per frame
    void Update()
    {
        overlay.color = new Color (overlay.color.r, overlay.color.g, overlay.color.b, (maxhp - hp)/maxhp);


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
        hp -= 2f;
    }
}
