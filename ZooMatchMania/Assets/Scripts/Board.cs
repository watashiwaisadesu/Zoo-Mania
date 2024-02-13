using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public struct PiecePrefab
{
    public PieceType type;
    public GamePiece prefab;
};
   
[System.Serializable]
public struct PiecePosition
{
    public PieceType type;
    public int x;
    public int y;
};


[RequireComponent(typeof(BoardAnimation))]
public class Board : SerializedMonoBehaviour
{
   
    public int streak = 1;
    private bool _gameOver;
    public bool IsGameOver => _gameOver;
    public void GameOver() => _gameOver = true;
    
    [field: SerializeField ] public int Width { get; private set; }
    [field: SerializeField ] public int Height { get; private set; }
    [field: SerializeField ] public int Offset { get; private set; }
   
    public PiecePrefab[] piecePrefabs;
    public PiecePosition[] initialPieces;
    internal GamePiece[,] AllPieces;
    public GamePiece CurrentGamePiece { get; set; }
    private Dictionary<PieceType,GamePiece> _piecePrefabDict;
    public Dictionary<PieceType,GamePiece> PiecePrefabDict=>_piecePrefabDict;
    
    [Header("Tile stuff")]
    [SerializeField] private NormalTile backgroundNormalTile;
    public List<BoardLayout> tileType;
    public BoardLayoutArea[] tileTypeArray; 
    public BreakableTile breakableTilePrefab;
    internal BreakableTile[,] BreakableTiles;
    public bool[,] BlankSpaces;
    //Game Controllers
   
    public HintManager HintManager { get; set; }
    public FindMatches findMatches;

    //Board Components
    public BoardAnimation BoardAnim { get; private set; }
    public DetectDeadLock DeadLock { get; private set; }
    public BoardCheck BoardCheck { get; private set; }

    private void OnValidate()
    {
        ClampTileArray(tileTypeArray);
        BlankSpaces = new bool[Width, Height];
    }
    
    public void Initialize()
    {
        findMatches = FindObjectOfType<FindMatches>();
        HintManager = FindObjectOfType<HintManager>();
        BreakableTiles = new BreakableTile[Width, Height];
        BoardAnim = GetComponent<BoardAnimation>();
        BoardCheck = new BoardCheck();
        DeadLock = new DetectDeadLock();
        BoardAnim.Initialize(this);
        BoardCheck.Initialize(this);
        DeadLock.Initialize(this);
        
        _piecePrefabDict = new Dictionary<PieceType, GamePiece>();
        AllPieces = new GamePiece[Width, Height];
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            PiecePrefabDict.TryAdd(piecePrefabs[i].type, piecePrefabs[i].prefab);
        }
        for (int i = 0; i < initialPieces.Length; i++)
        {
            if (initialPieces[i].x >= 0 && initialPieces[i].y < Width
                                        && initialPieces[i].y >=0 && initialPieces[i].y <Height)
            {
                BoardAnim.SpawnNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);
            }
        }
        Setup();
    }
    
    private void ClampTileArray(BoardLayoutArea[] boardLayoutAreas)
    {
        if (boardLayoutAreas != null)
        {
            foreach (var tile in boardLayoutAreas)
            {
                tile.xStart = (tile.xStart > Width) ? Width - 1 : tile.xStart;
                tile.yStart = (tile.yStart > Height) ? Height - 1 : tile.yStart;

                tile.xEnd = (tile.xEnd <= tile.xStart) ? tile.xStart + 1 : (tile.xEnd > Width) ? Width : tile.xEnd;
                tile.yEnd = (tile.yEnd <= tile.yStart) ? tile.yStart + 1 : (tile.yEnd > Height) ? Height : tile.yEnd;
            }
        }
    }
    
    private void GenerateBreakableTiles()
    {
        foreach (BoardLayoutArea bL in tileTypeArray)
        {
            for (int i = bL.xStart; i <= bL.xEnd; i++)
            {
                for (int j = bL.yStart; j <= bL.yEnd; j++)
                {
                    if (bL.tileKind == TileKind.Breakable)
                    {
                        BoardLayout newBoardLayout = new BoardLayout();
                        if (bL.tileKind == TileKind.Breakable)
                        {
                            newBoardLayout.tileKind = TileKind.Breakable;
                            newBoardLayout.xStart = i;
                            newBoardLayout.yStart = j;
                            this.tileType.Add(newBoardLayout);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < tileType.Count; i++)
        {
            if (tileType[i].tileKind == TileKind.Breakable)
            {
                Vector2 tempPosition = new Vector2(tileType[i].xStart, tileType[i].yStart);
                BreakableTile tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                BreakableTiles[tileType[i].xStart, tileType[i].yStart] = tile;
            }
        }
    }
    
    private void GenerateBlankSpaces()
    {
        foreach (BoardLayoutArea bL in tileTypeArray)
        {
            for (int i = bL.xStart; i <= bL.xEnd; i++)
            {
                for (int j = bL.yStart; j <= bL.yEnd; j++)
                {
                    BoardLayout newBoardLayout = new BoardLayout();
                    if (bL.tileKind == TileKind.Blank)
                    {
                        newBoardLayout.tileKind = TileKind.Blank;
                        newBoardLayout.xStart = i;
                        newBoardLayout.yStart = j;
                        this.tileType.Add(newBoardLayout);
                    }
                }
            }
        }
        for (int i = 0; i < tileType.Count; i++)
        {
            if (tileType[i].tileKind == TileKind.Blank)
            {
                BlankSpaces[tileType[i].xStart, tileType[i].yStart] = true;
            }
        }
    }
    
    private void GenerateBackground()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (!BlankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j);
                    NormalTile bgGo = Instantiate(backgroundNormalTile, tempPosition, Quaternion.identity);
                    bgGo.name = $"BG ( {i} , {j} )";
                    bgGo.transform.parent = this.transform.Find("BG Container");
                }
            }
        }
    }
    private void Setup()
    {
        GenerateBlankSpaces();
        GenerateBackground();
        GenerateBreakableTiles();
        GameController.CurrentState=GameState.Wait;
        BoardAnim.FillBoard();
    }
}



