/* This lives on the top button prefab
 * this program is called by CursorRayInteracter whenever 
 * a ray hits a collidable object.
 * This program will make the button bigger if the user is looking at it. If the user has been looking 
 * at it long enough, the button is clicked
 */
//TODO

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonRayHandler : MonoBehaviour {
	public float secondsToClick = 2f;
	public float finalScale = 0.3f;
	private float scale;
	private static int numClicked = -1; //This is which button is the one that's clicked (-1 means no button)
	private static int count = 0;
	private int myNum; //This is a unique assigned to each button for identification
	private float secElapsed = 0;
	private bool amIOn = false;
	private ButtonUtil buttonUtil;
	private Image image;//This is to change the button color

	// Use this for initialization
	void Start () {
		myNum = count;
		count += 1;
		buttonUtil = GetComponentsInChildren<ButtonUtil> ()[0];
		image = GetComponentsInChildren<Image> ()[0];
	}
	public void RayHit()
	{
		if (buttonUtil.HasVideo()) {
			//If being stared at long enough and this button isn't already clicked, do the action
			if ((secElapsed >= secondsToClick) & (numClicked != myNum)) {
				print ("User Clicked on a button");
				numClicked = myNum;
				amIOn = true;
				secElapsed = 0;
				//play file
				buttonUtil.PlayFile ();
				//return size to normal
				transform.localScale = new Vector3 (1f, 1f, 1f);
				///Turn the button gray
				image.color = Color.gray;
			} else if (numClicked != myNum) {
				secElapsed += 2 * Time.deltaTime;		
			}
		}
	}


	void Update () {
		
		if (secElapsed > 0 & (numClicked!=myNum)) {
			//Slowly decrease how long we've been looking
			secElapsed -= Time.deltaTime;
			//Slowly change size
			scale = 1+finalScale*secElapsed/secondsToClick;
			transform.localScale = new Vector3(scale, scale, scale);
		} else {
			
		}
		if (amIOn && numClicked != myNum) {//If this method is called it means that the current button has just been changed
			amIOn = false;
			//Return the button to a white color
			image.color = Color.white;
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
