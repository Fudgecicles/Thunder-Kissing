﻿using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {

    Cupid cupidScript;
    Lover loverScript;
    CharacterMotor motor;
    public bool cupid = true;
    GUIHandler handler;
    GameObject arrow;

	// Use this for initialization
	void Start () {
        arrow = transform.Find("Main Camera").Find("Arrows").gameObject;
        handler = GetComponent<GUIHandler>();
        motor = GetComponent<CharacterMotor>();
        cupidScript = GetComponent<Cupid>();
        loverScript = GetComponent<Lover>();

		Screen.lockCursor = true;
		//Screen.showCursor = false;
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
                    Debug.Log("this haps");
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    for (int k = 0; k < players.Length; k++)
                    {
                        players[k].GetComponent<PlayerManager>().photonView.RPC("playerHit", PhotonTargets.AllBuffered, col.GetComponent<Arrow>().id, PhotonNetwork.player.ID);
                    }
                    PhotonNetwork.Instantiate("Prefabs/prf_love_spurt", col.transform.FindChild("GameObject").transform.position, (col.transform.rotation), 0);
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

    public void toLover()
    {
        if (cupid)
        {
            cupidScript.enabled = false;
            loverScript.isLover = true;
            motor.cupid = false;
            cupid = false;
            handler.updateState(false);
            arrow.SetActive(false);
        }
    }

    public  void toCupid()
    {
        if (!cupid)
        {
            cupidScript.enabled = true;
            loverScript.isLover = false;
            cupidScript.numArrows = 3;
            motor.cupid = true;
            cupid = true;
            handler.updateState(true);
            arrow.SetActive(true);
        }
    }
}
