using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstScene = "Main_Menu";

    private void Start()
    {
        // Load the first real scene
        SceneManager.LoadScene(firstScene);
    }
}

