using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLocation {

   public static string weapon_box_path = "prefeb/items/Weapon_Box";
   public static string m4a1_path = "prefeb/items/weapons/M4A1/M4A1";

   public static Object Load(string path)
   {
      return Resources.Load(path);
   }
}
