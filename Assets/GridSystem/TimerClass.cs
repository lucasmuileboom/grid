using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerClass : MonoBehaviour
{
	public delegate void TimerCompletedEvent();
	public event TimerCompletedEvent TimerCompleted;

	private float seconds;
	public float GetSeconds() 
	{
		return seconds;
	}
	private float trheshold;
	public float GetThreshold() 
	{
		return trheshold;
	}

	public void StartTimer(float sec) 
	{
		trheshold = sec;
		StartCoroutine(Timer(sec));
	}

	private IEnumerator Timer(float threshold) 
	{
		seconds = 0;
		while(seconds < threshold) 
		{
			seconds += Time.deltaTime;
			yield return null;
		}

		TimerCompleted?.Invoke();
	}
}