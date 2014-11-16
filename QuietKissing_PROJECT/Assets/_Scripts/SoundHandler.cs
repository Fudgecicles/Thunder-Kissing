using UnityEngine;
using System.Collections;

public class SoundHandler : Photon.MonoBehaviour {

    AudioSource[] sources;
    int current = 0;
    bool grounded = true;

	// Use this for initialization
	void Start () {
        sources = GetComponents<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (grounded&&(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            if (!sources[current].isPlaying)
            {
                PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, true);
            }
        }
        else
        {
            PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grounded = false;
            PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, false);
        }
	}

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        grounded = true;
        if (col.transform.tag == "Floor")
        {
            if (col.gameObject.layer == 8)
            {
                if (current != 1)
                {
                    if (sources[current].isPlaying)
                    {
                        PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, false);
                        current = 1;
                        PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, true);
                    }
                }
            }
            else
            {
                if (current != 0)
                {
                    if (sources[current].isPlaying)
                        PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, false);
                        current = 0;
                        PhotonNetwork.RPC(photonView, "NetworkSounds", PhotonTargets.AllBuffered, current, true);
                }
            }
        }
    }

    [RPC]
    void NetworkSounds(int index, bool start)
    {
        if (start)
        {
            sources[index].Play();
        }
        else
        {
            sources[index].Stop();
        }
    }
}
