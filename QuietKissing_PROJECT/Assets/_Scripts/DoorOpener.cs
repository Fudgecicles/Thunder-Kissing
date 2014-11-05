using UnityEngine;
using System.Collections;

public class DoorOpener : Photon.MonoBehaviour {

    bool canOpen = false;
    bool open = false;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (canOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                photonView.RPC("openRPC", PhotonTargets.AllBuffered);
            }
        }
	}

    [RPC]
    public void openRPC()
    {
        if (!open)
        {
            rigidbody.AddForce(-transform.forward * 300);
            open = true;

        }
        else
        {
            rigidbody.AddForce(transform.forward * 300);
            open = false;
        }
        StartCoroutine(lockPos());
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            canOpen = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            canOpen = false;
        }
    }

    IEnumerator lockPos()
    {
        yield return new WaitForSeconds(.1f);
        rigidbody.velocity = Vector3.zero;
    }


}
