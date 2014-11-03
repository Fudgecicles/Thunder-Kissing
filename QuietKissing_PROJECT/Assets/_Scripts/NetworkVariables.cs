using UnityEngine;
using System.Collections;

public class NetworkVariables : Photon.MonoBehaviour {

    public int cupids;
    public int lovers;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void incrementCupids()
    {
        photonView.RPC("incrementCupidsRPC", PhotonTargets.AllBuffered);
    }

    public void incrementLovers()
    {
        photonView.RPC("incrementLoversRPC", PhotonTargets.AllBuffered);
    }

    [RPC]
    void incrementCupidsRPC()
    {
        ++cupids;
    }

    [RPC]
    void incrementLoversRPC()
    {
        ++lovers;
    }

}
