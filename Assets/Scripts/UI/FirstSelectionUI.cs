using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FirstSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button firstButton;

        private void OnEnable()
        {
            firstButton.Select();
        }
    }
}
