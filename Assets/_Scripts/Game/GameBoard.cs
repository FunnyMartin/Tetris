using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;

public class GameBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece piece { get; private set; }
    public Piece nextPiece { get; private set; }
    public Piece savedPiece { get; private set; }
    public int score { get; private set; }
    public int linesCleared { get; private set; }
    public int speedLevel { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector2Int sizeOfBoard = new Vector2Int(10, 20);
    public Vector3Int spawnPos = new Vector3Int(-1, 8);
    public Vector3Int previewPosition = new Vector3Int(-1, 12, 0);
    public Vector3Int holdPosition = new Vector3Int(-1, 16, 0);
    public GameOver gameOver;
    public List<TetrominoData> piecesList = new List<TetrominoData>();
    public System.Random rand = new System.Random();
    public TMP_Text scoreCounter;

    //RectInt that had bounds of the playing board.
    public RectInt Bounds
    {
        get
        {
            Vector2Int pos = Vector2Int.zero;
            pos.x = -sizeOfBoard.x / 2;
            pos.y = -sizeOfBoard.y / 2;
            return new RectInt(pos, sizeOfBoard);
        }
    }

    //Resets the piecesList.
    private void ResetPieces()
    {
        piecesList.Clear();
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            piecesList.Add(tetrominoes[i]);
        }
    }

    //Picks a random piece and removes it from piecesList.
    public TetrominoData PickAndRemovePiece()
    {
        if (piecesList.Count == 0)
        {
            ResetPieces();
        }

        int index = rand.Next(piecesList.Count);
        TetrominoData piece = piecesList[index];
        piecesList.RemoveAt(index);
        return piece;
    }

    //ChangeSpeed(), UpdateLevel() and UpdateScoreCounter() is called every frame.
    public void Update()
    {
        ChangeSpeed();
        UpdateLevel();
        UpdateScoreCounter(score);
    }

    //Changes the speed of the game based on speedLevel resulting in progressively harder difficulty.
    public void ChangeSpeed()
    {
        switch (speedLevel)
        {
            case 1:
                piece.delayStep = 1; break;
            case 2:
                piece.delayStep = 0.95f; break;
            case 3:
                piece.delayStep = 0.9f; break;
            case 4:
                piece.delayStep = 0.85f; break;
            case 5:
                piece.delayStep = 0.8f; break;
            case 6:
                piece.delayStep = 0.75f; break;
            case 7:
                piece.delayStep = 0.7f; break;
            case 8:
                piece.delayStep = 0.65f; break;
            case 9:
                piece.delayStep = 0.6f; break;
            case 10:
                piece.delayStep = 0.55f; break;
            case 13:
                piece.delayStep = 0.45f; break;
            case 16:
                piece.delayStep = 0.3f; break;
            case 19:
                piece.delayStep = 0.1f; break;
            case 29:
                piece.delayStep = 0.001f; break;
        }
    }

    //Updates the speed level when 10 or more lines are cleared.
    public void UpdateLevel()
    {
        if(linesCleared >= 10)
        {
            speedLevel++;
            linesCleared = 0;
        }
    }

    //Updates the score counter to the current score value.
    public void UpdateScoreCounter(int score)
    {
        scoreCounter.text = "SCORE:\n" + score;
    }

    //When scene is loaded it initializes all tetrominoes and sets gameObjects.
    private void Awake()
    {
        try
        {
            for (int i = 0; i < tetrominoes.Length; i++)
            {
                tetrominoes[i].Initialize();
            }

            tilemap = GetComponentInChildren<Tilemap>();
            piece = GetComponentInChildren<Piece>();
            nextPiece = gameObject.AddComponent<Piece>();
            savedPiece = gameObject.AddComponent<Piece>();
            nextPiece.enabled = false;
            savedPiece.enabled = false;
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Sets the next piece for preview.
    private void SetNextPiece()
    {
        if (nextPiece.coords != null)
        {
            Clear(nextPiece);
        }

        TetrominoData data = PickAndRemovePiece();

        nextPiece.Initialize(this, previewPosition, data);
        Set(nextPiece);
    }

    //Called when scene loads and sets speedLevel, ResetPieces(), SetNextPiece(), SpawnPiece().
    private void Start()
    {
        speedLevel = 1;
        ResetPieces();
        SetNextPiece();
        SpawnPiece();
    }

    //Spawns a piece into the game board and checks for GameOver() and SetNextPiece() is called.
    public void SpawnPiece()
    {
        try
        {
            piece.Initialize(this, spawnPos, nextPiece.tetrominoData);

            if (PosValid(piece, spawnPos))
            {
                Set(piece);
            }
            else
            {
                GameOver();
            }

            SetNextPiece();
        }
        catch(MissingReferenceException e)
        {
            Debug.Log(e.ToString());
        }
        
    }

    //Swaps a hold piece and next piece. If hold position is null then it creates a new piece at next position.
    public void SwapPiece()
    {
        try
        {
            TetrominoData savedData = savedPiece.tetrominoData;

            if (savedData.coords != null)
            {
                Clear(savedPiece);
                savedPiece.Initialize(this, holdPosition, nextPiece.tetrominoData);
                Set(savedPiece);
                Clear(nextPiece);
                nextPiece.Initialize(this, previewPosition, savedData);
                Set(nextPiece);
            }
            else
            {
                savedPiece.Initialize(this, holdPosition, nextPiece.tetrominoData);
                Set(savedPiece);
                Clear(nextPiece);
                nextPiece.Initialize(this, previewPosition, PickAndRemovePiece());
                Set(nextPiece);
            }
        }
        catch(NullReferenceException e ) 
        {
            Debug.Log(e.ToString());
        }
        
    }

    //Game over, resets values and shows game over screen.
    public void GameOver()
    {
        try
        {
            gameOver.Setup(score);
            Time.timeScale = 0f;
            tilemap.ClearAllTiles();
            score = 0;
            speedLevel = 1;
            AudioManager.instance.sfxSource.Stop();
            AudioManager.instance.PlaySFX("GameOver");
        } 
        catch(NullReferenceException e) 
        {
            Debug.Log(e.ToString());
        }
        
    }

    //Adds score to score counter which is 100 points times speedLevel.
    public void AddScore()
    {
        score += speedLevel * 100;
    }

    //Sets all coords on the tilemap.
    public void Set(Piece piece)
    {
        foreach (Vector3Int coords in piece.coords)
        {
            Vector3Int tilePos = coords + piece.position;
            tilemap.SetTile(tilePos, piece.tetrominoData.tile);
        }
    }

    //Sets all nulls on the tilemap instead of tiles.
    public void Clear(Piece piece)
    {
        foreach (Vector3Int coords in piece.coords)
        {
            Vector3Int tilePos = coords + piece.position;
            tilemap.SetTile(tilePos, null);
        }
    }

    //Checks if all piece positions are valid.
    public bool PosValid(Piece piece, Vector3Int pos)
    {
        RectInt bounds = Bounds;

        foreach (Vector3Int coords in piece.coords)
        {
            Vector3Int tilePos = coords + pos;

            if (!TileValid(bounds, tilePos))
            {
                return false;
            }
        }

        return true;
    }

    //Checks if position is valid, checks if its inside the playing board and if its inside another locked piece.
    private bool TileValid(RectInt bounds, Vector3Int tilePos)
    {
        if (!bounds.Contains((Vector2Int)tilePos))
        {
            return false;
        }

        if (tilemap.HasTile(tilePos))
        {
            return false;
        }

        return true;
    }

    //Checks for line clears in the whole board.
    public void ProcessGrid()
    {
        RectInt gridArea = Bounds;
        int rowIdx = gridArea.yMin;

        while (rowIdx < gridArea.yMax)
        {
            if (CheckFullRow(rowIdx))
            {
                RemoveRow(rowIdx);
            }
            else
            {
                rowIdx++;
            }
        }
    }

    //Checks one row if it is full or not.
    public bool CheckFullRow(int rowIdx)
    {
        RectInt gridArea = Bounds;

        for (int columnIdx = gridArea.xMin; columnIdx < gridArea.xMax; columnIdx++)
        {
            Vector3Int coordPos = new Vector3Int(columnIdx, rowIdx, 0);

            if (!tilemap.HasTile(coordPos))
            {
                return false;
            }
        }

        return true;
    }

    //Removes one row and moves all rows above down. Also calls AddScore() and adds linesCleared++.
    public void RemoveRow(int rowIdx)
    {
        RectInt gridArea = Bounds;

        for (int columnIdx = gridArea.xMin; columnIdx < gridArea.xMax; columnIdx++)
        {
            Vector3Int coordPos = new Vector3Int(columnIdx, rowIdx, 0);
            tilemap.SetTile(coordPos, null);
        }

        MoveRowsDown(rowIdx, gridArea);

        AddScore();
        linesCleared++;
        AudioManager.instance.PlaySFX("LineClear");
    }

    //Starting from a specified row in a given grid area, this method moves each row of tiles down by one position. The top row of the grid area is then cleared.
    private void MoveRowsDown(int startRowIdx, RectInt gridArea)
    {
        for (int rowIdx = startRowIdx + 1; rowIdx < gridArea.yMax; rowIdx++)
        {
            for (int columnIdx = gridArea.xMin; columnIdx < gridArea.xMax; columnIdx++)
            {
                Vector3Int coordPos = new Vector3Int(columnIdx, rowIdx, 0);
                TileBase tileAbove = tilemap.GetTile(coordPos);

                coordPos = new Vector3Int(columnIdx, rowIdx - 1, 0);
                tilemap.SetTile(coordPos, tileAbove);
            }
        }

        for (int columnIdx = gridArea.xMin; columnIdx < gridArea.xMax; columnIdx++)
        {
            Vector3Int coordPos = new Vector3Int(columnIdx, gridArea.yMax - 1, 0);
            tilemap.SetTile(coordPos, null);
        }
    }
}