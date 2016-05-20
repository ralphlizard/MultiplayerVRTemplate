#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkHUDCustom : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;
		int trueOffsetX;
		int trueOffsetY;

		// Runtime variable
		bool showServer = false;

		void Awake()
		{
			trueOffsetX = offsetX * Screen.width / 20;
			trueOffsetY = offsetY * Screen.height / 20;
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "LAN Host(H)"))
				{
					manager.StartHost();
				}
				ypos += 2 * trueOffsetY;

				if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 5, trueOffsetY), "LAN Client(C)"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + trueOffsetX * 5, ypos, trueOffsetX * 10, trueOffsetY), manager.networkAddress);
				ypos += 2 * trueOffsetY;

				if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += 2 * trueOffsetY;
			}
			else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Server: port=" + manager.networkPort);
					ypos += trueOffsetY * 2;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					ypos += trueOffsetY * 2;
				}
			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);

					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += trueOffsetY * 2;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Stop (X)"))
				{
					manager.StopHost();
				}
				ypos += trueOffsetY * 2;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Enable Match Maker (M)"))
					{
						manager.StartMatchMaker();
					}
					ypos += trueOffsetY * 2;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Create Internet Match"))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += trueOffsetY * 2;

							GUI.Label(new Rect(xpos, ypos, trueOffsetX * 5, trueOffsetY), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos + trueOffsetX * 5, ypos, trueOffsetX * 5, trueOffsetY), manager.matchName);
							ypos += trueOffsetY * 2;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Find Internet Match"))
							{
								manager.matchMaker.ListMatches(0,trueOffsetY, "", manager.OnMatchList);
							}
							ypos += trueOffsetY * 2;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += trueOffsetY * 2;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Change MM server"))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += trueOffsetY * 2;
						if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += trueOffsetY * 2;
						if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += trueOffsetY * 2;
						if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += trueOffsetY * 2;

					GUI.Label(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += trueOffsetY * 2;

					if (GUI.Button(new Rect(xpos, ypos, trueOffsetX * 10, trueOffsetY), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += trueOffsetY * 2;
				}
			}
		}
	}
};
#endif //ENABLE_UNET
