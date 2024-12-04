using Godot;

public partial class GameState : Node
{
    private static GameState _instance;
    public int TotalScore { get; set; }
    public int TotalMistakes { get; set; }
    public bool IsLoadProgress { get; set; } = false;
    public override void _Ready()
    {
        _instance = this;
    }
    public static GameState GetInstance()
    {
        return _instance;
    }
}
