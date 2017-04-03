using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GainItem : NetworkBehaviour
{

   [SyncVar]
   public string prefabURI;

   [SyncVar]
   public NetworkInstanceId objectNetId;

   [SerializeField]
   private float pickUpDistance = 2.0f;

   Color beforeColor;
   // Use this for initialization
   void Start()
   {


   }

   // Update is called once per frame
   void Update()
   {

   }

   void OnMouseEnter()
   {
      beforeColor = GetComponentInChildren<Renderer>().material.color;
      SetColor(Color.green);
      // GetComponentInChildren<Renderer>().material.color = Color.green;
   }

   void OnMouseExit()
   {
      SetColor(beforeColor);
   }

   void OnMouseUp()
   {
      GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      foreach (GameObject player in players)
      {
         if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
         {
            player.GetComponent<PlayerItems>().CmdPickUp(this.netId);
         }
      }
      
   }

   

   void SetColor(Color newColor)
   {
      Renderer[] renderers = GetComponentsInChildren<Renderer>();
      foreach (Renderer r in renderers)
      {
         foreach (Material m in r.materials)
         {
            if (m.HasProperty("_Color"))
               m.color = newColor;
         }
      }
   }


}
