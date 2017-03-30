using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class GunController : NetworkBehaviour
{

   private float lastFire = 0;

   [SyncVar]
   private bool firing = false;
   [SyncVar]
   private Vector3 fireRot;

   [SerializeField]
   private GameObject bullet;

   [SerializeField]
   private GameObject fireEffect;

   [SerializeField]
   private Transform firePoint;


   [SyncVar]
   public Quaternion gunQuaternion;

   [SyncVar]
   public NetworkInstanceId parentNetId;

   private GameObject character;

   ItemInfo itemInfo;

   //private GameObject currentFireEffect;

   void Awake()
   {
      
   }

   // Use this for initialization
   void Start()
   {
      character = ClientScene.FindLocalObject(parentNetId);
      itemInfo = GetComponent<ItemInfo>();
   }

   // Update is called once per frame
   void FixedUpdate()
   {
      if (!isLocalPlayer)
      {
         return;
      }
   }

   void Update()
   {
     
      if (!character.GetComponent<NetworkBehaviour>().isLocalPlayer)
      {
         return;
      }
     
      if (CrossPlatformInputManager.GetButton("Fire1"))
      {
        // CmdFire();
      }
      else if (CrossPlatformInputManager.GetButtonUp("Fire1"))
      {
      }

   }

   [ClientRpc]
   void RpcFireBullet(NetworkInstanceId shooterNetId, NetworkInstanceId bulletNetId)
   {
      if (isClient)
      {
         GameObject shooter = ClientScene.FindLocalObject(shooterNetId);
         GameObject bullet = ClientScene.FindLocalObject(bulletNetId);
         bullet.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 10;
      }

   }

   [Command]
   public void CmdFire()
   {

      if (Time.time - lastFire >= itemInfo.fireInterval)
      {

         lastFire = Time.time;
        
         GameObject bullet_clone = GameObject.Instantiate(this.bullet, firePoint.position, firePoint.rotation) as GameObject;

         FireEffect();
        
         bullet_clone.GetComponent<Rigidbody>().velocity = bullet_clone.transform.forward * 10;
         GetComponent<AudioSource>().Play();
         NetworkServer.Spawn(bullet_clone);


         //bullet_clone.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * 10);

         // RpcFireBullet(shooterNetId, bullet_clone.GetComponent<NetworkIdentity>().netId);
      }
      else
      {
         //if(currentFireEffect != null)
         //{
         //  Destroy(currentFireEffect.gameObject);
         //}
      }

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

   public override void OnStartClient()
   {
      Debug.Log("OnStartClient " + gunQuaternion);


      GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
      Transform[] arms = parentObject.GetComponentsInChildren<Transform>();
      foreach (Transform o in arms)
      {
         //Debug.Log(o.name);
         if (o.name.Equals("swat:RightHandRing1"))
         {

            transform.position = o.transform.position;
            transform.SetParent(o.transform);
            // weapon_clone.GetComponent<GunPositionSync>().handMount = o;

            break;
         }
      }
      transform.localRotation = gunQuaternion;
   }
}
