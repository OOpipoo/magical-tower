using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicalTower.Infrastructure
{
   public class DebugController : MonoBehaviour
    {
        [SerializeField] private float _slowScale = 0.2f;
        [SerializeField] private float _fastScale = 3f;

        private const float NormalScale = 1f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = _slowScale;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = _fastScale;
            }
            else
            {
                Time.timeScale = NormalScale;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = NormalScale;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = NormalScale;
        }
    }
}
