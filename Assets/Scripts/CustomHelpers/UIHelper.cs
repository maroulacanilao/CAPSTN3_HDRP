using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CustomHelpers
{
    public static class UIExtensions
    {
        private static PointerEventData _eventDataCurrentPosition;

        private static List<RaycastResult> _results;
        private static Vector2 _screenSize;
        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        /// <summary>
        ///     returns world size base on the screen and camera size
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector2 GetScreenWorldSize(this Camera cam)
        {
            var aspect = (float) Screen.width / Screen.height;

            var worldHeight = cam.orthographicSize * 2;

            var worldWidth = worldHeight * aspect;

            return new Vector2(worldWidth, worldHeight);
        }

        /// <summary>
        ///     returns world size base on the screen and camera size
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Vector2 GetScreenWorldSize(this Scene scene)
        {
            var cam = scene.GetFirstMainCameraInScene();

            return GetScreenWorldSize(cam);
        }

        /// <summary>
        ///     returns a world space position (Vector 2) from a given position of a canvas and camera
        /// </summary>
        /// <param name="element"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector2 GetWorldPositionOfCanvasElement(this Camera cam, RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                element,
                element.position,
                cam,
                out var result);
            return result;
        }

        public static void SetTextDynamic(this TextMeshProUGUI text, object value)
        {
            text.SetText(value.ToString());
        }
        
        public static void ClampPositionToScreen(this RectTransform rectTransform_)
        {
            if (_screenSize == Vector2.zero) _screenSize = new Vector2(Screen.width, Screen.height);
            
            var _pos = rectTransform_.transform.position;
            var _minPos = rectTransform_.sizeDelta / 2;
            var _maxPos = _screenSize - _minPos;
            rectTransform_.transform.position = new Vector2(Mathf.Clamp(_pos.x, _minPos.x, _maxPos.x), Mathf.Clamp(_pos.y, _minPos.y, _maxPos.y));
        }
    }
}