/*
 * This guy lives on an empty "Opening Sequence" GameObject. 
 * 
 * When the game begins, he creates black walls around the camera and an opening text telling the user that she should 
 * press "R" or an Xbox key once they're in a comfortable head position.
 * Once this is done, the logo moves to the canvas, the black walls move out, and the description fades out with the user's head in the right spot
 * 
 */

using UnityEngine;
using System.Collections;

public class OpeningSequence : MonoBehaviour
{
	public AvatarSwitchRayHandler avatarSwitchRayHandler;
	public Transform[] ThingsToBeMadeInvisibleInOpener;
	private bool hasOpenerBeenRun = false;
	private bool allDone = false;
	private Animation anim;
	// Use this for initialization
	void Start ()
	{
		//Enable all the walls / description
		foreach (Transform child in transform) {

			child.gameObject.SetActive (true);

		}
		//disable everything we want hidden
		foreach (Transform obj in ThingsToBeMadeInvisibleInOpener) {
			
			obj.gameObject.SetActive (false);
			
		}
		//The animation object
		anim = GetComponent<Animation> ();
	}

	//This runa once Animation has finished
	private void CleanUp ()
	{

		//disable walls
		foreach (Transform child in transform) {
			
			child.gameObject.SetActive (false);
			
		}
		//re-enable the disbaled objects
		foreach (Transform obj in ThingsToBeMadeInvisibleInOpener) {
			
			obj.gameObject.SetActive (true);
			
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (!hasOpenerBeenRun) {

			if (Input.GetKeyDown ("r")) {
				print ("Head Synced");
				//run animation
				anim.Play ("Opener");
				//Make the avatar switch reflect the fact that the avatar now exists.
				avatarSwitchRayHandler.Reset ();
				hasOpenerBeenRun = true;
			}
		
		} else if (!allDone && !anim.isPlaying) {
			allDone = true;
			CleanUp ();

		}
	}
}
