using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw_Showing : MonoBehaviour
{
    public Canvas ThrowPromptCanvas;

    void OnTriggerEnter(Collider TheThingEnteringTheTrigger)
    {
        if (TheThingEnteringTheTrigger.tag == "Player")
        {
            Debug.Log("Player is by the jester");
            ThrowPromptCanvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider TheThingLeaving)
    {
        if (TheThingLeaving.tag == "Player")
        {
            Debug.Log("The Player has left the jester counter");
            ThrowPromptCanvas.enabled = false;
        }
    }


}
