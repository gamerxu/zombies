using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class FireController : NetworkBehaviour
{

   private float lastFire = 0;

   [SerializeField]
   private float fireInterval = 0.3f;
   [SerializeField]
   private GameObject bullet;

   private bool m_IsFiring;
   public bool isFiring { get { return m_IsFiring; } }

   // Use this for initialization
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if (!isLocalPlayer)
      {
         return;
      }

      if (CrossPlatformInputManager.GetButton("Fire1"))
      {
         m_IsFiring = true;

         Ray ray = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);
         RaycastHit[] hitInfo = Physics.RaycastAll(ray);
         foreach (RaycastHit hit in hitInfo)
         {
            //Debug.Log(hit.collider.name);
            if (hit.collider.name.Equals("Terrain"))
            {
               transform.LookAt(new Vector3(hit.point.x, transform.transform.position.y, hit.point.z));
               GetComponent<Play_syncRotation>().Transmit(transform.transform.position, transform.localRotation);
               break;
            }
         }


         GetComponent<Animator>().SetBool("Fire", true);
         GetComponent<Animator>().SetBool("Run", false);
         //GetComponentInChildren<GunController>().CmdFire();
         
      }
      else if (CrossPlatformInputManager.GetButtonUp("Fire1"))
      {
         m_IsFiring = false;
         GetComponent<Animator>().SetBool("Fire", false);
         // GetComponentInChildren<GunController>().CmdFire(false, transform.forward);
      }

   }
   
}
