using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour
{

   private float lastFire = 0;

   [SerializeField]
   private float fireInterval = 0.3f;

   private bool firing = false;
   private Vector3 fireRot;

   [SerializeField]
   private GameObject bullet;

   [SerializeField]
   private GameObject fireEffect;

   //private GameObject currentFireEffect;

   // Use this for initialization
   void Start()
   {
      Debug.Log("Gun start " + isLocalPlayer);
   }

   // Update is called once per frame
   void FixedUpdate()
   {
      if(!isLocalPlayer)
      {
         return;
      }

      if (firing && Time.time - lastFire >= fireInterval)
      {
         
         lastFire = Time.time;
         GameObject bullet_clone = GameObject.Instantiate(this.bullet, transform.position, Quaternion.identity) as GameObject;
         FireEffect();
        
         GetComponent<AudioSource>().Play();
         bullet_clone.GetComponent<Rigidbody>().velocity = fireRot * 10;
      } else
      {
         //if(currentFireEffect != null)
         //{
          //  Destroy(currentFireEffect.gameObject);
         //}
      }
   }

   public void Fire(bool fire, Vector3 fireRot)
   {
      firing = fire;
      this.fireRot = fireRot;
   }

   void FireEffect()
   {
      Transform[] children = GetComponentsInChildren<Transform>();
      foreach (Transform child in children)
      {
         if (child.name.Equals("firePoint"))
         {

            GameObject effectObj = GameObject.Instantiate(fireEffect, child.transform.position, child.transform.rotation) as GameObject;
            effectObj.transform.parent = child.transform;
            NetworkServer.Spawn(effectObj);
            Destroy(effectObj, 0.2f);
            
            //Debug.Log("firePoint " + currentFireEffect.transform);
         }
      }
   }
}
