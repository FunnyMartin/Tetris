using System.Collections.Generic;
using UnityEngine;

public static class Information
{
    public static readonly float angle = Mathf.PI / 2f;
    public static readonly float cos = (float)Mathf.Cos(angle);
    public static readonly float sin = (float)Mathf.Sin(angle);
    public static readonly float[] rotationMatrix = new float[] { cos, sin, -sin, cos };

    //Dictionary full of coords for each tetromino.
    public static readonly Dictionary<Tetromino, Vector2Int[]> Coords = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, CoordsForI() },
        { Tetromino.O, CoordsForO() },
        { Tetromino.T, CoordsForT() },
        { Tetromino.J, CoordsForJ() },
        { Tetromino.L, CoordsForL() },
        { Tetromino.S, CoordsForS() },
        { Tetromino.Z, CoordsForZ() },
    };

    private static Vector2Int[] CoordsForI()
    {
        return new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) };
    }

    private static Vector2Int[] CoordsForO()
    {
        return new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(1, 0) };
    }

    private static Vector2Int[] CoordsForT()
    {
        return new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) };
    }

    private static Vector2Int[] CoordsForJ()
    {
        return new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) };
    }

    private static Vector2Int[] CoordsForL()
    {
        return new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) };
    }

    private static Vector2Int[] CoordsForS()
    {
        return new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0) };
    }

    private static Vector2Int[] CoordsForZ()
    {
        return new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(1, 0) };
    }

    private static Vector2Int[,] WallKicksI()
    {
        return new Vector2Int[,] {
            { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
            { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
            { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
            { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        };
    }

    private static Vector2Int[,] WallKicksOTJLSZ()
    {
        return new Vector2Int[,] {
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
            { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
            { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        };
    }

    //Returns wall kick tests for each piece.
    public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>()
    {
        { Tetromino.I, WallKicksI() },
        { Tetromino.O, WallKicksOTJLSZ() },
        { Tetromino.T, WallKicksOTJLSZ() },
        { Tetromino.J, WallKicksOTJLSZ() },
        { Tetromino.L, WallKicksOTJLSZ() },
        { Tetromino.S, WallKicksOTJLSZ() },
        { Tetromino.Z, WallKicksOTJLSZ() },
    };
}
