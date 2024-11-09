using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class BoardClass : MonoBehaviour
{
    List<Transform> list = new List<Transform>();
    Tilemap tilemap;
    public List<Vector3> availablePlaces;
    public List<Vector3Int> availablePlaces2;

    // Start is called before the first frame update
    void Start()
    {
        /*tilemap = GetComponent<Tilemap>();
        GetPlaces();
        foreach (Vector3Int t in availablePlaces2)
        {
            SetTileColour(Color.green,t, tilemap);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Colour".
        tilemap.SetTileFlags(position, TileFlags.None);

        // Set the colour.
        tilemap.SetColor(position, colour);
    }

    private void GetPlaces()
    {
        tilemap = transform.GetComponentInParent<Tilemap>();
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
}
