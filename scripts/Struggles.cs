using Godot;
using System;

public partial class Struggles : Node2D
{
	private const int START_XY_POS = 60;

	[Signal]
	public delegate void NextSceneEventHandler();
	[Signal]
	public delegate void GameOverEventHandler();

	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child.Name.ToString().Contains("Water") && child.HasSignal("ShowGameOver"))
        {
            child?.Connect("ShowGameOver", Callable.From(_ShowGameOver));
        }
		}
	}

	public override void _Process(double delta)
	{
	}

	public void _ShowGameOver()
	{
		GD.Print("Struggles: Show Game Over Emitted");
		EmitSignal(SignalName.GameOver);
	}

	public void _NextScene()
	{
		EmitSignal(SignalName.NextScene);
	}

	public void ResetLevel()
	{
		Character character = GetNode<Character>("Character");
		Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");
		Node2D uiControls = GetNode<Node2D>("Controller");
		Vector2I frameCoords = icon.FrameCoords;

		icon.FrameCoords = new Vector2I(frameCoords.X, 0); //* Resetting color
		character.Position = new Vector2(START_XY_POS, START_XY_POS); //* will reset the position of the character
		uiControls.Visible = true;  //* will show again the UI Controls

		character.ResetSprite();
	}
}
