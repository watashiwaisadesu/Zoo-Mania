
public abstract class BombActivatorBase
{
    protected static Board Board;
    public void Initialize(Board board)
    {
        Board = board;
    }
    public abstract void Activate(int column=0, int row=0);
}



