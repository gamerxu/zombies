using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScirpt : NetworkBehaviour {

   [SerializeField]
   private GameObject bloodEffect;

   void Awake()
   {
      Destroy(this.gameObject, 2.0f);
   }

   // Use this for initialization
   void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   void OnCollisionEnter(Collision collision)
   {
      Debug.Log("Bullet OnCollisionEnter");
      var hit = collision.gameObject;
      var hitPlayer = hit.GetComponent<ZombieControoler>();
      if (hitPlayer != null)
      {
         foreach (ContactPoint contact in collision.contacts)
         {
            ContactPoint point = contact;
            Debug.Log(Quaternion.LookRotation(hit.transform.position, transform.position));
            GameObject.Instantiate(bloodEffect, point.point, Quaternion.LookRotation(hit.transform.position, transform.position));
           
         }
         Destroy(this.gameObject);
      }

      //DestroyImmediate(this);
      //
   }


}
