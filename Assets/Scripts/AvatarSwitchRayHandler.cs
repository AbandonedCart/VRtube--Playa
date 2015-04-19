/* This lives on the AvatarSwitch Button.
 * It receives a RayHit() message from Ray Firer and turns off/on the avatar
 * 
 * 
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarSwitchRayHandler : MonoBehaviour
{
	
	public GameObject avatar;
	private Text textComp;
	public Font myFont;
	public string switchOffMessage = "Turn Avatar Off";
	public string switchOnMessage = "Turn Avatar On";

	void Start ()
	{
		textComp = GetComponentInChildren<Text> ();
		textComp.font = myFont;
		//If the avatar is currently enabled
		if (avatar.activeSelf) {
			textComp.text = switchOnMessage;
		} else {
			textComp.text = switchOffMessage;
		}

	}
	public void Reset ()
	{
		print ("reset");
		//If the avatar is currently enabled
		if (avatar.activeSelf) {
			textComp.text = switchOnMessage;
		} else {
			textComp.text = switchOffMessage;
		}

	}
	public void OnClick ()
	{
		//If the avatar is currently enabled
		if (avatar.activeSelf) {
			textComp.text = switchOnMessage;
			avatar.SetActive (false);
			print ("User turned off avatar");
		} else {
			textComp.text = switchOffMessage;
			print ("User turned on avatar");
			avatar.SetActive (true);
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
