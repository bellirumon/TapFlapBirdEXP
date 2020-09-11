using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownText : MonoBehaviour
{
    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;

    void OnEnable()
    {
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        for (int i = 2; i > 0 ; i--)
        {
            yield return new WaitForSeconds(1); //2 seconds delay
        }

        OnCountdownFinished();
    }
}
