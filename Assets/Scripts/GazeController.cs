using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GazeController : NetworkBehaviour {

	public Camera mainCamera;
	public GameObject headedSlug;
	public GameObject headlessSlug;
	public AudioListener listener;
	public OVRCameraRig OculusCameraRig;
	public int solved;
	private GameObject gazeTarget;
	private RaycastHit hit;
	private Vector3 fwd;
	private Transform spawn;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		if (isLocalPlayer)
		{
			OculusCameraRig.enabled = true;
			listener.enabled = true;
			headedSlug.SetActive(false);
			headedSlug.SetActive(false);
			mainCamera.enabled = true;
//			mainCamera.tag = "MainCamera";
//			this.gameObject.tag = "Player";
		}
		else
		{
			headlessSlug.SetActive (false);
//			mainCamera.tag = "Untagged";		
		}
	}
	
	// Update is called once per frame
	void Update () {
		//raycast hits object
		fwd = mainCamera.transform.TransformDirection (Vector3.forward);
		if (gazeTarget == null &&
		    Physics.Raycast (mainCamera.transform.position, fwd, out hit, 100) && 
			hit.collider.tag == "GazeAware" &&
			hit.collider != GetComponent<Collider>())
		{
			gazeTarget = hit.collider.gameObject;
			gazeTarget.SendMessageUpwards ("LookedAt");
			gazeTarget.SendMessageUpwards ("AttachGazeController", this);
			
		}
		else if (gazeTarget != null &&
		         Physics.Raycast (mainCamera.transform.position, fwd, out hit, 100) &&
		         hit.collider.gameObject.name == gazeTarget.name)
		{
			gazeTarget.SendMessageUpwards ("LookedAt");
		}
//		print ("Gazing " + gazeTarget != null);
	}

	public void GazeRelease ()
	{
		gazeTarget = null;
	}

	public void Solve ()
	{
		print ("Solved" + solved);
		solved++;
	}

}