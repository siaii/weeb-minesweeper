using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private int height = 20;
    private int width = 20;
    private int mines = 8;


    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void OnEndEditWidth(string widthInput)
    {
        int.TryParse(widthInput, out width);
    }

    public void OnEndEditHeight(string heightInput)
    {
        int.TryParse(heightInput, out height);
    }

    public void OnEndEditMines(string minesInput)
    {
        int.TryParse(minesInput, out mines);
    }

    public void EasyButton()
    {
        height = 10;
        width = 10;
        mines = 5;
        LoadMainGame("MainGame");
    }

    public void MediumButton()
    {
        height = 20;
        width = 20;
        mines = 30;
        LoadMainGame("MainGame");
    }

    public void HardButton()
    {
        height = 30;
        width = 30;
        mines = 60;
        LoadMainGame("MainGame");
    }

    public void LoadMainGame(string name)
    {
        FieldController.FC.SetHeight(height);
        FieldController.FC.SetWidth(width);
        FieldController.FC.SetMines(mines);

        SceneManager.LoadSceneAsync(name,LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync("MainMenu");
    }

}
