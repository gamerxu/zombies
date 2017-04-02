using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestGainItem : NetworkBehaviour {

	// Use this for initialization
	void Start () {

      GameObject weapon_Box = Resources.Load("prefeb/items/Weapon_Box") as GameObject;
      GameObject weapon_Box_clone = GameObject.Instantiate(weapon_Box, new Vector3(10,0,10), weapon_Box.transform.rotation);
      weapon_Box_clone.GetComponent<GainItem>().prefabURI = "prefeb/items/weapons/M4A1/M4A1";
      NetworkServer.Spawn(weapon_Box_clone);
      weapon_Box_clone.SetActive(true);
   }
	
	// Update is called once per frame
	void Update () {
		
	}
}
