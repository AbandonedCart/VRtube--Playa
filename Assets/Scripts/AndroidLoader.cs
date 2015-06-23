using UnityEngine;
using System.Collections;
using QuaternionSoft.Q3D.UnityPlayer;
using System.IO;

public class AndroidLoader : MonoBehaviour {

	private Q3DRenderer rendererQ3D;
	[SerializeField] private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rendererQ3D = GetComponentInParent<Q3DRenderer> ();
		string[] q3dFiles = Directory.GetFiles(Application.persistentDataPath + "/", "*.q3d");
		if (q3dFiles != null && q3dFiles.Length > 0) {
			if (File.Exists (q3dFiles [0].Replace (".q3d", ".wav"))) {
				StartCoroutine (loadFile (q3dFiles [0]));
			} else {
				rendererQ3D.Filename = q3dFiles [0];
				rendererQ3D.LoadFile (q3dFiles [0]);
				rendererQ3D.AutoPlay = true;
			}
		} else {
			// TODO: Warn user we have nothing to play
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
}
