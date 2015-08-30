using UnityEngine;
using System.Collections;
using QuaternionSoft.Q3D.UnityPlayer;
using System.IO;

public class AndroidLoader : VRGUI {

	private Q3DRenderer rendererQ3D;
	private string[] q3dFiles;
	[SerializeField] private AudioSource audioSource;
	private int index = 0;

	private string GetPathToExternalData() {
#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				using(AndroidJavaObject fileDir = obj_Activity.Call<AndroidJavaObject>("getExternalFilesDir", "")) {
					return fileDir.Call<string>("getAbsolutePath");
				}
			}
		}
#endif
		return Application.persistentDataPath;
	}
	
	// Use this for initialization
	void Start () {
		q3dFiles = Directory.GetFiles(GetPathToExternalData() + "/", "*.q3d");
		rendererQ3D = GetComponentInParent<Q3DRenderer> ();
		if (q3dFiles != null && q3dFiles.Length > 0) {
			if (File.Exists (q3dFiles [index].Replace (".q3d", ".wav"))) {
				StartCoroutine (loadFile (q3dFiles [index]));
			} else {
				rendererQ3D.Filename = q3dFiles [index];
				rendererQ3D.LoadFile (q3dFiles [index]);
				rendererQ3D.AutoPlay = true;
			}
		} else {
			// TODO: Warn user we have nothing to play
			rendererQ3D.StopAllCoroutines();
			Application.Quit ();
		}
	}

	IEnumerator loadFile(string path) {
		WWW www = new WWW("file://" + path.Replace(".q3d", ".wav"));
		
		AudioClip q3dAudioClip = www.audioClip;
		while (!q3dAudioClip.isReadyToPlay)
			yield return www;
		
		audioSource.clip = q3dAudioClip;

		rendererQ3D.Filename = path;
		rendererQ3D.LoadFile (path);
		audioSource.Play ();
		rendererQ3D.AutoPlay = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (rendererQ3D.IsPlaying) {
			rendererQ3D.AutoPlay = false;
		}
	}

	private int aspectRatio() {
		if (Camera.main.aspect >= 1.7)
		{
			return 2;
		}
		else if (Camera.main.aspect >= 1.5)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
	
	private float baseWidth() {
		return 	854.0f;
	}
	
	private float baseHeight() {
		if (aspectRatio() == 1) {
			return 640.0f;
		} else {
			return 480.0f;
		}
	}
	
	public override void OnVRGUI()
	{
		Vector3 scale = new Vector3 (
			Screen.width / baseWidth(),
			Screen.height / baseHeight(),
			1f
			);
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.button.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.fontSize = 12;
		GUI.skin.button.fontSize = 12;
		GUI.backgroundColor = Color.white;
		GUI.color = Color.white;
		
		if (GUI.Button (new Rect (0, 0, 90, 40), "Previous") || Input.GetKeyDown (KeyCode.LeftArrow)) {
			index -= 1;
			if (q3dFiles != null && index >= 0) {
				rendererQ3D.StopAllCoroutines();
				if (File.Exists (q3dFiles [index].Replace (".q3d", ".wav"))) {
					StartCoroutine (loadFile (q3dFiles [index]));
				} else {
					rendererQ3D.Filename = q3dFiles [index];
					rendererQ3D.LoadFile (q3dFiles [index]);
					rendererQ3D.AutoPlay = true;
				}
			} else {
				index += 1;
			}
		}

		if (GUI.Button (new Rect (744, 0, 90, 40), "Next") || Input.GetKeyDown (KeyCode.RightArrow)) {
			index += 1;
			if (q3dFiles != null && q3dFiles.Length > index) {
				rendererQ3D.StopAllCoroutines();
				if (File.Exists (q3dFiles [index].Replace (".q3d", ".wav"))) {
					StartCoroutine (loadFile (q3dFiles [index]));
				} else {
					rendererQ3D.Filename = q3dFiles [index];
					rendererQ3D.LoadFile (q3dFiles [index]);
					rendererQ3D.AutoPlay = true;
				}
			} else {
				index -= 1;
			}
		}
		
		GUI.matrix = svMat;
	}
}
