using UnityEngine;
using UnityEngine.SceneManagement;

/**
* A MonoBehavior script that controls the black fade animation on scene load/unload.
*/
public class BlackFadeController : MonoBehaviour {
    public Animator animator;
    private string sceneName;
    
    /**
    * Start the fade process on the animator.
    */
    public void fadeToScene(string sceneName) {
        animator.SetTrigger("Fade Out");
        this.sceneName = sceneName;
    }

    /**
    * A callback function to switch to the desired scene once the animation has finished.
    * Undefined behavior if fadeToScene is called more than once before the animation has finished.
    */
    public void onFadeComplete() {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
