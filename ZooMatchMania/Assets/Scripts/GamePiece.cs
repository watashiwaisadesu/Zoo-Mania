using UnityEngine;

public class GamePiece : MonoBehaviour
{ 
    [SerializeField] private bool isShowingHint;
    [SerializeField] private GamePieceData data;
    public Board Board { get; private set; }
    public GamePieceData GamePieceData { get => data; set => data = value; }
    [SerializeField] private AudioClip spawnSound;
    [field:SerializeField] public AudioClip moveSound { get; private set; }

    public FindMatches FindMatches { get; private set; }
    public HintManager Hintmanager { get; private set; }

    private MovablePiece MovableComponent { get; set; }
    public ColorPiece ColorableComponent { get; private set; }
    public ClearablePiece ClearableComponent { get; private set; }
    
    public Hud Hud { get; private set; }

    public bool IsMovable() => MovableComponent != null;
    public bool IsColored() => ColorableComponent != null;
    public bool IsClearable() => ClearableComponent != null;
    
    
    //Properties
    private void Awake()
    { 
        Board = FindObjectOfType<Board>();
        Hintmanager = FindObjectOfType<HintManager>();
        FindMatches = FindObjectOfType<FindMatches>();
        MovableComponent = GetComponent<MovablePiece>();
        ClearableComponent = GetComponent<ClearablePiece>();
        ColorableComponent = GetComponent<ColorPiece>();
        Hud = FindObjectOfType<Hud>();
    }
    
    private void Start()
    {
       
        if (Hintmanager == null)Debug.LogError("HintManager instance not found.");
        if (FindMatches == null) Debug.LogError("FindMatches instance not found.");
        AudioSource.PlayClipAtPoint(spawnSound, transform.position);
    }
    
    public void Init(int x, int y, PieceType type,MatchedType matchedType=MatchedType.None)
    {
        data.Column = x;
        data.Row = y;
        data.Type = type;
        data.MatchedType = matchedType;
    }

    private void Update()
    {
        if (IsMovable()&&!isShowingHint)
        {
            MovableComponent.UpdatePosition();
        }
    }

    public void IsShowingHint()
    {
        isShowingHint = !isShowingHint;
    }
}


