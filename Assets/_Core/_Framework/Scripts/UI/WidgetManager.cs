using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    public class WidgetManager : Singleton<WidgetManager>
    {
        //Properties
        public Dictionary<string, GameObject> Widgets = new Dictionary<string, GameObject>();


        protected override void Start()
        {
            base.Start();
            //Widgets = new Dictionary<string, GameObject>();
        }


        public void RegisterWidget(string name, GameObject widget)
        {
            GameObject existingWidget;
            //don't register multiple widgets with the same name
            if (Widgets.TryGetValue(name, out existingWidget))
            {
                Debug.Log("Widget already registered with that name!");
                return;
            }

            Widgets.Add(name, widget);
        }

        /// <summary>
        /// Destroys a specific widget.
        /// </summary>
        /// <param name="widget">Game object to destroy.</param>
        public void DeregisterWidget(string name)
        {
            GameObject widget;
            if (Widgets.TryGetValue(name, out widget))
            {
                Destroy(Widgets[name]);
                Widgets.Remove(name);
                return;
            }

            Debug.Log("No widget with name " + name + " registered to the UI manager!");
        }

        /// <summary>
        /// Adds a registered widget to the screen. 
        /// </summary>
        /// <param name="name">The name of the registered widget you'd like to add to the viewport.</param>
        public GameObject OpenWidget(string name)
        {
            Debug.Log(Widgets.ToString());
            GameObject widget;
            if(Widgets.TryGetValue(name, out widget))
            {
                //widget.SetActive(true);
                IWidget ui = widget.GetComponent<IWidget>();
                if(ui != null) ui.Open();
                return widget;
            }

            Debug.Log("No widget with name " + name + " registered to the UI manager!");
            return null;

        }

        /// <summary>
        /// Disables visibility of specific widget by deactivating game object. 
        /// </summary>
        /// <param name="name">The name of the registered widget to remove from the viewport</param>
        public void CloseWidget(string name)
        {
            GameObject widget;
            if (Widgets.TryGetValue(name, out widget))
            {
                IWidget ui = widget.GetComponent<IWidget>();
                if(ui != null) ui.Close();
                //widget.SetActive(false);
                return;
            }

            Debug.Log("No widget with name " + name + " registered to the UI Manager!");
        }

        /// <summary>
        /// Deactivates all widgets.
        /// </summary>
        public void CloseAllWidgets()
        {
            foreach(string name in Widgets.Keys)
            {
                IWidget ui = Widgets[name].GetComponent<IWidget>();
                if (ui != null) ui.Close();
                //Widgets[name].SetActive(false);
            }
        }

        /// <summary>
        /// Destroys all widgets, clearing them from memory. 
        /// </summary>
        public void ClearAllWidgets()
        {
            foreach(string name in Widgets.Keys)
            {
                DeregisterWidget(name);
            }
        }
    }
}
