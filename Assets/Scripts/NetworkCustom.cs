using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.VR;

public class NetworkCustom : NetworkManager {

	public enum TestType
	{
		Oculus,
		Cardboard
	};
	public enum PlayerType
	{
		Cardboard,
		Oculus,
		Gear
	};

	public TestType testType;
	public GameObject cardboardPlayer;
	public GameObject oculusPlayer;
	public GameObject gearPlayer;

	public class NetworkMessage : MessageBase {
		public PlayerType chosenClass;
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		NetworkMessage message = extraMessageReader.ReadMessage< NetworkMessage>();
		PlayerType selectedClass = message.chosenClass;

		Transform startPos = GetStartPosition ();

		if (selectedClass == PlayerType.Oculus) {
			GameObject player = Instantiate(oculusPlayer, startPos.position, startPos.rotation) as GameObject;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}

		if (selectedClass == PlayerType.Cardboard) {
			GameObject player = Instantiate(cardboardPlayer, startPos.position, startPos.rotation) as GameObject;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
	}

	public override void OnClientConnect(NetworkConnection conn) {
		NetworkMessage test = new NetworkMessage();
		test.chosenClass = ChooseCharacter ();
		ClientScene.AddPlayer(conn, 0, test);
	}

	PlayerType ChooseCharacter ()
	{
		Debug.Log("is server : "+ Network.isServer);
		Debug.Log("is client : "+ Network.isClient);
		#if UNITY_EDITOR
		if (testType == TestType.Oculus)
		{
			VRSettings.enabled = true;
			print("VR enabled : " + VRSettings.enabled);
			print("Editor Oculus chosen");
			return PlayerType.Oculus;
		}
		else if (testType == TestType.Cardboard)
		{
			VRSettings.enabled = false;
			print("Editor Cardboard chosen");
			return PlayerType.Cardboard;
		}
		#endif

		#if UNITY_STANDALONE
		VRSettings.enabled = true;
		print("Standalone Oculus chosen");
		return PlayerType.Oculus;
		#endif

		#if UNITY_ANDROID
		VRSettings.enabled = false;
		print("Android Cardboard chosen");
		return PlayerType.Cardboard;
		#endif

		#if UNITY_IOS
		VRSettings.enabled = false;
		print("iOS Cardboard chosen");
		return PlayerType.Cardboard;
		#endif
	}
}