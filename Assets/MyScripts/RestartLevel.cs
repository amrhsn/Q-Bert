﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (LifeController.health == 0)
        {

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    
}
}
