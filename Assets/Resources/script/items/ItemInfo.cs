using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour {

   public Texture2D icon;
   public string name;
   public float fireInterval;
   public float accuracy;
   public float headShotRate;
   public ItemType itemType;
   public string color;
   public string story;

   private const string LINE = "\n";

   public string getDescription()
   {
      if(ItemType.weapon == itemType)
      {
         return takeColor(color, takeSize(20, name)) + LINE
         + takeColor(color, takeSize(7, itemType.ToString())) + LINE
         + "speed : " + fireInterval + LINE
         + "accuracy : " + accuracy + LINE
         + "headShotRate : " + headShotRate + LINE
         + LINE
         + story + LINE;
      }
      return "";
      
   }

   string takeColor(string color ,string content)
   {
      if(!string.IsNullOrEmpty(color))
         return "<color='" + color + "'>" + content + "</color>";
      return content;
   }

   string takeSize(int size , string content)
   {
      if(size > 0)
       return "<size='" + size + "'>" + content + "</size>";
      return content;
   }

}

public enum ItemType
{
   weapon, glove,gear,head,body,shoes,items,ammo
}

