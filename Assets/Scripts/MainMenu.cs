using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGameNow()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySfx("Click");
        }
        else
        {
            Debug.LogError("AudioManager.Instance is null!");
        }

        SceneManager.LoadScene("Game");
    }

}
