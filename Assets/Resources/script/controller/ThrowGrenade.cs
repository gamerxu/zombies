using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThrowGrenade : MonoBehaviour
{

   [SerializeField]
   private GameObject grenadeObj;

   // Use this for initialization
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if (CrossPlatformInputManager.GetButtonUp("Fire2"))
      {
         Ray ray = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);
         RaycastHit[] hitInfo = Physics.RaycastAll(ray);
         foreach(RaycastHit hit in hitInfo)
         {
            Debug.Log(hit.collider.name);
            if(hit.collider.name.Equals("Terrain"))
            //if(hit.collider.CompareTag("Terrain"))
            {
               //Debug.Log("Throw....." + mouseWLpos);
               GameObject grenadeObj_clone = GameObject.Instantiate(grenadeObj, transform.position, transform.rotation) as GameObject;
               grenadeObj_clone.GetComponent<GrenadeScript>().target = hit.point;
               grenadeObj_clone.SetActive(true);
               break;
            }
         }


      }
   }
}
