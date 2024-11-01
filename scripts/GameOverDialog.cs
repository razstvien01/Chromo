using Godot;
using System;

public partial class GameOverDialog : Control
{
	private const int INIT_POS_Y = -500;
	private const int START_XY_POS = 60;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnYesPressed()
	{
		CharacterBody2D character = GetParent().GetNode<CharacterBody2D>("Character");
		Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");
		Node2D uiControls = GetParent().GetNode<Node2D>("Controller");
		Vector2I frameCoords = icon.FrameCoords;

		icon.FrameCoords = new Vector2I(frameCoords.X, 0); //* Resetting color
		character.Position = new Vector2(START_XY_POS, START_XY_POS); //* will reset the position of the character
		Position = new Vector2(0, INIT_POS_Y);  //* hide the button UI
		uiControls.Visible = true;  //* will show again the UI Controls
	}

	public void _OnNoPressed()
	{
		// TODO - Go back to the Main Menu
		GD.Print("No Hello World");
	}
}
