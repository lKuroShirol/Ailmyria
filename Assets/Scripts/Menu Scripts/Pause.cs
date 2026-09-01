using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject panelPausa;
    [SerializeField] GameObject panelControls;
    private void Awake()
    {
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
        panelControls.SetActive(false);
    }

    // Update is called once per frame
    public void StartGame()
    {
        SceneManager.LoadScene("Bosque");
    }

    public void OnPause()
    {      
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }
    
    public void OnResume()
    {
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
    }

    public void Menu()
    {  
        Time.timeScale = 1f;
        SceneManager.LoadScene("INTRO");
    }

    public void Creditos()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Creditos");

    }

    public void Controls()
    {
        panelControls.SetActive(true);
    }

    public void Back()
    {
        panelControls.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
