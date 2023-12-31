using System;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameBoard gameBoard { get; private set; }
    public TetrominoData tetrominoData { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] coords { get; private set; }
    public Piece nextPiece { get; private set; }
    public Piece savedPiece { get; private set; }
    public int rotationIdx { get; private set; }

    public float delayStep = 1f, delayMove = 0.1f, delayLock = 0.5f;
    private float timeStep, timeMove, timeLock;

    //Initializes gameBoard, position and tetrominoData, sets times and coords.
    public void Initialize(GameBoard gameBoard, Vector3Int pos, TetrominoData tetrominoData)
    {
        try
        {
            this.gameBoard = gameBoard;
            position = pos;
            this.tetrominoData = tetrominoData;
            rotationIdx = 0;
            SetTimes();
            SetCoords(tetrominoData);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Sets times for step, move and lock, and is called by Initialize().
    private void SetTimes()
    {
        timeStep = Time.time + delayStep;
        timeMove = Time.time + delayMove;
        timeLock = 0f;
    }

    //Sets coords to this piece and is called by Initialize().
    private void SetCoords(TetrominoData tetrominoData)
    {
        this.coords = this.coords ?? new Vector3Int[tetrominoData.coords.Length];

        for (int i = 0; i < coords.Length; i++)
        {
            coords[i] = (Vector3Int)tetrominoData.coords[i];
        }
    }

    //Handles showing tiles, lock time, rotations, drops, movements and steps every frame.
    private void Update()
    {
        ClearBoard();
        UpdateLockTime();
        HandleRotation();
        HandleDrop();
        HandleMovement();
        AdvancePiece();
        SetBoard();
    }

    //Clears this board of all tiles.
    private void ClearBoard()
    {
        gameBoard.Clear(this);
    }

    //Updates lock time by Time.deltaTime.
    private void UpdateLockTime()
    {
        timeLock += Time.deltaTime;
    }

    //Calls method Rotate(), when Q or E is pressed.
    private void HandleRotation()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.Q) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
            {
                Rotate(-1);
                AudioManager.instance.PlaySFX("Rotation");
            }
            else if (Input.GetKeyDown(KeyCode.E) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
            {
                Rotate(1);
                AudioManager.instance.PlaySFX("Rotation");
            }
        } 
        catch(NullReferenceException e) 
        {
            Debug.Log(e.ToString());
        }
        
    }

    //Calls method HardDrop(), when spacebar is pressed.
    private void HandleDrop()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.Space) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
            {
                HardDrop();
                AudioManager.instance.PlaySFX("Drop");
            }
        }
        catch(NullReferenceException e) 
        {
            Debug.Log(e.ToString());
        }
        
    }

    //Calls method HandleMoveInputs(), when current time is bigger than timeMove.
    private void HandleMovement()
    {
        if (Time.time > timeMove)
        {
            HandleMoveInputs();
        }
    }

    //Calls method Step(), when current time is bigger than timeStep. 
    private void AdvancePiece()
    {
        if (Time.time > timeStep)
        {
            Step();
        }
    }

    //Sets tiles on tilemap.
    private void SetBoard()
    {
        gameBoard.Set(this);
    }

    //Handles players input, S = soft drop, A or D = sideways movement, Shift = holding piece.
    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.S) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
        {
            bool moved = Move(Vector2Int.down);
            timeStep = moved ? Time.time + delayStep : timeStep;
        }

        Vector2Int dir = Input.GetKey(KeyCode.A) ? Vector2Int.left : Vector2Int.right;
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
        {
            Move(dir);
        }

        try
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !PauseMenu.gameIsPaused && !GameOver.gameIsOver)
            {
                gameBoard.SwapPiece();
                AudioManager.instance.PlaySFX("Hold");
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Moves piece down by 1 grid after certain time. If timeLock is bigger than delayLock then the piece gets locked in place.
    private void Step()
    {
        bool moved = Move(Vector2Int.down);
        timeStep = moved ? Time.time + delayStep : timeStep;

        if (timeLock >= delayLock)
        {
            Lock();
        }
    }

    //Moves a piece to the bottom and then locks it in place.
    private void HardDrop()
    {
        while (true)
        {
            if (!Move(Vector2Int.down))
            {
                break;
            }
        }

        Lock();
    }

    //Locks the piece in place, checks for line clears and adds a new piece.
    private void Lock()
    {
        gameBoard.Set(this);
        gameBoard.ProcessGrid();
        gameBoard.SpawnPiece();
    }

    //Checks if movement of piece is valid and then moves accordingly, returns bool of move validity.
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPos = CalculateNewPosition(translation);
        bool valid = gameBoard.PosValid(this, newPos);

        if (valid)
        {
            UpdatePosition(newPos);
        }

        return valid;
    }

    //Calculates new position from argument variable and returns new position.
    private Vector3Int CalculateNewPosition(Vector2Int translation)
    {
        Vector3Int newPos = position;
        newPos.x += translation.x;
        newPos.y += translation.y;
        return newPos;
    }

    //Updates position of a piece, updates timeMove by current time and delayMove and updates timeLock to zero.
    private void UpdatePosition(Vector3Int newPos)
    {
        position = newPos;
        timeMove = Time.time + delayMove;
        timeLock = 0f;
    }

    //Saves original rotation, rotates the piece, if rotation isnt valid then reverts back to original rotation.
    private void Rotate(int dir)
    {
        int originalRotation = rotationIdx;
        RotateCoords(dir);

        if (!TestWallKicks(rotationIdx, dir))
        {
            RevertRotation(originalRotation, dir);
        }
    }

    //Rotates the piece using rotation matrix
    private void RotateCoords(int dir)
    {
        rotationIdx = Wrap(rotationIdx + dir, 0, 4);
        ApplyRotationMatrix(dir);
    }

    //Rotates the piece using rotation matrix backwards
    private void RevertRotation(int originalRotation, int dir)
    {
        rotationIdx = originalRotation;
        ApplyRotationMatrix(-dir);
    }

    //Applies rotation matrix from static class Information on every part of the piece.
    private void ApplyRotationMatrix(int dir)
    {
        float[] matrix = Information.rotationMatrix;

        for (int i = 0; i < coords.Length; i++)
        {
            Vector3 coord = coords[i];
            Vector3Int rotatedCoord = RotateCoord(coord, matrix, dir);
            coords[i] = rotatedCoord;
        }
    }

    //Rotates one part and offsets I and O as they have different rotation points. Returns rotated coord.
    private Vector3Int RotateCoord(Vector3 coord, float[] matrix, int dir)
    {
        int x, y;

        switch (tetrominoData.tetromino)
        {
            case Tetromino.I:
            case Tetromino.O:
                coord.x -= 0.5f;
                coord.y -= 0.5f;
                x = Mathf.CeilToInt((coord.x * matrix[0] * dir) + (coord.y * matrix[1] * dir));
                y = Mathf.CeilToInt((coord.x * matrix[2] * dir) + (coord.y * matrix[3] * dir));
                break;

            default:
                x = Mathf.RoundToInt((coord.x * matrix[0] * dir) + (coord.y * matrix[1] * dir));
                y = Mathf.RoundToInt((coord.x * matrix[2] * dir) + (coord.y * matrix[3] * dir));
                break;
        }

        return new Vector3Int(x, y, 0);
    }

    //Tests wall kicks from static class Information, if any rotation is valid, returns true, if every rotation is invalid, returns false.
    private bool TestWallKicks(int rotationIdx, int rotationDir)
    {
        int wallKickIndex = GetWallKickIndex(rotationIdx, rotationDir);
        Vector2Int[] wallKicksArray = new Vector2Int[tetrominoData.wallKicks.GetLength(1)];

        for (int i = 0; i < tetrominoData.wallKicks.GetLength(1); i++)
        {
            wallKicksArray[i] = tetrominoData.wallKicks[wallKickIndex, i];
        }

        foreach (Vector2Int translation in wallKicksArray)
        {
            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    //Calculates wall kick index and wraps it so its in a valid range.
    private int GetWallKickIndex(int rotationIdx, int rotationDir)
    {
        int wallKickIdx = rotationIdx * 2 + (rotationDir < 0 ? -1 : 0);
        return Wrap(wallKickIdx, 0, tetrominoData.wallKicks.GetLength(0));
    }

    //Wrap is an utility function that wraps an input value around to ensure it is within a specified range.
    private int Wrap(int input, int min, int max)
    {
        return input < min ? max - (min - input) % (max - min) : min + (input - min) % (max - min);
    }
}
