using UnityEngine;
using System.Collections;

public class NetworkingInitialization : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings(".1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        GameObject monster = PhotonNetwork.Instantiate("Prefabs/prf_Cupid", Vector3.zero, Quaternion.identity, 0);
        CharacterController controller = monster.GetComponent<CharacterController>();
        controller.enabled = true;
        Camera cam = monster.GetComponentInChildren<Camera>();
        cam.enabled = true;
        FPSInputController input = monster.GetComponent<FPSInputController>();
        input.enabled = true;
        MouseLook look = monster.GetComponent<MouseLook>();
        look.enabled = true;
        AudioListener listener = monster.GetComponentInChildren<AudioListener>();
        listener.enabled = true;
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
