using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Show_GrabPrompt : MonoBehaviour
{
    //The Canvas says "Grab - Left Mouse Button"
    public Canvas GrabPromptCanvas;
    void OnTriggerEnter(Collider TheThingEnteringTheTrigger)
    {
        if(TheThingEnteringTheTrigger.tag == "Player")
        {
            Debug.Log("Player is by the food");
            //Show the Grab Control canvas
            GrabPromptCanvas.enabled = true;

        }
    }
    void OnTriggerExit(Collider TheThingLeaving)
    {
        if(TheThingLeaving.tag == "Player")
        {
            Debug.Log("The player has left the table");
            //Hide the Grab Control Canvas
            GrabPromptCanvas.enabled = false; 
        }
    }
}
