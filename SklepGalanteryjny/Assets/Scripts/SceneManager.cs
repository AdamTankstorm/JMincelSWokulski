using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        SceneManager.LoadScene("");
    }
    public void Quit()
    {
       Application.Quit();
    }
}
