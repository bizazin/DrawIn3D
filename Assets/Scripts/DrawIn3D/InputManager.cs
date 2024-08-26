using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawIn3D
{
    public class InputManager : MonoBehaviour
    {
        public abstract class Link
        {
            public Finger Finger;

            public static T Find<T>(IEnumerable<T> links, Finger finger)
                where T : Link, new() =>
                links.FirstOrDefault(link => link.Finger == finger);

            public static T Create<T>(ref List<T> links, Finger finger)
                where T : Link, new()
            {
                var link = Find(links, finger);

                if (link == null)
                {
                    link = new T
                    {
                        Finger = finger
                    };

                    links.Add(link);
                }
                else
                    Debug.LogError("Link already exists!");

                return link;
            }
        }

        public class Finger
        {
            public int Index;
            public Vector2 ScreenPosition;
        }

        private static readonly List<RaycastResult> TempRaycastResults = new(10);
        private static PointerEventData _tempPointerEventData;
        private static EventSystem _tempEventSystem;

        public static bool PointOverGui(Vector2 screenPosition) =>
            RaycastGui(screenPosition).Count > 0;

        private static List<RaycastResult> RaycastGui(Vector2 screenPosition)
        {
            TempRaycastResults.Clear();

            var currentEventSystem = EventSystem.current;

            if (currentEventSystem != _tempEventSystem)
            {
                _tempEventSystem = currentEventSystem;

                _tempPointerEventData = new PointerEventData(_tempEventSystem);
            }

            _tempPointerEventData.position = screenPosition;

            currentEventSystem.RaycastAll(_tempPointerEventData, TempRaycastResults);

            return TempRaycastResults;
        }
    }
}