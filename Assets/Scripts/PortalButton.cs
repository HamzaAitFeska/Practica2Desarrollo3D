using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent m_Event;
    void OnTriggerEnter(Collider _Collider)
    {
        if (_Collider.tag == "Player")
        {
           m_Event.Invoke();
        }

    }
}
