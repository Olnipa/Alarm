using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AlarmColorChanger : MonoBehaviour
{
    [SerializeField] private ThiefDetector _thiefDetector;
    [SerializeField] private Color _alarmColorOff;
    [SerializeField] private Color _alarmColorOn;

    private bool _alarmIsOn;

    private void Start()
    {
        _thiefDetector.ThiefSpotted += AlarmOn;
        _thiefDetector.ThiefIsGone += AlarmOff;
    }

    private void OnDisable()
    {
        _thiefDetector.ThiefSpotted -= AlarmOn;
        _thiefDetector.ThiefIsGone -= AlarmOff;
    }

    private void AlarmOn()
    {
        _alarmIsOn = true;
        StartCoroutine(AlarmBlinkingOn());
    }

    private IEnumerator AlarmBlinkingOn()
    {
        bool alarmColorIsRed = false;
        var waitForHalfSecond = new WaitForSeconds(0.5f);

        while (_alarmIsOn)
        {
            if (alarmColorIsRed)
            {
                alarmColorIsRed = false;
                gameObject.GetComponent<Renderer>().material.color = _alarmColorOff;
            }
            else
            {
                alarmColorIsRed = true;
                gameObject.GetComponent<Renderer>().material.color = _alarmColorOn;
            }

            yield return waitForHalfSecond;
        }
    }

    private void AlarmOff()
    {
        _alarmIsOn = false;
        gameObject.GetComponent<Renderer>().material.color = _alarmColorOff;
    }
}
