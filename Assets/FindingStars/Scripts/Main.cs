using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FindingStars
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private string playSceneName;

        private void OnEnable()
        {
            if (actionReference != null && actionReference.action != null)
            {
                actionReference.action.performed += OnTapPerformed;
                actionReference.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (actionReference != null && actionReference.action != null)
            {
                actionReference.action.performed -= OnTapPerformed;
                actionReference.action.Disable();
            }
        }

        private void OnTapPerformed(InputAction.CallbackContext _)
        {
            if (!string.IsNullOrEmpty(playSceneName))
            {
                SceneManager.LoadScene(playSceneName);
            }
        }
        
        private int SumNumber(int[] numbers)
        {
            return numbers?.Sum() ?? 0;
        }
    }
}
