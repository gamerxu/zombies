using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ItemMenu : NetworkBehaviour
{


   public GUISkin skin;

   int screenWidth = Screen.width;
   int screenHeight = Screen.height;

   int windowWidth = 300;
   int windowHeight = 500;
   int windowX, windowY;
   Rect bar;
   int buttonSize = 50;
   int labelSize = 80;
   private bool m_showMenu = false;
   public bool showMenu { get { return m_showMenu; } }

   private bool m_mouseOnMenu = false;
   public bool mouseOnMenu { get { return m_mouseOnMenu; } }


   PlayerItems playerItems;
   private string lastTooltip = " ";
   private bool showInfo = false;

   private bool pickUpSelfItem = false;
   private Texture2D cursorIcon;
   private GameObject hodingItem;

 

   // Use this for initialization
   void Start()
   {
      if(isLocalPlayer)
      {
         windowX = (screenWidth - windowWidth) / 2;
         windowY = (screenHeight - windowHeight) / 2;
         bar = new Rect(windowX, windowY, windowWidth, windowHeight);
         playerItems = GetComponent<PlayerItems>();
      }
     
   }

   // Update is called once per frame
   void Update()
   {
      if (!isLocalPlayer)
      {
         return;
      }

      if (Input.GetKeyDown(KeyCode.C))
      {
         m_showMenu = !m_showMenu;
      }

      if (Input.GetMouseButtonUp(0) && !m_mouseOnMenu && pickUpSelfItem)
      {
         //TODO drap item
         Debug.Log("drap item");
         playerItems.CmdDrapItem(hodingItem);
         ResetPickUp();
      }

      if(Input.GetMouseButtonUp(1) && pickUpSelfItem)
      {
         ResetPickUp();
      }
   }
   void OnGUI()
   {
      if (!showMenu)
      {
         return;
      }
      GUI.skin = skin;
      GUI.depth = 0;
      //bar = new Rect(windowX, windowY, windowWidth, windowHeight);
      bar = GUILayout.Window(0, bar, BarItem, "Character");

      if (showInfo)
      {
         ShowInfo();
      }

      if (pickUpSelfItem)
      {
         DrewCursor();
      }
   }

   void BarItem(int id)
   {
      //Main Menu
      GUILayout.BeginVertical();

      EquipMenu();
      GUILayout.Label("", GUILayout.Height(buttonSize / 2));
      ItemsMenu();
      //End Main Menu
      GUILayout.EndVertical();
      CheckMouseOver();
      GUI.DragWindow();
   }

   void ItemsMenu()
   {
      //Items Menu;
      GUILayout.BeginVertical();

      for (int i = 6; i < 26; i++)
      {
         if (i == 6)
         {
            GUILayout.BeginHorizontal();
         }

         if (i % 5 == 1 && i != 6)
         {
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
         }


         GameObject itemObj = ClientScene.FindLocalObject(playerItems.indexItems(i));
         ItemGUI(itemObj,i);

      }
      GUILayout.EndHorizontal();
      //End Items Menu;
      GUILayout.EndVertical();
   }

   void EquipMenu()
   {

      //Equip Menu
      GUILayout.BeginHorizontal();
      GUILayout.Space(10);

      //Column 1
      GUILayout.BeginVertical();

      //Weapon
      GUILayout.Label("weapon", GUILayout.Width(labelSize));
      GameObject weaponObj = ClientScene.FindLocalObject(playerItems.indexItems(0));
      ItemGUI(weaponObj,0);


      //glove

      GUILayout.Label("glove", GUILayout.Width(labelSize));
      GameObject gloveObj = ClientScene.FindLocalObject(playerItems.indexItems(1));
      ItemGUI(gloveObj,1);

      //Head
      GUILayout.Label("gear", GUILayout.Width(labelSize));
      GameObject gearObj = ClientScene.FindLocalObject(playerItems.indexItems(2));
      ItemGUI(gearObj,2);



      //Column 1 End
      GUILayout.EndVertical();

      //Column 2
      GUILayout.BeginVertical();
      //Weapon

      GUILayout.Label("head", GUILayout.Width(labelSize));
      GameObject headObj = ClientScene.FindLocalObject(playerItems.indexItems(3));
      ItemGUI(headObj,3);

      GUILayout.Label("body", GUILayout.Width(labelSize));
      GameObject bodyObj = ClientScene.FindLocalObject(playerItems.indexItems(4));
      ItemGUI(bodyObj,4);

      GUILayout.Label("shoes", GUILayout.Width(labelSize));
      GameObject shoesObj = ClientScene.FindLocalObject(playerItems.indexItems(5));
      ItemGUI(shoesObj,5);

      //Column 2 End
      GUILayout.EndVertical();

      GUILayout.BeginVertical();
      GUILayout.Label("test", GUILayout.Width(labelSize));
      GUILayout.Label("test", GUILayout.Width(labelSize));
      GUILayout.Label("test", GUILayout.Width(labelSize));
      GUILayout.EndVertical();
      //End Equip Menu
      GUILayout.EndHorizontal();
   }


   void CheckMouseOver()
   {
      if (bar.Contains(Input.mousePosition))
      {
         m_mouseOnMenu = true;
      }
      else
      {
         m_mouseOnMenu = false;
      }

      if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip)
      {
         if (lastTooltip != "")
            //SendMessage("OnMouseOut", SendMessageOptions.DontRequireReceiver);
            showInfo = false;
         if (GUI.tooltip != "")
            //SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
            showInfo = true;
         lastTooltip = GUI.tooltip;
      }
   }



   void ShowInfo()
   {
      //Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
      //GUIUtility.ScreenToGUIPoint()
      //Debug.Log("OnMouseOver " + Event.current.mousePosition + " " + Input.mousePosition);
     // int tempDepth = GUI.depth;
      //GUI.depth = 1;
      Vector2 textSize = skin.box.CalcSize(new GUIContent(lastTooltip));
      GUI.TextArea(new Rect(Event.current.mousePosition.x + 25, Event.current.mousePosition.y + 25, textSize.x, textSize.y), lastTooltip);
      // GUILayout.BeginArea(new Rect(Event.current.mousePosition.x + 25, Event.current.mousePosition.y + 25, 100, 100));
      //GUILayout.TextArea(lastTooltip);
      // GUILayout.EndArea();
     // GUI.depth = tempDepth;
   }

   void PickUpSelfItems(GameObject item, int itemIdx)
   {
      hodingItem = item;
      cursorIcon = item.GetComponent<ItemInfo>().icon;
      pickUpSelfItem = true;
   }

   void ResetPickUp()
   {
      hodingItem = null;
      pickUpSelfItem = false;
   }

   void DrewCursor()
   {
      GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 45, 45), cursorIcon);
   }

   void ItemGUI(GameObject item,int index)
   {
      if (item != null)
      {
         Texture2D icon = item.GetComponent<ItemInfo>().icon;

         GUIContent c = new GUIContent(icon, item.GetComponent<ItemInfo>().getDescription());
         if (GUILayout.Button(c, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)) && isLocalPlayer)
         {
            if (Input.GetMouseButtonUp(0))
            {
               if (pickUpSelfItem)
               {
                  //TODO change item
                  Debug.Log("change item");
                  //Change equip
                  playerItems.CmdChangeItem(index, item,hodingItem);
                  ResetPickUp();
               }
               else if(!playerItems.IsEmptyItem(item.GetComponent<NetworkIdentity>().netId))
               {
                  PickUpSelfItems(item, index);
               }

            }
            
         }
      }
      else
      {
         if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
         {
            Debug.Log("empty");
         }
      }
   }
}
