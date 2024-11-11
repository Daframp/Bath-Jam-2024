using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BoardControl : MonoBehaviour
{
    private List<Color> colors = new List<Color> { Color.blue,Color.green,Color.red,Color.yellow};
    private List<Color> colorsList = new List<Color>();

    [SerializeField] float interval;
    private float time;

    private Tilemap[] tilemaps;
    private TileBase Set1;
    private TileBase Set2;
    private List<Vector3> availablePlaces;
    private List<Vector3Int> availablePlaces2;

    private int GameState;
    private int Counter;

    private Color[] CurrentColors = new Color[] { Color.white, Color.black };
    private List<Color> TempColors = new List<Color>();

    private string[] WeaponAttachs = new string[] { "Sword", "Shotgun", "Piercing", "Sniper"};
    private string[] Others = new string[] {"Explosives", "Wall", "BoardExpansion", "Tophat", "VetoBlue", "VetoGreen", "VetoYellow", "VetoRed" };


    // Start is called before the first frame update
    void Start()
    {
        CurrentColors = new Color[] { Color.white, Color.black };
        TempColors = new List<Color>();
        Counter = 0;    
        GameState = 0;
        NextStage();
        NextRound();
        interval = 3f;
        resetColorList();
        tilemaps = GetComponentsInChildren<Tilemap>();
        Set1 = tilemaps[0].GetTile(new Vector3Int(0,0,0));
        Set2 = tilemaps[1].GetTile(new Vector3Int(1, 0, 0));
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
        CreateSprite("Shop1","Heart", new Vector3((float) (xm + ((xM - xm) / 2) + 2.25), (float)2.75));
        int rnd = Random.Range(0, WeaponAttachs.Length);
        CreateSprite("Shop2", WeaponAttachs[rnd], new Vector3((float)(xm + ((xM - xm) / 2) + 2.25), (float)0.50));
        rnd = Random.Range(0, Others.Length);
        CreateSprite("Shop3", Others[rnd], new Vector3((float)(xm + ((xM - xm) / 2) + 2.25), (float)-1.75));
    }

    public void CloseShop()
    {
        GameObject[] temp = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].name == "Shop1")
            {
                GameObject.Destroy(temp[i]);
            }
            if (temp[i].name == "Shop2")
            {
                GameObject.Destroy(temp[i]);
            }
            if (temp[i].name == "Shop3")
            {
                GameObject.Destroy(temp[i]);
            }
        }
    }

    private void CreateSprite(string name, string item, Vector3 pos)
    {
        Sprite s = Resources.Load<Sprite>("SpritesPNG/"+item);
        var new_sprite = new GameObject();
        new_sprite.name = name;
        new_sprite.tag = "Shop";
        new_sprite.AddComponent<BoxCollider2D>();
        new_sprite.GetComponent<BoxCollider2D>().isTrigger = true;
        new_sprite.GetComponent <BoxCollider2D>().size = new Vector2((float)0.16, (float)0.16);
        new_sprite.AddComponent<SpriteRenderer>();
        var ui_renderer = new_sprite.GetComponent<SpriteRenderer>();
        ui_renderer.sprite = s; // Change to load the sprite file

        new_sprite.transform.localPosition = pos;
        new_sprite.transform.localScale = new Vector3(5,5,0);
    }

    public void DecreaseSize()
    {
        Debug.Log(tilemaps[0].cellBounds.xMin);
        foreach (var tilemap in tilemaps)
        {
            RemoveTiles(tilemap);
            tilemap.CompressBounds();
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
        GameObject background = GameObject.Find("Background");
        background.transform.localScale -= new Vector3((float)1.1, (float)1.1, 0);
    }
    public void IncreaseSize()
    {
        Debug.Log(tilemaps[0].cellBounds.xMin);
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
        TileBase temp;
        if (increment == 0)
        {
            temp = Set1;
        }
        else
        {
            temp = Set2;
        }
        for (int i = xm+increment-1; i < xM+1; i++)
        {

            t.SetTile(new Vector3Int(i, ym - 1), temp);
            if (!(i + 1 > xM))
            {
                t.SetTile(new Vector3Int(i + 1, yM), temp);
            }
            i++;
        }

        for (int i = ym+increment-1; i < yM+1; i++)
        {
            t.SetTile(new Vector3Int(xm-1, i), temp);
            if (!(i+1 > yM))
            {
                t.SetTile(new Vector3Int(xM, i + 1), temp);
            }
            i++;
        }
        GameObject background = GameObject.Find("Background");
        background.transform.localScale += new Vector3((float)1.1, (float)1.1, 0);

    }


    public void VetoColor(Color c)
    {
        colors.Remove(c);
        colors.Add(Color.white);
    }

    public void AddColor(Color c)
    {
        colors.Add(c);
        colors.Remove(Color.white);
    }

    public int[] GetSize()
    {
        var t = tilemaps[0];
        return new int[] { t.cellBounds.xMin, t.cellBounds.xMax, t.cellBounds.yMin, t.cellBounds.yMax };
    }
}
