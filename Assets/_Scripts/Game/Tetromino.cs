using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, O, T, J, L, S, Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tile tile;
    public Vector2Int[] coords;
    public Tetromino tetromino;
    public Vector2Int[,] wallKicks;

    //Used for initializing each tetromino piece.
    public void Initialize()
    {
        try
        {
            coords = Information.Coords[tetromino];
            wallKicks = Information.WallKicks[tetromino];
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }
}