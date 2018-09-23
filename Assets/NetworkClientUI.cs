using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.UI;

public class NetworkClientUI : MonoBehaviour {

	public static NetworkClient client;
	ClientNetworkDiscovery discovery;

	string ipe = "";
	
	static float joyH, joyV;

	public enum State {
		Joystick,
		Minimap
	};

	public State[] allStates = new State[] {
		State.Joystick,
		State.Minimap
	};

	public GameObject[] joystickItems;
	public GameObject[] minimapItems;

	public State currentState;
	public Vector3 playerlocation = new Vector3(0,0,0);
	public Vector3 playerRotation = new Vector3(0,0,0);

	public GameObject minimapMarker;
	public float markerSpeed, markerRotation;
	void OnGUI()
    {
        string ipaddress = LocalIPAddress();
        GUI.Box(new Rect(10,Screen.height - 50, 100,50),ipaddress);
        GUI.Label(new Rect(20,Screen.height - 30, 100,20), "Status:" + client.isConnected);

        if(!client.isConnected)
        {
			ipe = GUI.TextField (new Rect(Screen.width * 0.5f, Screen.height * 0.5f ,150,100), ipe);
        	if(GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.65f,60,50),"Connect"))
        	{
        		Connect();
        	}
        } else {
			GUI.Label(new Rect(100,Screen.height - 30, 100,20), "State:" + currentState.ToString());
			if (currentState == State.Joystick) {
				GUI.Label (new Rect(Screen.width * 0.5f, Screen.height * 0.5f ,150,100), joyH.ToString() + " | " + joyV.ToString());
			} else if (currentState == State.Minimap) {
				GUI.Label (new Rect(Screen.width * 0.5f, Screen.height * 0.5f ,150,100), playerlocation.x.ToString() + " | " + playerlocation.y.ToString() + " | " + playerlocation.z.ToString());
			}
		}
    }

	// Use this for initialization
	void Start () {
		client = new NetworkClient();
		client.RegisterHandler(889, ClientReceiveLocation);
		discovery = GetComponent<ClientNetworkDiscovery>();
		discovery.StartAsClient();
	}

	void Update () {
		minimapMarker.transform.position = Vector3.MoveTowards(minimapMarker.transform.position, playerlocation, markerSpeed * Time.deltaTime);
		minimapMarker.transform.rotation = Quaternion.RotateTowards (minimapMarker.transform.rotation, Quaternion.Euler(playerRotation), markerRotation * Time.deltaTime);
	}

	void Connect()
	{
		client.Connect(ipe, 25000);
	}

	public void Connect (string ipa) {
		client.Connect(ipa, 25000);
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

	static public void SendJoystickInfo(float hDelta, float vDelta)
	{
		if(client.isConnected)
		{
			StringMessage msg = new StringMessage();
			msg.value = hDelta + "|" + vDelta;
			joyH = hDelta; 
			joyV = vDelta;
			client.Send(888, msg);
		}
	}

	private void ClientReceiveLocation (NetworkMessage message) {
		StringMessage msg = new StringMessage ();
		msg.value = message.ReadMessage<StringMessage>().value;

		string[] deltas = msg.value.Split('|');
		//m_HVAxis.Update(Convert.ToSingle(deltas[0])); 
		//m_VVAxis.Update(Convert.ToSingle(deltas[1]));

		float x = Convert.ToSingle(deltas[0]);
		float y = Convert.ToSingle(deltas[1]);
		float z = Convert.ToSingle(deltas[2]);

		float rx = Convert.ToSingle(deltas[3]);
		float ry = Convert.ToSingle(deltas[4]);
		float rz = Convert.ToSingle(deltas[5]);
		playerlocation = new Vector3(x,y,z);
		playerRotation = new Vector3(rx, ry, rz);
	}

	public void ChangeState () {
		if (currentState == State.Joystick) {
			foreach (GameObject g in joystickItems) {
				g.SetActive(false);
			}
			foreach (GameObject g in minimapItems) {
				g.SetActive (true);
			}

			currentState = State.Minimap;
		} else {
			foreach (GameObject g in joystickItems) {
				g.SetActive(true);
			}
			foreach (GameObject g in minimapItems) {
				g.SetActive (false);
			}

			currentState = State.Joystick;
		}
	}
}

