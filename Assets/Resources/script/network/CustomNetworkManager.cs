
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{


   public override void OnStartServer()
   {
      Debug.Log("CustomNetworkManager : OnStartServer" );
   }
}
