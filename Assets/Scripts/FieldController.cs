using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FieldController : MonoBehaviour
{
    private int height = 20;
    private int width = 20;
    private int mines = 8;

    private float timer = 0;


    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private Image smug;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image nice;
    [SerializeField] private TextMeshProUGUI GameWonText;
    [SerializeField] private TextMeshProUGUI timeLabel;
    //[SerializeField] private TextMeshProUGUI restartLabel;
    [SerializeField] private Button restartButton;

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip win;

    private bool GameOver = false;
    private bool GameWon = false;
    private bool counting = false;

    public static FieldController FC;

    private GameObject[,] field;

    private void Awake()
    {
        setSelf();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameOverText.enabled = false;
        smug.enabled = false;
        nice.enabled = false;
        GameWonText.enabled = false;
        timeLabel.enabled = false;
        restartButton.gameObject.SetActive(false);
        //restartLabel.enabled = false;
        StartCoroutine(getFieldDelay());
    }

    // Update is called once per frame
    void Update()
    {

        if (GameOver)
        {
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    Minefield mine = field[i, j].GetComponent<Minefield>();
                    if(mine.CheckBomb())
                        field[i, j].GetComponent<Minefield>().OpenCover();
                }
            }
            canvas.enabled = true;
            soundSource.PlayOneShot(explosion);
            GameOverText.enabled = true;
            smug.enabled = true;
            restartButton.gameObject.SetActive(true);
            GameOver = false;
            counting = false;
        }

      
        if(GameWon)
        {
            canvas.enabled = true;
            soundSource.PlayOneShot(win);
            nice.enabled = true;
            GameWonText.enabled = true;
            restartButton.gameObject.SetActive(true);
            GameWon = false;
            counting = false;
        }

        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            timeLabel.enabled = true;
            if(counting)
                timer += Time.deltaTime;
            timeLabel.text = Mathf.RoundToInt(timer).ToString();

        }
           
    }


    public int getHeight()
    {
        return height;
    }

    public int getWidth()
    {
        return width;
    }

    public int getMines()
    {
        return mines;
    }

    public void SetHeight(int inputHeight)
    {
        height = inputHeight;
    }

    public void SetWidth(int inputWidth)
    {
        width = inputWidth;
    }

    public void SetMines(int inputMines)
    {
        mines = inputMines;
    }

    private void setSelf()
    {
        if (FC == null)
        {
            FC = this;
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(canvas);
        }
        if(FC!=this)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetGameOver()
    {
        GameOver = true;
    }

    private IEnumerator getFieldDelay()
    {
        yield return new WaitUntil(()=> SceneManager.GetActiveScene().name=="MainGame");
        field = FieldGenerator.FG.getFieldData();
        counting = true;
        StartCoroutine(CheckCorrectTag(field));
    }

    private IEnumerator CheckCorrectTag(GameObject[,] field)
    {
        int correctCount = 0;
        int[][] bombCoords=FieldGenerator.FG.getBombCoords();
        while (!GameOver)
        {
            for(int i = 0; i < mines; i++)
            {
                if (field[bombCoords[i][1], bombCoords[i][0]].GetComponent<Minefield>().correctTag)
                {
                    correctCount++;
                }
            }
            if (correctCount == mines)
            {
                GameWon = true;
                yield break;
            }
            else
            {
                correctCount = 0;
            }

            yield return new WaitForSeconds(0.4f);

        }
    }

    public void RestartGame()
    {
        canvas.enabled = false;
        GameOverText.enabled = false;
        smug.enabled = false;
        nice.enabled = false;
        GameWonText.enabled = false;
        timeLabel.enabled = false;
        restartButton.enabled = false;
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync("MainGame");
        Destroy(canvas.gameObject);
        Destroy(this.gameObject);
    }
}
