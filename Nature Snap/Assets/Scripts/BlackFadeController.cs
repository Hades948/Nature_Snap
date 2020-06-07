using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackFadeController : MonoBehaviour {
    public Animator animator;

    private string sceneName;
    
    public void fadeToScene(string sceneName) {
        animator.SetTrigger("FadeOut");
        this.sceneName = sceneName;
    }

    public void onFadeComplete() {
        SceneManager.LoadSceneAsync(sceneName);
    }

    void Update() {
        
    }
}
