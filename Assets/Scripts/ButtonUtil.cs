/** I live on ButtonCube!
 * This program is in charge of placing the button text, playing the q3d videos, placing the Q3D files
 * 
 * This Button defines the default Q3D position and orientation! 
 * 
 */
//TODO
//The q3d seems to be activated multiple times. This may waste memory/ CPU
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using QuaternionSoft.Q3D.UnityPlayer;

public class ButtonUtil : MonoBehaviour
{
	
	private Text textComp;
	public Font myFont;
	public string message = "";
	public float startDelay = 0.2f;
	public float midDelay = 0.1f;
	public GameObject debugPrompt;
	public GameObject notDebugPrompt;
	public string fileLocation = "empty";
	private ButtonUtil[] siblings;
	private Q3DRenderer rendererQ3D;
	private Transform Q3DPrefab;
	private string infoLocation;
	private MoveQ3D moveQ3D;
	private GameObject Q3DInstance;
	private bool hasVideo = false;
	private string[] positionData;
	private string[] rotationData;
	private string[] scaleData;
	private bool debug;
	private bool loop;
	private bool defaultLoop = true;
	private string[] defaultPositionData = new string[3]{"0.5","2","-0.5"};
	private string[] defaultRotationData = new string[4]{"0","0","0","1"};
	private string[] defaultScaleData = new string[3]{"1","1","1"};
	
	// Use this for initialization
	
	void Start ()
	{
		textComp = GetComponentInChildren<Text> ();
		textComp.font = myFont;
		textComp.fontSize = 50;
		textComp.transform.localScale = new Vector3 (.25f, .25f, 1f);
		textComp.rectTransform.sizeDelta = new Vector2 (610f, 60f);
		textComp.transform.localPosition = new Vector3 (0f, -2.5f, 0f);
		positionData = new string[3];
		rotationData = new string[4];
		scaleData = new string[3];
		
	}
	
	public void clearInstance ()
	{
		if (Q3DInstance != null) {
			Object.DestroyImmediate (Q3DInstance);
			Q3DInstance = null;
			rendererQ3D = null;
			moveQ3D = null;
		}
	}
	public void setMessage (string msg)
	{
		message = msg;
		//I believe that this is a coroutine because coroutines are able to waitforseconds
		StartCoroutine ("Type");
		
	}
	public bool HasVideo ()
	{
		return hasVideo;
	}
	public void setFile (string file, Transform _Q3DPrefab)
	{
		hasVideo = true;
		fileLocation = file;
		Q3DPrefab = _Q3DPrefab;
		infoLocation = string.Concat (fileLocation, ".txt");
		
	}
	public void setSiblings (ButtonUtil[] _siblings)
	{
		siblings = _siblings;
	}
	public void MoveButton (float dist)//Let's just overload this method if we want to move buttons with more degrees of freedom
	{
		transform.position = transform.position + this.transform.up * dist;
	}
	private void LoadPlacementData ()
	{
		//Here we set the default Q3D position, rotation, debug, etc.
		debug = true;
		defaultPositionData.CopyTo (positionData, 0);
		defaultRotationData.CopyTo (rotationData, 0);
		defaultScaleData.CopyTo (scaleData, 0);
		loop = defaultLoop;
		
		
		//Load q3D info on position etc.
		try {
			string[] lines = System.IO.File.ReadAllLines (@infoLocation);
			
			// Display the file contents by using a foreach loop.
			foreach (string line in lines) {
				//if the line currently being processes isn't a comment
				if (!line.StartsWith ("#")) {
					string[] segments = line.Split ('=');
					if (segments [0].Equals ("DEBUG") && segments [1].Equals ("False")) {
						print ("Debugging will be turned OFF");
						debug = false;
					} else if (segments [0].Equals ("DEBUG") && segments [1].Equals ("True")) {
						print ("Debugging will be turned ON");
						debug = true;
					} else if (segments [0].Equals ("POSITION")) {
						positionData = segments [1].Split (',');
					} else if (segments [0].Equals ("ROTATION")) {
						rotationData = segments [1].Split (',');
					} else if (segments [0].Equals ("SCALE")) {
						scaleData = segments [1].Split (',');
					} else if (segments [0].Equals ("LOOP") && segments [1].Equals ("True")) {
						loop = true;
					} else if (segments [0].Equals ("LOOP") && segments [1].Equals ("False")) {
						loop = false;
					}
				}
				
			}
			//Make the changes indicated in the configuration file
			moveQ3D.MovePlayer (positionData, rotationData, scaleData, debug, loop);
			//Print this stuff out for debugging
			print ("Using config file to move");
			print ("position data is: " + positionData [0] + "x " + positionData [1] + "y " + positionData [2] + "z");
			print ("rotation data is: " + rotationData [0] + "x " + rotationData [1] + "y " + rotationData [2] + "z " + rotationData [3] + "w");
			print ("scaleData data is: " + scaleData [0] + "x " + scaleData [1] + "y " + scaleData [2] + "z");
			print ("loop is set to: " + loop);
			
			
		} catch (IOException e) {
			print ("Could not find file");
			debug = true;
			this.ResetQ3DPosition ();
			print (e);
		} catch (System.IO.IsolatedStorage.IsolatedStorageException e) {
			print ("Dunno where the file be");
			debug = true;
			this.ResetQ3DPosition ();
			print (e);
		}
		
		//Turn the debug prompt on/off
		debugPrompt.SetActive (debug);
		//Same thing with the non-debug prompt
		notDebugPrompt.SetActive (!debug);
	}
	private void ResetQ3DPosition ()
	{
		moveQ3D.MovePlayer (defaultPositionData, defaultRotationData, defaultScaleData, true, defaultLoop);
		rendererQ3D.loop = defaultLoop;
	}
	public void PlayFile ()
	{
		//Make sure every other button turns off their Q3D player
		for (int i=0; i<siblings.Length; i++) {
			siblings [i].SendMessage ("clearInstance");
		}
		//Start up Q3D prefab
		Q3DInstance = ((Transform)Instantiate (Q3DPrefab, new Vector3 (0, 0, 0), Quaternion.identity)).gameObject;
		
		rendererQ3D = Q3DInstance.GetComponent<Q3DRenderer> ();
		moveQ3D = rendererQ3D.GetComponent<MoveQ3D> ();
		//Make the Q3D instance turn on 
		rendererQ3D.gameObject.SetActive (false);
		rendererQ3D.AutoPlay = false;
		rendererQ3D.LoadFile (fileLocation + ".q3d");
		rendererQ3D.gameObject.SetActive (true);
		//rendererQ3D.AutoPlay = true;
		this.LoadPlacementData ();
		
		StartCoroutine ("PlayQ3D");
	}
	IEnumerator PlayQ3D ()
	{
		//For some reason this way of playing the Q3D file seems to get the best sync...
		yield return new WaitForSeconds (0);
		
		rendererQ3D.Play ();
		
	}
	
	public IEnumerator Type ()
	{
		yield return new WaitForSeconds (startDelay);
		for (int i = 0; i<=message.Length; i++) {
			textComp.text = message.Substring (0, i);
			yield return new WaitForSeconds (midDelay);
		}
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