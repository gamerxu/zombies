using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZombieControoler : NetworkBehaviour {

   [SyncVar]
   private int health = 100;

   // Use this for initialization
   void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   void OnCollisionEnter(Collision collision)
   {
      if(isServer)
      {
         Debug.Log("Zombie OnCollisionEnter " + collision.collider.tag);
         if (collision.collider.tag.Equals("bullet"))
         {
            health -= 10;
         }
      }
    
      
   }
}
