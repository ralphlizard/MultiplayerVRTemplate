using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.VR;

namespace UnityEngine.Networking
{
	public class InitializeNetwork : MonoBehaviour
	{
		public NetworkManager manager;
		public GameObject cardboardPlayer;
		public GameObject oculusPlayer;
		public GameObject gearPlayer;

		void Awake()
		{
			DontDestroyOnLoad (gameObject);
			manager = GetComponent<NetworkManager>();
			//enable match maker
			manager.StartMatchMaker();

			#if UNITY_EDITOR 
			manager.playerPrefab = cardboardPlayer;
			VRSettings.enabled = true;
			#endif

			#if UNITY_5_3
			manager.playerPrefab = oculusPlayer;
			VRSettings.enabled = true;
			#endif

			#if UNITY_ANDROID
			manager.playerPrefab = cardboardPlayer;
			#endif

			/*
			#if UNITY_IOS
			manager.playerPrefab = cardboardPlayer;
			VRSettings.enabled = false;
			#endif
			*/
		}

		void Update()
		{
		if (Input.GetKey (KeyCode.R)) {
				ResetGame ();
			}
		}

		public void ResetGame()
		{
		
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		}

		public void CreateServer()
		{
			//create internet match
			manager.matchMaker.CreateMatch ("default", 4, true, "", manager.OnMatchCreate);
			print ("Server created");

		}

		public void JoinServer()
		{
			if (manager.matches != null) { //check for server first
				var match = manager.matches[manager.matches.Count-1]; //the last is the most recent
				manager.matchName = match.name;
				manager.matchSize = (uint)match.currentSize;
				manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
				print ("Server joined");
			}
		}
	}
};

