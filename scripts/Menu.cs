using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var continueButton = GetNode<Button>("ScoreContainer/ContinueButton");
		if (FileAccess.FileExists("user://save_game.json"))
		{
			continueButton.Disabled = false;
			continueButton.Visible = true;
		}
		else
		{
			continueButton.Disabled = true;
			continueButton.Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnStartButtonPressed()
	{
		GameState.GetInstance().IsLoadProgress = false; // Start fresh
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}

	public void _OnExitButtonPressed()
	{
		GetTree().Quit();
	}
	public void _OnContinueButtonPressed()
	{
		GameState.GetInstance().IsLoadProgress = true; // Set load progress
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
}
