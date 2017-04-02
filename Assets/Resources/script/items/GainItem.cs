using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GainItem : NetworkBehaviour {

   [SyncVar]
   public string prefabURI;

   [SyncVar]
   public NetworkInstanceId objectNetId;

   [SerializeField]
   private float pickUpDistance = 2.0f;

   Color beforeColor;
   // Use this for initialization
   void Start () {
      
      
   }
	
	// Update is called once per frame
	void Update () {
		
	}

   void OnMouseEnter()
   {
      Debug.Log("OnMouseEnter :" + this.gameObject.name);
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
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if(Vector3.Distance(this.transform.position,player.transform.position) <= pickUpDistance)
      {
         if(player.GetComponent<PlayerItems>().HaveSlot())
         {
            if(objectNetId != null && objectNetId.Value > 0)
            {
               player.GetComponent<PlayerItems>().CmdUnPackItemById(objectNetId);
            } else
            {
               player.GetComponent<PlayerItems>().CmdUnPackItem(prefabURI);
            }
            
            Destroy(this.gameObject);
         } else
         {
            Debug.Log("Enough");
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
