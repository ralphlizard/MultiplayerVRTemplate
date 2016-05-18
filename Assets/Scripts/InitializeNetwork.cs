using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace UnityEngine.Networking
{
	public class InitializeNetwork : MonoBehaviour
	{
		public NetworkManager manager;

		void Awake()
		{
			DontDestroyOnLoad (gameObject);
			manager = GetComponent<NetworkManager>();
			//enable match maker
			manager.StartMatchMaker();

			if (manager.matches == null) {
				//create internet match
				manager.matchMaker.CreateMatch ("default", 4, true, "", manager.OnMatchCreate);
			}
			/*
			if (!isPlayer1) {
				//find internet match
				manager.matchMaker.ListMatches (0, 20, "", manager.OnMatchList);
				if (manager.matches != null) {

					//join match
					Match.MatchDesc match = manager.matches.ToArray()[0];
					manager.matchName = "default";
					manager.matchSize = 4;
					manager.matchMaker.JoinMatch (match.networkId, "", manager.OnMatchJoined);
				}
			}
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
	}
};
