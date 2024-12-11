using Godot;
using System;
using System.Collections.Generic;

public partial class ChooseLevel : Control
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
	public void _OnLevel1Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 1;
		GameState.GetInstance().IsLoadProgress = false;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnLevel2Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 2;
		GameState.GetInstance().IsLoadProgress = false;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnLevel3Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 3;
		GameState.GetInstance().IsLoadProgress = false;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnLevel4Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 4;
		GameState.GetInstance().IsLoadProgress = false;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
	public void _OnLevel5Pressed()
	{
		GameState.GetInstance().CurrentLoadStage = 5;
		GameState.GetInstance().IsLoadProgress = false;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/stages/Game.tscn"));
	}
}
