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

	public void _OnBackMenuButtonPressed()
	{
		var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
		GetTree().ChangeSceneToPacked(menuScene);
	}
	// _on_replay_button_pressed
	public void _OnReplayButtonPressed()
	{
		var examScene = (PackedScene)GD.Load("res://scenes/Exam.tscn");
		GetTree().ChangeSceneToPacked(examScene);
	}
}