using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerItems : NetworkBehaviour
{

   [SyncVar]
   public NetworkInstanceId empty;

   private SyncListNetworkInstanceId items = new SyncListNetworkInstanceId();


   void Start()
   {

      GameObject emptyObject = Resources.Load("prefeb/items/EmptyObject") as GameObject;
      GameObject emptyObject_clone = GameObject.Instantiate(emptyObject, emptyObject.transform.position, emptyObject.transform.rotation);
      NetworkServer.Spawn(emptyObject_clone);
      empty = emptyObject_clone.GetComponent<NetworkIdentity>().netId;
      for (int i = 0; i < 26; i++)
      {
         items.Add(empty);
      }
      CmdEquip();
   }

   public bool HaveSlot()
   {
      for (int i = 5; i < 26; i++)
      {
         if (items[i].Equals(empty))
         {
            return true;
         }
      }
      return false;
   }

   public bool IsEmptyItem(NetworkInstanceId netId)
   {
      return empty.Equals(netId);
   }

   [Command]
   public void CmdUnPackItem(string prefabUri)
   {
      NetworkInstanceId item = UnPackWeapon(prefabUri);

      for (int i = 6; i < 26; i++)
      {
         if (items[i].Equals(empty))
         {
            //This is a empty so put the item there
            items[i] = item;
            return;
         }
      }
   }

   [Command]
   public void CmdUnPackItemById(NetworkInstanceId objectNetId)
   {
      for (int i = 6; i < 26; i++)
      {
         if (items[i].Equals(empty))
         {
            //This is a empty so put the item there
            items[i] = objectNetId;
            return;
         }
      }
   }



   NetworkInstanceId UnPackWeapon(string prefabUri)
   {
      GameObject weapon = ResourceLocation.Load(prefabUri) as GameObject;
      //Debug.Log(weapon.transform.rotation);
      GameObject weapon_clone = GameObject.Instantiate(weapon, weapon.transform.position, weapon.transform.rotation);
      weapon_clone.GetComponent<GunController>().gunQuaternion = weapon_clone.transform.rotation;
      weapon_clone.GetComponent<GunController>().parentNetId = this.netId;
      weapon.transform.localRotation = weapon.transform.rotation;
      weapon_clone.SetActive(true);

      // NetworkServer.Spawn(weapon_clone);
      NetworkServer.SpawnWithClientAuthority(weapon_clone, connectionToClient);

      return weapon_clone.GetComponent<NetworkIdentity>().netId;
   }

   public NetworkInstanceId indexItems(int index)
   {
      if(index < items.Count)
      {
         return items[index];
      }
      return new NetworkInstanceId(0);
   }

   [Command]
   public void CmdDrapItem(GameObject gameObject)
   {
      string prefabPath = "";
      // if(ItemType.weapon == gameObject.GetComponent<ItemInfo>().itemType)
      // {
      prefabPath = ResourceLocation.weapon_box_path;
      //  } 
      GameObject _box = ResourceLocation.Load(prefabPath) as GameObject;
      GameObject _box_clone = GameObject.Instantiate(_box, transform.position + new Vector3(1, 0, 1), _box.transform.rotation);
      _box_clone.GetComponent<GainItem>().objectNetId = gameObject.GetComponent<NetworkIdentity>().netId;
      NetworkServer.Spawn(_box_clone);
      _box_clone.SetActive(true);

      int idx = items.IndexOf(gameObject.GetComponent<NetworkIdentity>().netId);
      if (idx >= 0)
      {
         items[idx] = empty;
      }
   }

   [Command]
   public void CmdChangeItem(int clickItemIndex, GameObject clickItem, GameObject holdItem)
   {
      NetworkInstanceId srcNetId = items[clickItemIndex];
      NetworkInstanceId destNetId = holdItem.GetComponent<NetworkIdentity>().netId;

      int destIndex = items.IndexOf(destNetId);
      //If source item is emptyItem then changes directly
      if(empty.Equals(srcNetId) && clickItemIndex >= 6)
      {
         items[clickItemIndex] = destNetId;
         items[destIndex] = empty;
         return;
      }

      //If source item is equip 
      if(clickItemIndex < 6 && validateItemType(clickItemIndex, holdItem.GetComponent<ItemInfo>().itemType))
      {
         items[clickItemIndex] = destNetId;
         items[destIndex] = srcNetId;
      }

      //If dest item is equip
      if(destIndex < 6 && (holdItem.GetComponent<ItemInfo>().itemType == clickItem.GetComponent<ItemInfo>().itemType))
      {
         items[clickItemIndex] = destNetId;
         items[destIndex] = srcNetId;
      }

      //Both are in item position
      if(destIndex >= 6 && clickItemIndex >=6)
      {
         items[clickItemIndex] = destNetId;
         items[destIndex] = srcNetId;
      }
      
   }

   private bool validateItemType(int index , ItemType itemType)
   {
      Debug.Log("validateItemType:" + index + " " + itemType);
      switch (index)
      {
         case 0:
            if (ItemType.weapon != itemType)
            {
               return false;
            }
            break;
         case 1:
            if (ItemType.glove != itemType)
            {
               return false;
            }
            break;
         case 2:
            if (ItemType.gear != itemType)
            {
               return false;
            }
            break;
         case 3:
            if (ItemType.head != itemType)
            {
               return false;
            }
            break;
         case 4:
            if (ItemType.body != itemType)
            {
               return false;
            }
            break;
         case 5:
            if (ItemType.shoes != itemType)
            {
               return false;
            }
            break;
      }
      return true;
   }

   [Command]
   public void CmdEquip()
   {

      GameObject weapon = ResourceLocation.Load("prefeb/items/weapons/M4A1/M4A1_01") as GameObject;
      //Debug.Log(weapon.transform.rotation);
      GameObject weapon_clone = GameObject.Instantiate(weapon, weapon.transform.position, weapon.transform.rotation);
      weapon_clone.GetComponent<GunController>().gunQuaternion = weapon_clone.transform.rotation;
      weapon_clone.GetComponent<GunController>().parentNetId = this.netId;
      weapon.transform.localRotation = weapon.transform.rotation;
      weapon_clone.SetActive(true);

      // NetworkServer.Spawn(weapon_clone);
      NetworkServer.SpawnWithClientAuthority(weapon_clone, connectionToClient);

      //Set Weapon
      Debug.Log("wpId " + weapon_clone.GetComponent<NetworkIdentity>().netId);

      items[0] = weapon_clone.GetComponent<NetworkIdentity>().netId;
   }

   public class SyncListNetworkInstanceId : SyncList<NetworkInstanceId>
   {

      protected override void SerializeItem(NetworkWriter writer, NetworkInstanceId item)
      {
         writer.Write(item);
      }

      protected override NetworkInstanceId DeserializeItem(NetworkReader reader)
      {
         return reader.ReadNetworkId();
      }

   }
}
