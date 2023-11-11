using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour {
    private const string _bootstrapperSceneName = "Bootstrapper";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init() {
        Debug.Log("Init bootstrapper");

        var currentScene = SceneManager.GetActiveScene();

        if (!SceneManager.GetSceneByName(_bootstrapperSceneName).isLoaded) {
            SceneManager.LoadScene(_bootstrapperSceneName);
        }

        SceneManager.LoadSceneAsync(currentScene.name, LoadSceneMode.Additive);
    }
}
