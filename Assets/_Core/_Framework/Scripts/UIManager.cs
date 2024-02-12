using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    public class UIManager : Singleton<UIManager>
    {
        //Properties
        public List<GameObject> activeWidgets;

        //return current widget count. 
        public int WidgetCount
        {
            get { return activeWidgets.Count; }
        }

        protected override void Start()
        {
            base.Start();

            //initialize active widgets
            activeWidgets = new List<GameObject>();
        }

        /// <summary>
        /// If a widget has already been instantiated, this function will set it to active.
        /// If a widget has not been instanctiated, 
        /// this function will instantiate it and add it to the list of active widgets.
        /// Returns the widget that has been added to the viewport. 
        /// </summary>
        /// <param name="widget"></param>
        public GameObject AddWidgetToViewport(GameObject widget)
        {
            if (activeWidgets.Contains(widget))
            {
                widget.SetActive(true);
                return widget;
            }

            GameObject newWidget = Instantiate(widget);
            activeWidgets.Add(newWidget);

            return widget;
        }

        /// <summary>
        /// Disables visibility of specific widget by deactivating game object. 
        /// </summary>
        /// <param name="widget">Game object to deactivate.</param>
        public void RemoveWidgetFromViewport(GameObject widget)
        {
            if (activeWidgets.Contains(widget))
            {
                widget.SetActive(false);
                return;
            }

            activeWidgets.Add(widget);
            widget.SetActive(false);
        }

        /// <summary>
        /// Destroys a specific widget.
        /// </summary>
        /// <param name="widget">Game object to destroy.</param>
        public void DestroyWidget(GameObject widget)
        {
            activeWidgets.Remove(widget);
            Destroy(widget);
        }


        /// <summary>
        /// Deactivates all widgets.
        /// </summary>
        public void HideAllWidgets()
        {
            foreach (GameObject widget in activeWidgets)
            {
                widget.SetActive(false);
            }
        }

        /// <summary>
        /// Destroys all widgets, clearing them from memory. 
        /// </summary>
        public void ClearAllWidgets()
        {
            foreach (GameObject widget in activeWidgets)
            {
                Destroy(widget);
            }
        }
    }
}
