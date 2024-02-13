using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWidget
{

    //open is called when the widget is opened
    public void Open() { }

    //closed is called when the widget is closed
    public void Close() { }

    //reset Widget to initial state if it has one
    public void Reset() { }

}
