using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControoler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   void OnCollisionEnter(Collision collision)
   {
      Debug.Log("Zombie OnCollisionEnter");
   }
}
