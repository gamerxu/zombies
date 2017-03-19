using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;


class TPlayerController : NetworkBehaviour
{

   [SerializeField]
   private float m_moveSpeed = 5;
   public float moveSpeed { get { return m_moveSpeed; } }

   [SerializeField]
   private float m_syncPositionThreshold = 0.25f;
   public float syncPositionThreshold { get { return m_syncPositionThreshold; } }

   [SerializeField]
   private int m_rotationSpeed = 15;
   public int rotationSpeed { get { return m_rotationSpeed; } }

   [SerializeField]
   private float m_camDistance = 6f;

   private Vector3 cameraRotate = new Vector3(45, -180, 0);

   void Awake()
   {

   }

   void Start()
   {
      if (isLocalPlayer)
      {
         Camera.main.transform.LookAt(this.transform.position);
         Camera.main.transform.position = new Vector3(this.transform.position.x, 0 + 4f, this.transform.position.z + 4f);
         Camera.main.transform.rotation = Quaternion.Euler(cameraRotate);

         GetComponent<Play_syncRotation>().Transmit(transform.transform.position, transform.rotation);

         CmdEquip(this.netId);

      }

   }

   public void Spawn()
   {
      Debug.Log("Spawn");
   }

   void Update()
   {
      if (!isLocalPlayer)
      {
         return;
      }

      float h = CrossPlatformInputManager.GetAxis("Horizontal");
      float v = CrossPlatformInputManager.GetAxis("Vertical");

      if (h != 0 || v != 0)
      {
         GetComponent<Animator>().SetBool("Run", true);
      }
      else if (h == 0 && v == 0)
      {
         GetComponent<Animator>().SetBool("Run", false);
      }

      // transFormValue = (v * Vector3.forward + h * Vector3.right) * Time.deltaTime;
      Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
      Vector3 transFormValue = v * m_CamForward + h * Camera.main.transform.right;
      float distance = 0;
      if (transFormValue.x != 0 || transFormValue.z != 0)
      {
         distance = Vector3.Distance(transform.position, transFormValue);


         Quaternion lerpRota = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(transFormValue), Time.deltaTime * 15);
         if (transform.localRotation != lerpRota)
         {
            transform.localRotation = lerpRota;

         }

         transFormValue *= Time.deltaTime;


         transform.Translate(transFormValue * m_moveSpeed, Space.World);
         if (distance >= m_syncPositionThreshold)
         {
            GetComponent<Play_syncRotation>().Transmit(transform.transform.position, transform.localRotation);
         }

      }
      //Camera.main.transform.LookAt(this.transform);
      Camera.main.transform.position = new Vector3(this.transform.position.x, 0 + m_camDistance, this.transform.position.z + m_camDistance);
   }


   [Command]
   void CmdEquip(NetworkInstanceId parentId)
   {

      GameObject weapon = Resources.Load("prefeb/items/weapons/M4A1/M4A1") as GameObject;
      //Debug.Log(weapon.transform.rotation);
      GameObject weapon_clone = GameObject.Instantiate(weapon, weapon.transform.position, weapon.transform.rotation);
      weapon_clone.GetComponent<GunController>().gunQuaternion = weapon_clone.transform.rotation;
      weapon_clone.GetComponent<GunController>().parentNetId = parentId;
      weapon.transform.localRotation = weapon.transform.rotation;
      weapon_clone.SetActive(true);

      //NetworkServer.Spawn(weapon_clone);
      NetworkServer.SpawnWithClientAuthority(weapon_clone, connectionToClient);


   }

}

