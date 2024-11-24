using Godot;
using System;

public partial class LevelCompleted : Control
{
	private string questionScenePath = "res://scenes/Exam.tscn";
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	// _on_back_menu_button_pressed
	public void _OnBackMenuButtonPressed()
	{
		var examScene = (PackedScene)GD.Load(questionScenePath);
		GetTree().ChangeSceneToPacked(examScene);
	}
}
