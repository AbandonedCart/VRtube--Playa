using UnityEngine;
using System.Collections;

public class ResetOrientation : MonoBehaviour
{
	public static OVRDisplay display { get; private set; }
	private OVRDisplay ovrDisplay; // This is the variable you'll use to reference the OVRDevice script.
	void Awake ()
	{
		//ovrdevice = GameObject.Find (OVRController).GetComponent<OVRDevice> (); // Here, we reference the script OVRDevice, attached to the gameobject OVRController, in our variable ovrdevice.
		//OVRDisplay ovrDisplay = new OVRDisplay ();
		//ovrDisplay.RecenterPose ();
		if (display == null)
			display = new OVRDisplay ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown (KeyCode.JoystickButton5)) {
			display.RecenterPose ();
		}

	}
}
