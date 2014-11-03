using UnityEngine;
using System.Collections;

public class KissBox : MonoBehaviour {
	public Lover myLover; // assign in inspector

	/* we only want this script to run on the owner's simulation*/
	/* i felt so fancy typing that*/
	void Awake() {
		if (!myLover.GetComponent<PhotonView>().isMine) {
			Destroy (this);
		}
	}

	void OnTriggerEnter (Collider other) {
		Lover otherLover = other.GetComponent<Lover> ();
		if (otherLover != null) {
			if (myLover.IsKissing && otherLover.IsKissing) {
				myLover.enterKissMutual(otherLover);
				otherLover.enterKissMutual (myLover);
			}
		}
	}

	/* this is for the case that two lovers are facing each other
	 * and their kiss boxes are touching
	 * but they haven't actually kissed yet */
	void OnTriggerStay (Collider other) {
		Lover otherLover = other.GetComponent<Lover> ();
		if (otherLover != null) {
			if (!myLover.IsKissingMutual) { // make sure that y'all aren't already macking
				if (myLover.IsKissing && otherLover.IsKissing) {
					myLover.enterKissMutual(otherLover);
					otherLover.enterKissMutual (myLover);
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		Lover otherLover = other.GetComponent<Lover> ();
		if (otherLover != null) {
			/*if (myLover.IsKissing && otherLover.IsKissing) {
				myLover.enterKissMutual(otherLover);
				otherLover.enterKissMutual (myLover);
			}*/
			if (myLover.IsKissingMutual) {
				myLover.exitKissMutual();
			}
		}
	}
}
