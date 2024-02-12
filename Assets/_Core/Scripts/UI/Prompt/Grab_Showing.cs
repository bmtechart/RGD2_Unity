using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grab_Showing : MonoBehaviour
{
    public Canvas GrabPromptCanvas;

    void OnTriggerEnter(Collider TheThingEnteringTheTrigger)
    {
        if(TheThingEnteringTheTrigger.tag == "Player")
        {
            Debug.Log("Player is by the food");
            GrabPromptCanvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider TheThingLeaving)
    {
        if(TheThingLeaving.tag == "Player")
        {
            Debug.Log("The Player has left the food");
            GrabPromptCanvas.enabled = false;   
        }
    }


}
