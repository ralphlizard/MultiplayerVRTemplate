using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CardboardNetwork : NetworkBehaviour {

	public Camera[] attachedCams;
	public GameObject reticle;
	public Transform dummyCardboard;
	public enum ReticleVisible 
	{
		None,
		PlayerOnly,
		All
	};
	public ReticleVisible reticleVisible;
	public GameObject cardboardPrefab;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		attachedCams = GetComponentsInChildren<Camera> ();
		if (isLocalPlayer) {
			GameObject cardboard = Instantiate (cardboardPrefab);
			cardboard.transform.parent = this.transform;
			cardboard.transform.localPosition = new Vector3 (0, 0, 0);
			cardboard.transform.localEulerAngles = new Vector3 (0, 0, 0);
			dummyCardboard.GetComponent<CopyTransform>().target = cardboard.transform.GetChild (0);
			foreach (Camera cam in attachedCams)
				cam.enabled = true;
		} 
		else 
		{
			foreach (Camera cam in attachedCams)
				cam.enabled = false;
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