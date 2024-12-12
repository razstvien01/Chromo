using System.Collections.Generic;
using Godot;

public partial class GameState : Node
{
    private static GameState _instance;
    public int TotalScore { get; set; }
    public int TotalMistakes { get; set; }
    public bool IsLoadProgress { get; set; } = false;
    public bool IsLoadTrivias { get; set; } = false;
    public int CurrentLoadStage { get; set; } = -1;
    public string CurrentLoadTrivia { get; set; } = null;
    public override void _Ready()
    {
        _instance = this;
    }
    public static GameState GetInstance()
    {
        return _instance;
    }
}
