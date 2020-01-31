using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject square;


    private FieldController mainMenu = FieldController.FC;

    public static FieldGenerator FG;

    private int fieldHeight;
    private int fieldWidth;
    private int fieldMines;


    public GameObject[,] field;
    private int[][] bombCoords;
    

    // Start is called before the first frame update
    void Start()
    {
        SetSelf();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Generator started");

        fieldHeight = mainMenu.getHeight();
        Debug.Log("Height: " + fieldHeight);

        fieldWidth = mainMenu.getWidth();
        Debug.Log("Width: " + fieldWidth);

        fieldMines = mainMenu.getMines();
        Debug.Log("Mines: " + fieldMines);

        GenerateField(fieldHeight, fieldWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateField(int height, int width)
    {
        field = new GameObject[height, width];

        float posX = (float) (0 - (width * 0.42 / 2));
        float posY = (float) (0 - (height * 0.42 / 2));
        float originalY = posY;

        Quaternion rot = new Quaternion();

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(posX, posY, 2f);

                field[i,j]=Instantiate(square, pos, rot) as GameObject;
                field[i,j].GetComponent<Minefield>().SetCoords(i, j);

                posY += 0.42f;
            }
            posX += 0.42f;
            posY = originalY;
        }

        GenerateMines(fieldMines, fieldHeight, fieldWidth);
        SetMineImage(fieldHeight, fieldWidth);
    }

    private void GenerateMines(int mines, int height, int width)
    {
        bombCoords = new int[mines][];

        for (int i = 0; i < fieldMines; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            field[x,y].GetComponent<Minefield>().SetBomb();
            bombCoords[i] = new int[2] { x, y };

            try
            {
                field[x + 1, y + 1].GetComponent<Minefield>().IncrementBomb();

            }
            catch (System.IndexOutOfRangeException)
            {
                
            }

            try
            {
                field[x + 1, y].GetComponent<Minefield>().IncrementBomb();

            }
            catch (System.IndexOutOfRangeException)
            {
                
            }

            try
            {
                 field[x + 1, y - 1].GetComponent<Minefield>().IncrementBomb();

            }
            catch (System.IndexOutOfRangeException)
            {
                
            }

            try
            {
                
                field[x, y + 1].GetComponent<Minefield>().IncrementBomb();
            }
            catch (System.IndexOutOfRangeException)
            {
                
            }

            try
            {
            field[x, y - 1].GetComponent<Minefield>().IncrementBomb();                

            }
            catch (System.IndexOutOfRangeException)
            {
               
            }

            try
            {
            field[x - 1, y + 1].GetComponent<Minefield>().IncrementBomb();
            }
            catch (System.IndexOutOfRangeException)
            {
                
            }

            try
            {
            field[x - 1, y].GetComponent<Minefield>().IncrementBomb();       

            }
            catch (System.IndexOutOfRangeException)
            {
               
            }

            try
            {
            field[x - 1, y - 1].GetComponent<Minefield>().IncrementBomb();               

            }
            catch (System.IndexOutOfRangeException)
            {
              
            }

        }
    }

    private void SetMineImage(int height, int width)
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                //Debug.Log(i + ", " + j);
                Minefield mine = field[i, j].GetComponent<Minefield>();
                mine.SetContent();
            }
        }
    }

    private void SetSelf()
    {
        if (FG == null)
        {
            FG = this;
        }
        else if(FG!=this)
        {
            Destroy(this.gameObject);
        }
    }

    public void RecursiveOpen(int x, int y, Minefield mine)
    {
        if (mine.isOpened())
        {
            return;
        }
        else
        {
            if (mine.bombInProximity != 0 || mine.CheckBomb())
            {
                mine.OpenCover();
                return;
            }
            else
            {
                mine.OpenCover();
                if (y + 1 < fieldHeight)
                {
                    RecursiveOpen(x, y + 1, field[x, y + 1].GetComponent<Minefield>());
                }
                if (x + 1 < fieldWidth)
                {
                    RecursiveOpen(x + 1, y, field[x + 1, y].GetComponent<Minefield>());
                }
                if (y - 1 >= 0)
                {
                    RecursiveOpen(x, y - 1, field[x, y - 1].GetComponent<Minefield>());
                }
                if (x - 1 >= 0)
                {
                    RecursiveOpen(x - 1, y, field[x - 1, y].GetComponent<Minefield>());
                }
            }
        }
    }

    public void BFSOpen(Minefield mine)
    {
        int x, y;
        Queue<Minefield> openQueue = new Queue<Minefield>();
        Minefield currMine;
        openQueue.Enqueue(mine);
        if(mine.CheckBomb())
            mine.OpenCover();
        while (openQueue.Count>0)
        {
            currMine = openQueue.Dequeue();
            if (currMine.isOpened() || currMine.CheckBomb())
            {
                continue;
            }

            x = currMine.Coords[0];
            y = currMine.Coords[1];
            if (currMine.bombInProximity != 0)
            {
                currMine.OpenCover();
                continue;
            }

            currMine.OpenCover();
            if (x<fieldWidth && y + 1 < fieldHeight) 
                openQueue.Enqueue(field[x, y+1].GetComponent<Minefield>());
            if (x + 1 < fieldWidth && y<fieldHeight)
                openQueue.Enqueue(field[x+1, y].GetComponent<Minefield>());
            if (x >= 0 && y - 1 >= 0)
                openQueue.Enqueue(field[x,y-1].GetComponent<Minefield>());
            if (x - 1 >= 0 && y >= 0)
                openQueue.Enqueue(field[x-1,y].GetComponent<Minefield>());
            

        }
        
    }

    public GameObject[,] getFieldData()
    {
        return field;
    }

    public int[][] getBombCoords()
    {
        return bombCoords;
    }
}
