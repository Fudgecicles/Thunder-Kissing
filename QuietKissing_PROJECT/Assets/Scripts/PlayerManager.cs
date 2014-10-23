using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public int numPlayers=0;
    public GameObject[] players; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int addPlayer(GameObject player)
    {
        players[numPlayers] = player;
        return numPlayers++;
    }
}
