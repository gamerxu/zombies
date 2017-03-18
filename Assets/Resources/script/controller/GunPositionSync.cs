using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunPositionSync : NetworkBehaviour {

   [SyncVar]
   public Quaternion gunQuaternion;

   [SyncVar]
   public NetworkInstanceId parentNetId;


   void Start()
   {
      //speed = GetComponent<TPlayerController>().moveSpeed;
      //syncThreshold = GetComponent<TPlayerController>().syncPositionThreshold;
      //rotationSpeed = GetComponent<TPlayerController>().rotationSpeed;
      
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
