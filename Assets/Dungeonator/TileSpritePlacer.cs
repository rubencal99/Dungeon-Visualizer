using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpritePlacer : MonoBehaviour
{
    [SerializeField]
    public Tilemap Tilemap;
    [SerializeField]
    private Tilemap Collisions;

    //  [SerializeField]
    // private Tilemap Obstacles;

    // We can create an array of TileBase for multiple tiles
    [SerializeField]
    public TileBase Tile;
    [SerializeField]
    public TileBase colliderTile;

    [SerializeField]
    private TileBase obstaclesTile;

    private List<TileNode> prevNodes;

    public static TileSpritePlacer instance;

    private void Awake()
    {
        instance = this;
    }
    public Tilemap GetTilemap()
    {
        return this.Tilemap;
    }

    public void PaintFloorTiles(List<TileNode> tileNodes)
    {
        Clear();
        PaintTiles(tileNodes, Tilemap, Tile);
    }

    public void PaintCollisions(List<TileNode> tileNodes, TileNode[,] map)
    {
        Vector2Int position = Vector2Int.zero;
        foreach (TileNode currentTile in tileNodes)
        {
            position.x = currentTile.x;
            position.y = currentTile.y;
            CheckForwalls(map, position);
        }
    }

    private void CheckForwalls(TileNode[,] grid, Vector2Int position)
    {
        Vector2Int wallPosition = Vector2Int.zero;
        for (int i = Mathf.Max(position.x - 1, 0); i <= Mathf.Min(position.x + 1, grid.GetLength(0)); i++)
        {
            for (int j = Mathf.Max(position.y - 1, 0); j <= Mathf.Min(position.y + 1, grid.GetLength(1)); j++)
            {
                TileNode potentialWall = grid[i, j];
                if (potentialWall.value == 0 && !potentialWall.isObstacle)
                {
                    wallPosition.x = i;
                    wallPosition.y = j;
                    PaintSingleTile(Collisions, colliderTile, wallPosition);
                }
                else if (potentialWall.value == 0 && potentialWall.isObstacle)
                {
                    wallPosition.x = i;
                    wallPosition.y = j;
                    PaintSingleTile(Collisions, obstaclesTile, wallPosition);
                }

            }
        }
    }

    private void PaintTiles(List<TileNode> tileNodes, Tilemap tilemap, TileBase tile)
    {
        // Debug.Log("Number of room tiles: " + tileNodes.Count);
        Vector2 position = Vector2.zero;
        foreach (TileNode tilenode in tileNodes)
        {
            position.x = tilenode.x;
            position.y = tilenode.y;
            var visual = (Vector3Int)tilemap.WorldToCell((Vector3)position);
            // tilenode.x = visual.x;
            // tilenode.y = visual.y;
            //Debug.Log("painting tiles, data pos = " + position + ", visual pos = " + visual);
            PaintSingleTile(tilemap, tile, position);
        }
        prevNodes = tileNodes;
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
    {
        // Debug.Log("In paint tiles");
        // Debug.Log("Position: (" + position.x  + ", " + position.y + ")");
        var tilePosition = tilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        // Debug.Log("In Clear");
        if (prevNodes != null)
        {
            Vector2 position = Vector2.zero;
            foreach (TileNode tilenode in prevNodes)
            {
                position.x = tilenode.x;
                position.y = tilenode.y;
                var tilePosition = Tilemap.WorldToCell((Vector3)position);
                Tilemap.SetTile(tilePosition, null);
            }
        }
        // Tilemap.ClearAllTiles();
    }
}