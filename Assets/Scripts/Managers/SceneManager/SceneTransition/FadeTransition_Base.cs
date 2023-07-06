using System.Threading.Tasks;
using UnityEngine;

namespace Managers.SceneLoader.SceneTransition
{
    public abstract class FadeTransition_Base : MonoBehaviour
    {
        [SerializeField] protected RectTransform imgRect;
    
        [SerializeField] protected float duration = 1;

        [SerializeField] protected Canvas _canvas;
        protected bool isStartScene;
        protected Vector2 screenSize;
        protected Vector2 middleScreen;

        public void Initialize()
        {
            Setup();
        }
    
        protected virtual void Setup() {}

        public Task StartTransition(bool _isStartScene)
        {
            isStartScene = _isStartScene;
            screenSize = _canvas.renderingDisplaySize;
        
            middleScreen = new Vector2(screenSize.x / 2f, screenSize.y / 2f);
            return OnTransition();
        }

        protected virtual Task OnTransition()
        {
            return null;
        }
    }
}
