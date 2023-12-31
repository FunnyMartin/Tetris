using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Shadow : MonoBehaviour
{
    public Tile shadowTile;
    public GameBoard gameBoard;
    public Piece pieceToTrack;
    private Tilemap shadowTilemap;
    private Vector3Int[] shadowCoords;
    private Vector3Int shadowPos;

    //Initializes shadowTilemap and shadowCoords.
    private void Initialize()
    {
        try
        {
            shadowTilemap = GetComponentInChildren<Tilemap>();
            shadowCoords = new Vector3Int[4];
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Updates the shadow of the piece.
    private void UpdatePhantom()
    {
        RemoveOldTiles();
        ClonePieceCells();
        CalculateDropPosition();
        PlaceNewTiles();
    }

    //Clones the coordinates of the tracked piece.
    private void ClonePieceCells()
    {
        shadowCoords = (Vector3Int[])pieceToTrack.coords.Clone();
    }

    //Calculates the drop position of the shadow by looping to the bottom and breaking the loop when position is invalid.
    private void CalculateDropPosition()
    {
        Vector3Int piecePos = pieceToTrack.position;
        int boardBottom = -gameBoard.sizeOfBoard.y / 2 - 1;
        gameBoard.Clear(pieceToTrack);

        for (int y = piecePos.y; y >= boardBottom; y--)
        {
            piecePos.y = y;

            if (gameBoard.PosValid(pieceToTrack, piecePos))
            {
                shadowPos = piecePos;
            }
            else
            {
                break;
            }
        }

        gameBoard.Set(pieceToTrack);
    }

    //Places new tiles on the shadowTilemap.
    private void PlaceNewTiles()
    {
        foreach (var cell in shadowCoords)
        {
            shadowTilemap.SetTile(cell + shadowPos, shadowTile);
        }
    }

    //Removes old tiles from shadowTilemap by setting them to null.
    private void RemoveOldTiles()
    {
        foreach (var coord in shadowCoords)
        {
            shadowTilemap.SetTile(coord + shadowPos, null);
        }
    }

    //Calls Initialize when the script instance is being loaded.
    private void Awake()
    {
        Initialize();
    }

    //Calls UpdatePhantom after all Update functions have been called.
    private void LateUpdate()
    {
        UpdatePhantom();
    }
}