using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}	
	
	public void _OnStartButtonPressed(){
		var gameScene = (PackedScene)GD.Load("res://scenes/stages/Game.tscn");
		GetTree().ChangeSceneToPacked(gameScene);
	}
	// _on_option_button_pressed
	public void _OnOptionButtonPressed(){
		var menuScene = (PackedScene)GD.Load("res://scenes/Option.tscn");
		GetTree().ChangeSceneToPacked(menuScene);
	}
	
	public void _OnViewScoresPressed(){
		var menuScene = (PackedScene)GD.Load("res://scenes/ViewScores.tscn");
		GetTree().ChangeSceneToPacked(menuScene);
	}
}
