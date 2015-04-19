using UnityEngine;
using System.Collections;

public class WindNoise : MonoBehaviour
{
	private OVRCameraRig ovrRig;
	public AudioSource audioSource;
	private float initialVolume;
	public float standardDeviation = 20f;
	private float normalizationFactor;
	private float minAngle;
	// Use this for initialization
	void Start ()
	{
		ovrRig = FindObjectOfType<OVRCameraRig> ();
		//audioSource = this.GetComponentInChildren<AudioSource> ();
		initialVolume = audioSource.volume;
		//print ("vol = " + initialVolume);
		//To find the normalization factor, we take the Z distance between the fan and person and then divide it by the distance magnitude. We then take the sine to get the minimum angle
		float zDist = Mathf.Abs (transform.position.z - ovrRig.transform.position.z);

		Vector3 dir = ovrRig.transform.position - transform.position;
		float minDist = dir.magnitude;
		//print ("minDist = " + minDist);
		minAngle = Mathf.Asin (zDist / minDist); //Radians
		minAngle = Mathf.Rad2Deg * (minAngle);
		//print ("minAngle = " + minAngle);
		normalizationFactor = Mathf.Exp ((Mathf.Pow (minAngle, 2f) / (2 * Mathf.Pow (standardDeviation, 2))));
		//print ("normalizatiuon = " + normalizationFactor);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 dir = ovrRig.transform.position - transform.position;
		dir = ovrRig.transform.InverseTransformDirection (dir); //dunno if needed
		float angle = Vector3.Angle (dir, transform.forward);
		if (angle < minAngle) {
			//print ("MinAngle was wrong, off by = " + (minAngle-angle));
			angle = minAngle;
			//HACK
			//This is because sometimes the angle used for normalization is ~5 degrees off. As a result, the sound gets too high at times. Should fix
		}
			
		//print ("angle = " + angle);
		float factor = normalizationFactor * Mathf.Exp (-(Mathf.Pow (angle, 2f) / (2 * Mathf.Pow (standardDeviation, 2))));
		//print ("volume down by" + factor);
		audioSource.volume = initialVolume * factor;
	}
}
/* This file is part of VRtube, Playa.

    VRtube, Playa is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VRtube, Playa is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VRtube, Playa.  If not, see <http://www.gnu.org/licenses/>.
*/