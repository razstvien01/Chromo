using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var continueButton = GetNode<Button>("VBoxContainer/ContinueButton");
		if (FileAccess.FileExists("user://save_game.json"))
		{
			continueButton.Disabled = false;
		}
		else
		{
			continueButton.Disabled = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnStartButtonPressed()
	{
		var gameScene = (PackedScene)GD.Load("res://scenes/stages/Game.tscn");
		GetTree().ChangeSceneToPacked(gameScene);
	}

	public void _OnExitButtonPressed()
	{
		GetTree().Quit();
	}
	public void _OnContinueButtonPressed()
	{
		// TODO Add a confirmation dialog when the player starts a new game, warning them that existing progress will be overwritten.
		// TODO Provide visual feedback when loading save data fails.
		var gameScene = GD.Load<PackedScene>("res://scenes/stages/Game.tscn");
		GetTree().ChangeSceneToPacked(gameScene);
	}
}
