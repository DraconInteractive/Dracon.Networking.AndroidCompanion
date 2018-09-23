using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkDiscovery : NetworkDiscovery {

	public override void OnReceivedBroadcast(string fromAddress, string data) {
		GetComponent<NetworkClientUI>().Connect(fromAddress);
		StopBroadcast();
	}
}
