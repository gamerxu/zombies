using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerItems : NetworkBehaviour {

   [SyncVar]
   public NetworkInstanceId m_weapon;
   public NetworkInstanceId weapon {get { return m_weapon; } set { m_weapon = value; } }
}
