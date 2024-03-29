﻿using UnityEngine;
using System.Collections;

public class NetworkingInitialization : MonoBehaviour {

    NetworkVariables variables;
    GameObject monster;
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
        if (PhotonNetwork.isMasterClient)
        {
            variables = PhotonNetwork.Instantiate("Prefabs/NetworkVariables", Vector3.zero, Quaternion.identity, 0).GetComponent<NetworkVariables>();
        }
        monster = PhotonNetwork.Instantiate("Prefabs/prf_Cupid", Vector3.zero, Quaternion.identity, 0);

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
        StartCoroutine(dumbFunction());
	}

    IEnumerator dumbFunction()
    {
        yield return null;
        if (!PhotonNetwork.isMasterClient)
        {
            variables = GameObject.Find("NetworkVariables(Clone)").GetComponent<NetworkVariables>();
        }
        if (variables.lovers < 2 && variables.cupids < 2)
        {
            int rand = Random.Range(0, 2);
            Debug.Log(rand);
            if (rand == 0)
            {
                monster.GetComponent<Cupid>().enabled = true;
                variables.incrementCupids();
                monster.GetComponent<CharacterMotor>().cupid = true;
                monster.GetComponent<PlayerManager>().cupid = true;
            }
            else
            {
                monster.GetComponent<Lover>().isLover = true;
                variables.incrementLovers();
                monster.GetComponent<CharacterMotor>().cupid = false;
                monster.GetComponent<PlayerManager>().cupid = false;

            }
        }
        else if (variables.lovers < 2)
        {
            monster.GetComponent<Lover>().isLover = true;
            variables.incrementLovers();
            monster.GetComponent<CharacterMotor>().cupid = false;
            monster.GetComponent<PlayerManager>().cupid = false;


        }
        else if (variables.cupids < 2)
        {
            monster.GetComponent<Cupid>().enabled = true;
            variables.incrementCupids();
            monster.GetComponent<CharacterMotor>().cupid = true;
            monster.GetComponent<PlayerManager>().cupid = true;

        }
        else
        {

        }
    }
    
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
