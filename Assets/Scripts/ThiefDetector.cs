using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThiefDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent _thiefSpotted;
    [SerializeField] private UnityEvent _thiefIsGone;

    private bool _thiefInside;

    public event UnityAction ThiefSpotted
    {
        add => _thiefSpotted.AddListener(value);
        remove => _thiefSpotted.RemoveListener(value);
    }

    public event UnityAction ThiefIsGone
    {
        add => _thiefIsGone.AddListener(value);
        remove => _thiefIsGone.RemoveListener(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Thief component))
        {
            _thiefInside = true;
            _thiefSpotted.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_thiefInside && other.TryGetComponent(out Thief player) == true)
        {
            _thiefInside = false;
            _thiefIsGone.Invoke();
        }
    }
}
