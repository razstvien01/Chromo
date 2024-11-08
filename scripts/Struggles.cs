using Godot;
using System;

public partial class Struggles : Node2D
{
	private const int START_XY_POS = 60;

	[Signal]
	public delegate void NextSceneEventHandler();
	[Signal]
	public delegate void GameOverEventHandler();
	
	Character character;
	
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child.Name.ToString().Contains("Water") && child.HasSignal("ShowGameOver"))
        {
            child?.Connect("ShowGameOver", Callable.From(_ShowGameOver));
        }
		}
		
		character = GetNode<Character>("Character");
		character.ResetSprite();
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
		character = GetNode<Character>("Character");
		Node2D uiControls = GetNode<Node2D>("Controller");
		
		character.Position = new Vector2(START_XY_POS, START_XY_POS); //* will reset the position of the character
		uiControls.Visible = true;  //* will show again the UI Controls

		character.ResetSprite();
	}
}
