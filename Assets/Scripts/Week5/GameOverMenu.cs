using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void BtnContinue()
    {
        SceneManager.LoadScene("Combat");
    }
}
