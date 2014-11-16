using UnityEngine;
using System.Collections;

public class Lover : MonoBehaviour {
	public Renderer myMeshRenderer;
	public Material mat_default;
	public Material mat_kissing;

	public AudioSource sfx_kiss; // looping kissing noise that plays while kissing
	public AudioSource[] sfx_kissMutualMusic; // looping kissmutual music that plays while kissing mutually
	public Vector4 kissingBounds; // clamping of the mouse look while kissing (x:min/max, y:min/max)

	private Vector4 originalBounds;
	private MouseLook[] mouseLooks;
	private PhotonView photonView;
    private GameObject particles;
    int source = 0;
    public bool isLover = false;
    GameObject school;
    GameObject locker;


	private bool m_isKissing = false;
	public bool IsKissing {
		get { return m_isKissing; }
	}

	private float m_score;
	public int Score {
		get { return (int)m_score; }
	}

	private bool m_isKissingMutual = false;
	public bool IsKissingMutual {
		get { return m_isKissingMutual; }
	}

	private float durationOfCurrentMutualKiss = 0;
	public Lover currentlyKissingThisLover;

    GUIHandler handler;

	void Awake() {
        school = GameObject.Find("School");
        locker = GameObject.Find("Locker");
        handler = GetComponent<GUIHandler>();

		photonView = GetComponent<PhotonView> ();

		// grab necessary mouse look information
		mouseLooks = GetComponentsInChildren<MouseLook> ();
		foreach (MouseLook m in mouseLooks) {
			if (m.axes == MouseLook.RotationAxes.MouseX) {
				originalBounds.x = m.minimumX;
				originalBounds.y = m.maximumX;
			}
			if (m.axes == MouseLook.RotationAxes.MouseY) {
				originalBounds.z = m.minimumY;
				originalBounds.w = m.maximumY;
			}
		}
	}
	
	void Update () {
        if (isLover)
        {
            if (photonView.isMine)
            {
                float distance = Mathf.Max(Vector3.Distance(transform.position, school.transform.position), Vector3.Distance(transform.position, locker.transform.position));
                if (distance < 50)
                {
                    if (transform.position.y > 0)
                    {
                        source = 2;
                    }
                    else
                    {
                        source = 1;
                    }
                }
                else
                {
                    if (transform.position.y > -5)
                    {
                        source = 2;
                    }
                    source = 0;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    enterKiss();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    exitKiss();
                }

                updateKissMutual();
            }
        }
	}

	// shouldClamp: if true, then x axis will clamp
	// NOTE: right now, the clamping is relative only on the x, clamping is still absolute on y
	// maybe change that? for now it's a-okay
	void setMouseLooksTo (Vector4 mouseBounds, bool shouldClamp) {
		foreach (MouseLook m in mouseLooks) {
			if (m.axes == MouseLook.RotationAxes.MouseX) {
				m.minimumX = m.currRotationX + mouseBounds.x;
				m.maximumX = m.currRotationX + mouseBounds.y;
			}
			if (m.axes == MouseLook.RotationAxes.MouseY) {
				m.minimumY = mouseBounds.z;
				m.maximumY = mouseBounds.w;
			}

			m.clampX = shouldClamp; // set whether or not mouse x should clamp
		}
	}

	void enterKiss() { // personal
		setMouseLooksTo (kissingBounds, true); // clamp mouse a bit
		GetComponent<CharacterMotor> ().canControl = false;

		photonView.RPC("enterKissRPC", PhotonTargets.AllBuffered);
	}

	[RPC]
	void enterKissRPC() {
		print (gameObject.name + " has entered a kiss!");
		m_isKissing = true;
		// [] stop this player from being able to move around
		sfx_kiss.Play (); // play looping kissing sound
		myMeshRenderer.material = mat_kissing;
	}

	void exitKiss() { // personal
		setMouseLooksTo (originalBounds, false); // unclamp mouse
		GetComponent<CharacterMotor> ().canControl = true;

		photonView.RPC("exitKissRPC", PhotonTargets.AllBuffered);
	}

	[RPC]
	void exitKissRPC() {
		print (gameObject.name + " has exited a kiss!");
		m_isKissing = false;
		sfx_kiss.Stop (); // stop looping kissing sound
		myMeshRenderer.material = mat_default;
	}

	void updateKissMutual() {
		if (m_isKissingMutual) {
			photonView.RPC ("updateKissMutualRPC", PhotonTargets.AllBuffered);
            handler.updateScore(Score);
			/* i have a suspicion that it's stupid do call an rpc in update
			 * what's the better way to do this
			 * idunno */
		}
	}

	[RPC]
	void updateKissMutualRPC() {
		durationOfCurrentMutualKiss += Time.deltaTime; // keep track of how long the kiss has been going
		m_score += Constants.s.rateOfKiss * Time.deltaTime; // update score according to the kiss
	}

	/* wait should this be an rpc or what? */
	// this is going to be called by a collider in ONE person's simulation 
	public void enterKissMutual(Lover loverImKissing) {
		if (m_isKissingMutual) {
			print ("i'm already kissing mutual!");
			return;
		}
		// [x] track lover you're currently kissing
		currentlyKissingThisLover = loverImKissing;
		// [x] play new song/sound or watevs
		//sfx_kissMutualMusic.Play ();

		// [] change screen to look all kissy and stuff

		photonView.RPC ("enterKissMutualRPC", PhotonTargets.AllBuffered); // call rpc
        particles = (GameObject)Instantiate(Resources.Load("Prefabs/prf_party_love"), transform.position, Quaternion.EulerAngles(new Vector3(-90,0,0)));
	}

	[RPC]
	void enterKissMutualRPC() {
		m_isKissingMutual = true; // set is kissing mutual to true
		if (photonView.isMine) sfx_kissMutualMusic[source].Play ();
	}

	// this is going to be called by a collider in ONE person's simulation
	public void exitKissMutual() {
		if (!m_isKissingMutual) {
			print ("i'm already not kissing mutual!");
			return;
		}
		photonView.RPC ("exitKissMutualRPC", PhotonTargets.AllBuffered); // call rpc

		// [x] stop song
		//sfx_kissMutualMusic.Pause ();
		// [] reset screen

		currentlyKissingThisLover.exitKissMutual (); // exit the kiss of the other lover
													// this won't cause a loop because of the if-statement at the start
		// [x] set currently-kissing-lover to null
		currentlyKissingThisLover = null;
        Destroy(particles);
	}

	[RPC]
	void exitKissMutualRPC() {
		m_isKissingMutual = false;
		if (photonView.isMine) sfx_kissMutualMusic[source].Pause ();
        if (particles != null)
        {
            Destroy(particles);
        }
	}
}
