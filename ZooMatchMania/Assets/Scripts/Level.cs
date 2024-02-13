using UnityEngine;

public class Level : MonoBehaviour
{
    internal Board board;
    internal Hud hud;

    public int score1Star;
    public int score2Star;
    public int score3Star;    

    protected LevelType type;

    protected int CurrentScore;

    private bool _didWin;

    public virtual void Initialize()
    {
        board = FindObjectOfType<Board>();
        hud = FindObjectOfType<Hud>();
        hud.SetScore(CurrentScore);
    }

    public LevelType Type => type;

    protected virtual void GameWin()
    {
        board.GameOver();
        _didWin = true;
        WaitForGridFill();
    }

    protected virtual void GameLose()
    {        
        board.GameOver();
        _didWin = false;
        WaitForGridFill();
    }

    public virtual void OnMove()
    {
    }

    public virtual void OnPieceCleared(GamePiece piece)
    {
        CurrentScore += piece.GamePieceData.score * board.streak;
        hud.SetScore(CurrentScore);
    }

    protected virtual void WaitForGridFill()
    {
         if (_didWin)
         {
             hud.OnGameWin(CurrentScore);
         }
         else
         {
             hud.OnGameLose();
         }
    }
}
