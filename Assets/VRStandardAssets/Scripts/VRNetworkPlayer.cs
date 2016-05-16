using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VRNetworkPlayer : NetworkBehaviour {

	public Camera mainCamera;
	public AudioListener listener;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		if (isLocalPlayer) {
			listener.enabled = true;
			mainCamera.enabled = true;
		} else {
			listener.enabled = false;
			mainCamera.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}