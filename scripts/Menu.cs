using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// TODO detect if there's a save point, if so, then add the CONTINUE button
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}	
	
	public void _OnStartButtonPressed(){
		var gameScene = (PackedScene)GD.Load("res://scenes/stages/Game.tscn");
		GetTree().ChangeSceneToPacked(gameScene);
	}
	
	public void _OnExitButtonPressed(){
		GetTree().Quit();
	}
	public void _OnContinueButtonPressed(){
		
	}
}
