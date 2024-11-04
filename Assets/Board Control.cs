using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardControl : MonoBehaviour
{
    private Color[] colors = new Color[] { Color.blue,Color.green,Color.magenta,Color.red};
    private List<Color> colorsList = new List<Color>();

    [SerializeField] float interval;
    private float time;

    private Tilemap[] tilemaps;
    private List<Vector3> availablePlaces;
    private List<Vector3Int> availablePlaces2;

    private int GameState;
    private int Counter;

    private Color[] CurrentColors;
    private List<Color> TempColors = new List<Color>();


    // Start is called before the first frame update
    void Start()
    {
        CurrentColors = new Color[] { Color.white, Color.black };
        TempColors = new List<Color>();
        Counter = 0;
        GameState = 1;
        interval = 5f;
        resetColorList();
        tilemaps = GetComponentsInChildren<Tilemap>();
        SetTileMap();
    }

    private void resetColorList()
    {
        foreach (var color in colors)
        {
            colorsList.Add(color);
        }
    }

    private void SetTileMap()
    {
        for (int i = 0; i < tilemaps.Length; i++)
        {
            GetPlaces(tilemaps[i]);
            foreach (Vector3Int t in availablePlaces2)
            {
                SetTileColour(CurrentColors[i], t, tilemaps[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState == 1)
        {
            Stage1();
        }
        else if (GameState == 2)
        {
            Stage2();
        }
        else if (GameState == 3)
        {
            Stage3();
        }
        else if (GameState == 4)
        {
            Stage4();
        }

        else if (GameState == 5)
        {
            Stage5();
        }

    }

    private void Stage1()
    {
        time += Time.deltaTime;
        while (time >= interval)
        {
            SetTileMap();
        }
    }
    private void Stage2()
    {
        time += Time.deltaTime;
        while (time >= interval)
        {
            CurrentColors[0] = TempColors[Counter];
            CurrentColors[1] = Color.white;
            SetTileMap() ;
            time -= interval;
            if (Counter == 0)
            {
                Counter = 1;
            }
            else
            {
                Counter = 0;
            }
        }
    }
    private void Stage3()
    {
        time += Time.deltaTime;
        while (time >= interval)
        {
            SetTileMap();
        }
    }
    private void Stage4()
    {
        time += Time.deltaTime;
        while (time >= interval)
        {
            CurrentColors[0] = TempColors[Counter];
            CurrentColors[1] = Color.white;
            SetTileMap();
            time -= interval;
            if (Counter == 2)
            {
                Counter = 0;
            }
            else
            {
                Counter++;
            }
        }
    }
    private void Stage5()
    {
        time += Time.deltaTime;
        while (time >= interval)
        {
            CurrentColors[0] = TempColors[Counter];
            CurrentColors[1] = TempColors[Counter + 1];
            SetTileMap();
            time -= interval;
            if (Counter == 2)
            {
                Counter = 0;
            }
            else
            {
                Counter = 2;
            }
        }
    }

    public void NextStage()
    {
        if (GameState < 5)
        {
            GameState++;
        }
        resetColorList();
        TempColors = new List<Color>();
        
        if (GameState == 1)
        {
            CurrentColors[0] = GetColor();
            CurrentColors[1] = Color.black;
        }
        if (GameState == 2)
        {
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
        }
        if (GameState == 3)
        {
            CurrentColors[0] = GetColor();
            CurrentColors[1] = GetColor();
        }
        if (GameState == 4)
        {
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
        }
        if (GameState == 5)
        {
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
            TempColors.Add(GetColor());
        }
    }


    private Color GetColor()
    {
        var rnd = Random.Range(0, colorsList.Count);
        var temp = colorsList[rnd];
        colorsList.RemoveAt(rnd);
        return temp;
    }

    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, colour);
    }

    private void GetPlaces( Tilemap tilemap)
    {
        availablePlaces = new List<Vector3>();
        availablePlaces2 = new List<Vector3Int>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                    availablePlaces2.Add(localPlace);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    public void Reset()
    {
        Start();
    }

    public Color[] CurrentColours()
    {
        return CurrentColors;
    }
}
