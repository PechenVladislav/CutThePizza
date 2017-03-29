using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public void ReladLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
