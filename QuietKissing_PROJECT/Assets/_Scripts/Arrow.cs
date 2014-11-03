using UnityEngine;
using System.Collections;

public class Arrow : Photon.MonoBehaviour {

    public bool stuck = false;
    public int id;
    float maxSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!stuck)
        {
            rigidbody.velocity += 5f*Time.deltaTime *Vector3.down;
        }
        
        RaycastHit info;
        if (Physics.Raycast(transform.position, rigidbody.velocity,out info, rigidbody.velocity.magnitude*Time.deltaTime )){
            if (info.transform.tag == "Floor")
            {
                transform.position = info.point;
                rigidbody.velocity = Vector3.zero;
                stuck = true;
                GameObject particle = transform.FindChild("Particle System").gameObject;
                particle.SetActive(true);
                particle.transform.LookAt(Vector3.up);

            }
        }
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, Time.deltaTime);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Floor")
        {
            stuck = true;
            rigidbody.velocity = Vector3.zero;
            GameObject particle = transform.FindChild("Particle System").gameObject;
            particle.SetActive(true);
            particle.transform.LookAt(Vector3.up);

        }
    }
    public void setSpeed(float power, int id)
    {
        photonView.RPC("setSpeedRPC", PhotonTargets.AllBuffered, power, id);
    }

    [RPC]
    void setSpeedRPC(float power, int id)
    {
        this.id = id;
        rigidbody.velocity = power/3 * maxSpeed*transform.forward;
    }
}
