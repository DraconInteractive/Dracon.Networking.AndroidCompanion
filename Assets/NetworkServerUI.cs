using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class NetworkServerUI : MonoBehaviour {
	
	CrossPlatformInputManager.VirtualAxis m_HVAxis; 
	CrossPlatformInputManager.VirtualAxis m_VVAxis; 
	string horizontalAxisName = "Horizontal"; 
	string verticalAxisName = "Vertical";

	public static float h, v;

	public GameObject car;

	float locationTimer = 0;
	void OnGUI()
    {
        string ipaddress = LocalIPAddress();
        GUI.Box(new Rect(10,Screen.height - 50, 100,50),ipaddress);
        GUI.Label(new Rect(20,Screen.height - 35, 100,20), "Status:" + NetworkServer.active);
        GUI.Label(new Rect(20,Screen.height - 20, 100,20), "Connected:" + NetworkServer.connections.Count);

		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.1f, 200, 100), h.ToString() + " | " + v.ToString());
    }

	// Use this for initialization
	void Start () {
		
		//m_HVAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
		//CrossPlatformInputManager.RegisterVirtualAxis(m_HVAxis);
		//m_VVAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
		//CrossPlatformInputManager.RegisterVirtualAxis(m_VVAxis);
		
		NetworkServer.Listen(25000);
		NetworkServer.RegisterHandler(888, ServerReceiveMessage);
		
	}

	private void ServerReceiveMessage(NetworkMessage message)
	{
		StringMessage msg = new StringMessage ();
		msg.value = message.ReadMessage<StringMessage>().value;

		string[] deltas = msg.value.Split('|');
		//m_HVAxis.Update(Convert.ToSingle(deltas[0])); 
		//m_VVAxis.Update(Convert.ToSingle(deltas[1]));

		h = Convert.ToSingle(deltas[0]);
		v = Convert.ToSingle(deltas[1]);
	}	

	// Update is called once per frame
	void Update () {
		locationTimer += Time.deltaTime;
		if (locationTimer > 0.5f) {
			locationTimer = 0;
			SendLocation();
			print ("Sending Location");
		}
	}

    public string LocalIPAddress()
     {
         IPHostEntry host;
         string localIP = "";
         host = Dns.GetHostEntry(Dns.GetHostName());
         foreach (IPAddress ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 localIP = ip.ToString();
                 //break;
             }
			 print ("IP: " + ip.ToString());
         }
         return localIP;
     }

	 public void SendLocation () {
		if(NetworkServer.connections.Count > 0)
		{
			StringMessage msg = new StringMessage();
			msg.value = car.transform.position.x + "|" + car.transform.position.y + "|" + car.transform.position.z;

			NetworkServer.SendToAll(889, msg);
		}
	 }
}

