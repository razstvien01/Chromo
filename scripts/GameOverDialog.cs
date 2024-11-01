using Godot;
using System;

public partial class GameOverDialog : Control
{
	private const int INIT_POS_Y = -500;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnYesPressed()
	{
		this.Position = new Vector2(0, INIT_POS_Y);
	}

	public void _OnNoPressed()
	{
		GD.Print("No Hello World");
	}
}
