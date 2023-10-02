using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < 9; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 0);
        }
        SceneManager.LoadScene(0);
    }
}
