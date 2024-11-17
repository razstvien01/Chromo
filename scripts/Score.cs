using Godot;
using System;

public partial class Score : Control
{
	public int TotalScore { get; set; }
	public int TotalMistakes { get; set; }
	private string _baseScorePath = "ScoreContainer";
	private Label _scoreLabel;
	public override void _Ready()
	{
		GameState gameState = GameState.GetInstance();
		
		GD.Print("Total Score: " + TotalScore);
		GD.Print("Total Mistakes: " + TotalMistakes);
		
		_scoreLabel = GetNode<Label>(_baseScorePath + "/" + "ScoreLabel");
		_scoreLabel.Text = TotalScore + "" + gameState.TotalScore;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
