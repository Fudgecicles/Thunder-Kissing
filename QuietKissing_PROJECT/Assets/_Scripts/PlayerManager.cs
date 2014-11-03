using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {

    Cupid cupidScript;
    Lover loverScript;
    bool cupid = true;

	// Use this for initialization
	void Start () {
        cupidScript = GetComponent<Cupid>();
        loverScript = GetComponent<Lover>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Arrow")
        {
            if (!col.GetComponent<Arrow>().stuck)
            {
                if (!cupid)
                {
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    for (int k = 0; k < players.Length; k++)
                    {
                        players[k].GetComponent<PlayerManager>().photonView.RPC("playerHit", PhotonTargets.AllBuffered, col.GetComponent<Arrow>().id, PhotonNetwork.player.ID);
                    }
                }
            }
        }
    }

    [RPC]
    void playerHit(int shooterID, int hitID)
    {
        if (photonView.isMine)
        {
            if (PhotonNetwork.player.ID == shooterID)
            {
                toLover();
            }
            else if (PhotonNetwork.player.ID != hitID)
            {
                toCupid(); 
            }
        }
    }

    void toLover()
    {
        if (cupid)
        {
            cupidScript.enabled = false;
            loverScript.isLover = true;
        }
    }

    void toCupid()
    {
        if (!cupid)
        {
            cupidScript.enabled = true;
            loverScript.isLover = false;
            cupidScript.numArrows = 3;
        }
    }
}
