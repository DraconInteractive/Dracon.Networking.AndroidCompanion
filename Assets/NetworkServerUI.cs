using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityStandardAssets.CrossPlatformInput;


public class NetworkServerUI : MonoBehaviour {

	string ipaddress = "";
	void OnGUI () {
		
		GUI.Box (new Rect(10, Screen.height - 50, 100, 50), ipaddress);
		GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + NetworkServer.active);
		GUI.Label(new Rect(20, Screen.height - 20, 100, 20), "Connected: " + NetworkServer.connections.Count);
	}

	void Start () {
		ipaddress = LocalIPAddress();
		NetworkServer.Listen(25000);
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
}
