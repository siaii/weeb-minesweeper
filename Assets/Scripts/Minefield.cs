using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Minefield : MonoBehaviour
{
    [SerializeField] private GameObject cover;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject tagLayer;
    [SerializeField] private Sprite[] mines;
    [SerializeField] private Sprite flag;

    private int[] coords=new int[2];
    private bool tagged=false;
    private bool bomb = false;
    public int bombInProximity = 0;
    public bool correctTag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        tagLayer.GetComponent<SpriteRenderer>().sprite = flag;
        SetFlag(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tagged && bomb)
        {
            correctTag = true;
        }
        else
        {
            correctTag = false;
        }
    }

    public void OpenCover()
    {
        if (cover.activeSelf)
        {
            cover.SetActive(false);
            if (CheckBomb()) {
                FieldController.FC.SetGameOver();
            }
        }

    }

    private void OnMouseOver()
    {
        if (Input.GetButtonUp("Open"))
        {
            FieldGenerator.FG.BFSOpen(this);
        }
        if (Input.GetButtonUp("Tag"))
        {
            if (cover.activeSelf)
            {
                if (tagged)
                {
                    Debug.Log("Untagged");
                    tagged = false;
                    SetFlag(false);
                    return;
                }
                if (!tagged)
                {
                    Debug.Log("Tagged");
                    tagged = true;
                    SetFlag(true);
                    return;
                }
            }
        }
    }

    public bool CheckBomb()
    {
        return bomb;
    }

    private void SetImage(int id)
    {
        content.GetComponent<SpriteRenderer>().sprite = mines[id];
    }

    private void SetFlag(bool active)
    {
        tagLayer.SetActive(active);
    }

    public void SetBomb()
    {
        bomb = true;
    }

    public void IncrementBomb()
    {
        bombInProximity++;
    }

    public void SetContent()
    {
        if (CheckBomb())
        {
            SetImage(9);
        }
        else
        {
            SetImage(bombInProximity);
        }
    }

    public void SetCoords(int x, int y)
    {
        coords[0] = x;
        coords[1] = y;
    }

    public bool isOpened()
    {
        return !cover.activeSelf;
    }

    public int[] Coords => coords;
}


