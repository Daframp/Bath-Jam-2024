using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

    private TextMeshProUGUI[] Texts;
    private string[] WeaponAttachs = new string[] { "Bayonet", "Shotgun", "Piercing", "Sniper"};
    private string[] Others = new string[] {"Exploding Barrels", "Random Walls", "Spiked Armour", "Increased Board", "Hat", "Veto Colour" };


    // Start is called before the first frame update
    void Start()
    {
        CurrentColors = new Color[] { Color.white, Color.black };
        TempColors = new List<Color>();
        Counter = 0;
        GameState = 0;
        //NextStage(); // remove this when we have the main game loop and put it in the main loop
        interval = 5f;
        resetColorList();
        tilemaps = GetComponentsInChildren<Tilemap>();
        Texts = GetComponentsInChildren<TextMeshProUGUI>();
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
        SetTileMap();
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
        SetTileMap();
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
        NextRound();
    }

    public void NextRound()
    {
        resetColorList();
        TempColors = new List<Color>();
        
        if (GameState == 1)
        {
            CurrentColors[0] = GetColor();
            CurrentColors[1] = Color.white;
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

    public void Shop()
    {
        int xm = tilemaps[0].cellBounds.xMin;
        int xM = tilemaps[0].cellBounds.xMax;
        SetTileColour(Color.blue, new Vector3Int(xm + ((xM - xm) / 2), 2), tilemaps[0]);
        SetTileColour(Color.blue, new Vector3Int(xm + ((xM - xm) / 2), 0), tilemaps[0]);
        SetTileColour(Color.blue, new Vector3Int(xm + ((xM - xm) / 2), -2), tilemaps[0]);
        int rnd = Random.Range(0, WeaponAttachs.Length);
        Texts[1].text = WeaponAttachs[rnd];
        rnd = Random.Range(0, Others.Length);
        Texts[2].text = Others[rnd];
    }

    public void DecreaseSize()
    {
        foreach (var tilemap in tilemaps)
        {
            RemoveTiles(tilemap);
        }
    }
    private void RemoveTiles(Tilemap t)
    {
        int xm = t.cellBounds.xMin;
        int xM = t.cellBounds.xMax;
        int ym = t.cellBounds.yMin;
        int yM = t.cellBounds.yMax;
        for (int i = xm; i < xM ; i++)
        {
            t.SetTile(new Vector3Int(i, ym), null);
            t.SetTile(new Vector3Int(i, yM-1), null);
        }

        for (int i = ym; i < yM; i++)
        {
            t.SetTile(new Vector3Int(xm, i), null);
            t.SetTile(new Vector3Int(xM-1, i), null);
        }
    }
    public void IncreaseSize()
    {
        for(int i = 0; i< tilemaps.Length;i++)
        {
            AddTiles(tilemaps[i],i);
        }
    }
    private void AddTiles(Tilemap t,int increment)
    {
        int xm = t.cellBounds.xMin;
        int xM = t.cellBounds.xMax;
        int ym = t.cellBounds.yMin;
        int yM = t.cellBounds.yMax;
        for (int i = xm+increment; i < xM; i++)
        {
            t.SetTile(new Vector3Int(i, ym+1), new Tile());
            t.SetTile(new Vector3Int(i, yM), new Tile());
            i++;
        }

        for (int i = ym+increment; i < yM; i++)
        {
            t.SetTile(new Vector3Int(xm+1, i), new Tile());
            t.SetTile(new Vector3Int(xM, i), new Tile());
            i++;
        }
    }
}
