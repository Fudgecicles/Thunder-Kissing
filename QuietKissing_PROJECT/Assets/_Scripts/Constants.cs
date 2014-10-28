using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
	private static Constants m_myPrivateSelf;
	public static Constants s {
		get {
			if (m_myPrivateSelf == null) m_myPrivateSelf = GameObject.FindObjectOfType<Constants>();
			return m_myPrivateSelf;
		}
	}

	public float rateOfKiss; // measured in kisses/sec
}
