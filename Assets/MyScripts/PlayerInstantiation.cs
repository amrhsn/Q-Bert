using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiation : MonoBehaviour {

    public Transform newInstance;

    public void RespawnPlayer()
    {
        Instantiate(gameObject.transform, newInstance.position, Quaternion.identity);
    }
}
