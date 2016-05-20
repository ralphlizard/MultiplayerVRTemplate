using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VRNetworkPlayer : NetworkBehaviour {

	public Camera mainCam;
	public AudioListener listener;
	public GameObject reticle;
	public enum ReticleVisible 
	{
		None,
		PlayerOnly,
		All
	};
	public ReticleVisible reticleVisible;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		if (isLocalPlayer) {
			listener.enabled = true;
			mainCam.enabled = true;
		} 
		else 
		{
			listener.enabled = false;
			mainCam.enabled = false;
		}

		if (reticleVisible == ReticleVisible.None) 
		{
			reticle.SetActive (false);
		} 
		else if (reticleVisible == ReticleVisible.PlayerOnly) 
		{
			if (isLocalPlayer) 
			{
				reticle.SetActive (true);
			} 
			else 
			{
				reticle.SetActive (false);
			}
		} 
		else 
		{
			reticle.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}