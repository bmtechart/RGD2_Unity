using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    public class UIManager : Singleton<UIManager>
    {
        //Properties
        public Dictionary<string, GameObject> Widgets;

        //return current widget count. 
        public int WidgetCount
        {
            get { return Widgets.Count; }
        }

        public void RegisterWidget(string name, GameObject widget)
        {
            //don't register multiple widgets with the same name
            if (Widgets.TryGetValue(name, out widget))
            {
                Debug.Log("Widget already registered with that name!");
                return;
            }

            GameObject newWidget = Instantiate(widget);
            newWidget.SetActive(false);
            Widgets.Add(name, widget);
        }

        /// <summary>
        /// Destroys a specific widget.
        /// </summary>
        /// <param name="widget">Game object to destroy.</param>
        public void DeregisterWidget(string name)
        {
            Destroy(Widgets[name]);
            Widgets.Remove(name);
        }

        protected override void Start()
        {
            base.Start();

            //initialize active widgets
            Widgets = new Dictionary<string, GameObject>();
        }

        

        /// <summary>
        /// Adds a registered widget to the screen. 
        /// </summary>
        /// <param name="name">The name of the registered widget you'd like to add to the viewport.</param>
        public GameObject AddWidgetToViewport(string name)
        {
            GameObject widget;
            if(Widgets.TryGetValue(name, out widget))
            {
                widget.SetActive(true);
                return widget;
            }

            Debug.Log("No widget with name " + name + " registered to the UI manager!");
            return null;

        }

        /// <summary>
        /// Disables visibility of specific widget by deactivating game object. 
        /// </summary>
        /// <param name="name">The name of the registered widget to remove from the viewport</param>
        public void RemoveWidgetFromViewport(string name)
        {
            GameObject widget;
            if (Widgets.TryGetValue(name, out widget))
            {
                widget.SetActive(false);
                return;
            }

            Debug.Log("No widget with name " + name + " registered to the UI Manager!");
        }




        /// <summary>
        /// Deactivates all widgets.
        /// </summary>
        public void HideAllWidgets()
        {
            foreach(string name in Widgets.Keys)
            {
                Widgets[name].SetActive(false);
            }
        }

        /// <summary>
        /// Destroys all widgets, clearing them from memory. 
        /// </summary>
        public void ClearAllWidgets()
        {
            foreach(string name in Widgets.Keys)
            {
                Destroy(Widgets[name]);
                Widgets.Remove(name);
            }
        }
    }
}
