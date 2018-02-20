using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {
    public GameObject life1, life2, life3, gameOver;
    public static int health;
    // Use this for initialization
    void Start () {
        health = 3;
        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (health > 3)
            health = 3;
        switch (health)
        {
            case 3:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                gameOver.SetActive(false);
                break;
            case 2:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                gameOver.SetActive(false);
                break;
            case 1:
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                gameOver.SetActive(false);
                break;
            case 0:
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                gameOver.SetActive(true);
                
                break;
        }
    }
}
