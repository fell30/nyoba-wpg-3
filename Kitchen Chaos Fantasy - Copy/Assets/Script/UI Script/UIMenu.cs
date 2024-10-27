using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{

    // [SerializeField] private GameObject OrderPanel;
    // [SerializeField] private GameObject[] TimerPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {

        // OrderPanel.SetActive(false);
    }

    // private IEnumerator OrderPanelCoroutine()
    // {
    //     TimerPanel[0].SetActive(true);
    //     yield return new WaitForSeconds(2);
    //     TimerPanel[1].SetActive(true);
    //     TimerPanel[0].SetActive(false);
    //     yield return new WaitForSeconds(2);
    //     TimerPanel[2].SetActive(true);
    //     TimerPanel[1].SetActive(false);
    //     yield return new WaitForSeconds(2);
    //     // OrderPanel.SetActive(true);
    //     TimerPanel[2].SetActive(false);
    //     yield return new WaitForSeconds(2);
    //     // OrderPanel.SetActive(false);



    // }

    public void QuitGame()
    {
        Application.Quit();
    }


}
