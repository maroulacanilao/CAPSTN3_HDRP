using BaseCore;
using UnityEngine;

namespace UI
{
    public class FarmUIManager : Singleton<FarmUIManager>
    {
        [SerializeField] private FarmUI[] farmUIs;
        

        protected override void Awake()
        {
            base.Awake();
            foreach (var _ui in farmUIs)
            {
                _ui.Initialize();
            }
        }
        
        public static void CloseAllUI()
        {
            foreach (var _ui in Instance.farmUIs)
            {
                _ui.gameObject.SetActive(false);
            }
        }
    }
}
