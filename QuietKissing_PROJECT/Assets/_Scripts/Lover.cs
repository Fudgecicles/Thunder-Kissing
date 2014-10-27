using UnityEngine;
using System.Collections;

public class Lover : MonoBehaviour {
	public AudioSource sfx_kiss; // looping kissing noise that plays while kissing
	public Vector4 kissingBounds; // clamping of the mouse look while kissing (x:min/max, y:min/max)

	private Vector4 originalBounds;
	private MouseLook[] mouseLooks;
	private PhotonView photonView;
	public bool isKissing; // should be private

	void Awake() {
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
		if (photonView.isMine) {
			if (Input.GetMouseButtonDown(0)) {
				enterKiss ();
			} else if (Input.GetMouseButtonUp (0)) {
				exitKiss ();
			}
		}
	}

	void setMouseLooksTo (Vector4 mouseBounds) {
		foreach (MouseLook m in mouseLooks) {
			if (m.axes == MouseLook.RotationAxes.MouseX) {
				m.minimumX = mouseBounds.x;
				m.maximumX = mouseBounds.y;
			}
			if (m.axes == MouseLook.RotationAxes.MouseY) {
				m.minimumY = mouseBounds.z;
				m.maximumY = mouseBounds.w;
			}
		}
	}

	void enterKiss() { // personal
		setMouseLooksTo (kissingBounds); // clamp mouse a bit

		photonView.RPC("enterKissRPC", PhotonTargets.AllBuffered);
	}

	[RPC]
	void enterKissRPC() {
		print (gameObject.name + " has entered a kiss!");
		isKissing = true;
		// [] stop this player from being able to move around
		sfx_kiss.Play (); // play looping kissing sound

	}

	void exitKiss() { // personal
		setMouseLooksTo (originalBounds); // unclamp mouse

		photonView.RPC("exitKissRPC", PhotonTargets.AllBuffered);
	}

	[RPC]
	void exitKissRPC() {
		print (gameObject.name + " has exited a kiss!");
		isKissing = false;
		sfx_kiss.Stop (); // stop looping kissing sound
	}
}
