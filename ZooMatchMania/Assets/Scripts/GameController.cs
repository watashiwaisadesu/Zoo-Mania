using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameState CurrentState  = GameState.Move;
    [SerializeField] private Board board; // Reference to your Board instance
    [SerializeField] private HintManager hintManager;
    [SerializeField] private FindMatches findMatches;
    [SerializeField] private Level level;
    [SerializeField] private Hud hud;
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        hintManager = FindObjectOfType<HintManager>();
        level = FindObjectOfType<Level>();
        hud = FindObjectOfType<Hud>();
        Debug.Log("aa");
    }

    void Start()
    {
        if (board != null)
        {
            board.Initialize();
            findMatches.Initialize(board);
            hintManager.Initialize(board);
            hud.Initialize();
            level.Initialize();
        }
    }
    
}


