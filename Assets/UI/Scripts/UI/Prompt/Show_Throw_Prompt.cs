using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_Throw_Prompt : MonoBehaviour
{
    //The Canvas says "Grab - Left Mouse Button"
    public Canvas ThrowPromptCanvas;
    void OnTriggerEnter(Collider TheThingEnteringTheTrigger)
    {
        if (TheThingEnteringTheTrigger.tag == "Player")
        {
            Debug.Log("Player is by the food");
            //Show the Grab Control canvas
            ThrowPromptCanvas.enabled = true;

        }
    }
    void OnTriggerExit(Collider TheThingLeaving)
    {
        if (TheThingLeaving.tag == "Player")
        {
            Debug.Log("The player has left the table");
            //Hide the Grab Control Canvas
            ThrowPromptCanvas.enabled = false;
        }
    }
}
