using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScirpt : NetworkBehaviour {

   [SerializeField]
   private GameObject bloodEffect;

   [SerializeField]
   private float m_moveSpeed = 20;
   public float moveSpeed { get { return m_moveSpeed; } }

   [SerializeField]
   private float m_syncPositionThreshold = 0.25f;
   public float syncPositionThreshold { get { return m_syncPositionThreshold; } }

   [SerializeField]
   private int m_rotationSpeed = 15;
   public int rotationSpeed { get { return m_rotationSpeed; } }

   public Vector3 forward;


   void Awake()
   {
      
      Destroy(this.gameObject, 2.0f);
   }

   // Use this for initialization
   void Start () {
     
   }
	
	// Update is called once per frame
	void FixedUpdate () {
     /*
     if(isServer)
      {
         syncPosition = transform.position;
      } else
      {
         transform.position = syncPosition;
      }
      */
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
            GameObject.Instantiate(bloodEffect, point.point, Quaternion.LookRotation(hit.transform.position, transform.position));
           
         }
         Destroy(this.gameObject);
      }

      //DestroyImmediate(this);
      //
   }


}
