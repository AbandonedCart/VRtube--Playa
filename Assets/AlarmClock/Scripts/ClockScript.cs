using UnityEngine;
using System.Collections;
using System;

public class ClockScript : MonoBehaviour 
{
	// time changed (called each second)
	public delegate void TimeChanged(int hours, int mins, int secs);
	
	// use this events
	public event TimeChanged OnTimeChanged;
	
	[Range(0, 100)]
	// time scale
	public float timeScale = 1f;
	// use real or custom time
	public bool useRealTime = true;
	// custom time
	[Range(0, 11)]
	public int customHours = 9;
	[Range(0, 59)]
	public int customMinutes = 30;
	
	// arrows refs
	private Transform arrowHour;
	private Transform arrowMin;
	private Transform arrowSec;
	
	// clock time
	private float _time;
	private int secs;
	private int mins;
	private int hours;
	
	//Tick sound
	private AudioSource tickSound;
	void Awake() 
	{
		arrowHour  = transform.FindChild("arrow_hour");
		arrowMin   = transform.FindChild("arrow_min");
		arrowSec   = transform.FindChild("arrow_sec");

		tickSound = GetComponentInParent<AudioSource> ();
		if (arrowHour == null)
			Debug.LogError("Missing 'arrow_hour' object");
		if (arrowMin == null)
			Debug.LogError("Missing 'arrow_min' object");
		if (arrowSec == null)
			Debug.LogError("Missing 'arrow_sec' object");

		_time = 0f;
		
		if (useRealTime)
		{
			DateTime now = DateTime.Now;
			hours = now.Hour;
			mins  = now.Minute;
			secs  = now.Second;
		}
		else
		{
			hours = customHours;
			mins  = customMinutes;
			secs  = 0;
		}
		
		// init arrows rotation
		InitArrows();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_time += Time.deltaTime * timeScale;
		while (_time > 1f)
		{
			_time -= 1f;
			secs++;
			// rotate seconds arrow
			arrowSec.transform.Rotate(0, -360f/60f, 0, Space.Self);
			//Make a tick noise. Added by James
			tickSound.Play();
			if (secs == 60)
			{
				secs = 0;
				mins++;
				// rotate minutes and hours arrow
				arrowMin.transform.Rotate(0, -360f/60f, 0, Space.Self);
				arrowHour.transform.Rotate(0, -360f/60f/12f, 0, Space.Self);
			
				if (mins == 60)
				{
					mins = 0;
					hours++;
					if (hours == 24)
					{
						hours = 0;
					}
				}
				
			}
			if (OnTimeChanged!=null)
				OnTimeChanged(hours, mins, secs);
			
		}
	}
	
	void InitArrows()
	{
		// rotate arrows
		arrowSec.transform.Rotate(0, -360f/60f*secs, 0, Space.Self);
		arrowMin.transform.Rotate(0, -360f/60f*mins, 0, Space.Self);
		arrowHour.transform.Rotate(0, -360f/60f/12f*mins - 360f/12f*hours, 0, Space.Self);
	}
	
}
