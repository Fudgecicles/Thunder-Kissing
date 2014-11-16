using UnityEngine;
using System.Collections;

public class NetworkingInitialization : MonoBehaviour {

    NetworkVariables variables;
    GameObject monster;
    GUIHandler handler;
    GameObject loverSpawn;
    GameObject[] cupidSpawns;
	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings(".1");
        loverSpawn = GameObject.Find("LoverSpawn");
        cupidSpawns = GameObject.FindGameObjectsWithTag("Spawn");
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
        GameObject temp = monster.transform.Find("Main Camera").gameObject;
        temp.transform.Find("Timer").gameObject.SetActive(true);
        temp.transform.Find("Score").gameObject.SetActive(true);
        temp.transform.Find("State").gameObject.SetActive(true);
        temp.transform.Find("Arrows").gameObject.SetActive(true);
        temp.transform.Find("end").gameObject.SetActive(true);
	}

    IEnumerator dumbFunction()
    {
        yield return null;
        monster.transform.Find("Main Camera").Find("end").gameObject.SetActive(false);

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
                monster.transform.position = cupidSpawns[Random.Range(0, cupidSpawns.Length)].transform.position;
                monster.GetComponent<Cupid>().enabled = true;
                variables.incrementCupids();
                monster.GetComponent<CharacterMotor>().cupid = true;
                monster.GetComponent<PlayerManager>().toCupid();
                
            }
            else
            {
                monster.transform.position = loverSpawn.transform.position;
                monster.GetComponent<Lover>().isLover = true;
                variables.incrementLovers();
                monster.GetComponent<CharacterMotor>().cupid = false;
                monster.GetComponent<PlayerManager>().toLover();

            }
        }
        else if (variables.lovers < 2)
        {
            monster.transform.position = cupidSpawns[Random.Range(0, cupidSpawns.Length)].transform.position;
            monster.GetComponent<Lover>().isLover = true;
            variables.incrementLovers();
            monster.GetComponent<CharacterMotor>().cupid = false;
            monster.GetComponent<PlayerManager>().toLover();


        }
        else if (variables.cupids < 2)
        {
            monster.transform.position = loverSpawn.transform.position;
            monster.GetComponent<Cupid>().enabled = true;
            variables.incrementCupids();
            monster.GetComponent<CharacterMotor>().cupid = true;
            monster.GetComponent<PlayerManager>().toCupid();

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
