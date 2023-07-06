using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.SceneLoader.SceneTransition
{
    public class StripsTransition : FadeTransition_Base
    {
        [SerializeField] List<Image> strips = new List<Image>();

        protected override Task OnTransition()
        {
            var _startFill = isStartScene ? 1f : 0;
            var _endFill = isStartScene ? 0 : 1f;


            var _tween = DOTween.To(() => _startFill, x => _startFill = x, _endFill, duration).OnUpdate(() =>
            {
                foreach (var image in strips)
                {
                    image.fillAmount = _startFill;
                }
            });

            _tween.SetUpdate(true);

            return _tween.AsyncWaitForCompletion();
        }
    }
}
