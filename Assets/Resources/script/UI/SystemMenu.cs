using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMenu : MonoBehaviour {

   int screenWidth = Screen.width;
   int screenHeight = Screen.height;

   int windowWidth = 300;
   int windowHeight = 45;
   int windowX, windowY;
   Rect bar;
   int buttonSize = 45;
   public bool showMenu = false;

   // Use this for initialization
   void Start()
   {
      windowX = (screenWidth - windowWidth);
      windowY = (screenHeight - windowHeight-25);
     
   }

   // Update is called once per frame
   void Update()
   {

   }

   void OnGUI()
   {
      if (!showMenu)
      {
         return;
      }
      bar = new Rect(windowX, windowY, windowWidth, windowHeight);
      GUILayout.Window(0, bar, BarItem, "");
      
   }

   void BarItem(int id)
   {
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("test1", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
      {
         Debug.Log("test1");
      }
      if (GUILayout.Button("test2", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
      {
         Debug.Log("test2");
      }
      GUILayout.EndHorizontal();
   }

}
