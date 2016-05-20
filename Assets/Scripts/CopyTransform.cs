using UnityEngine;
using System.Collections;

public class CopyTransform : MonoBehaviour {
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.localPosition = target.localPosition;
			transform.localRotation = target.localRotation;
		}
	}
}
