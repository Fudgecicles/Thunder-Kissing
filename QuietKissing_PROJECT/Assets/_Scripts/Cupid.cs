using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// by: steven
// my 1+only
// 40
//haybpperydirhty day
public class Cupid : Photon.MonoBehaviour {

    public int numArrows = 3;
    float power=1;
    GameObject shotLocation;
    List<GameObject> arrows = new List<GameObject>();
    GameObject camera;
    GUIHandler handler;
    public AudioSource twang;
	
	void Start () {
        shotLocation = transform.Find("shotLocation").gameObject;
        camera = transform.FindChild("Main Camera").gameObject;
        handler = GetComponent<GUIHandler>();
    }

	void Update () {
        if (photonView.isMine)
        {
            if (numArrows > 0)
            {
                if (Input.GetMouseButton(0))
                {
                    chargeShot();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    fireShot();
                    
                }
            }
        }
	}

    void chargeShot()
    {
        if (power > 3)
        {
            power = 3;
        }
        else if (power < 3)
        {
            power += Time.deltaTime;
        }
    }

    void fireShot()
    {
        GameObject arrow;
        arrow =  PhotonNetwork.Instantiate("Prefabs/Arrow", shotLocation.transform.position, camera.transform.rotation, 0);
        arrow.GetComponent<Arrow>().setSpeed(power, PhotonNetwork.player.ID);
        arrow.GetComponent<Arrow>().id = PhotonNetwork.player.ID;
        power = 1;
        --numArrows;
        handler.updateArrows(numArrows);
    }

    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Particle")
        {
            Arrow a = col.transform.parent.GetComponent<Arrow>();
            if (a.stuck)
            {
                PhotonNetwork.Destroy(col.transform.parent.gameObject);
                ++numArrows;
                handler.updateArrows(numArrows);
            }
            else
            {
                
            }
        }
    }

}
