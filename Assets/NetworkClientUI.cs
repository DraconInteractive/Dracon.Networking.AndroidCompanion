using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class NetworkClientUI : MonoBehaviour {

	NetworkClient client;

	string ipenter = "";
	void OnGUI () {
		string ipaddress = LocalIPAddress();
		GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
		GUI.Label(new Rect(20, Screen.height - 30, 100, 20), "Status: " + client.isConnected);

		if (!client.isConnected) {
			ipenter = GUI.TextField(new Rect(50, Screen.height * 0.1f, 120, 50), ipenter);
			if (GUI.Button(new Rect(50, Screen.height * 0.15f, 120, 100), "Connect")) {
				Connect();
			}
		}

		if (ipenter != "") {
			GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 250, 100), ipenter);
		}
	}
	// Use this for initialization
	void Start () {
		client = new NetworkClient();
	}
	
	void Connect () {
		client.Connect(ipenter, 25000);
	}
	// Update is called once per frame
	void Update () {
		
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
                 break;
             }
         }
         return localIP;
     }
}
