using Godot;
using System;

public partial class ChooseTrivia : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnReturnToMainMenuPressed()
	{
		QueueFree();
	}
	public void _OnTrivia1Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 1;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnTrivia2Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 2;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnTrivia3Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 3;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnTrivia4Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 4;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnTrivia5Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 5;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
}
