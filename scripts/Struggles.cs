using Godot;
using System;

public partial class Struggles : Node2D
{
	private Vector2 START_XY_POS;

	[Export]
	public TriviaResource Trivia { get; set; }

	[Signal]
	public delegate void NextSceneEventHandler();
	[Signal]
	public delegate void GameOverEventHandler();
	[Signal]
	public delegate void LoadTriviaEventHandler(TriviaResource triviaResource);
	[Signal]
	public delegate void PauseEventHandler();

	Character character;

	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if ((child.Name.ToString().Contains("Water") || child.Name.ToString().Contains("Puddle")) && child.HasSignal("ShowGameOver"))
			{
				child?.Connect("ShowGameOver", Callable.From(_ShowGameOver));
			}
		}
		
		character = GetNode<Character>("Character");
		START_XY_POS = character.Position;
		character.ResetSprite();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_cancel"))
		{
			EmitSignal(SignalName.Pause);
			UiControlsVisible = false;
		}
	}

	public bool UiControlsVisible {
		set {
			Node2D uiControls = GetNode<Node2D>("Controller");
			uiControls.Visible = value;
		}
	}

	public void _ShowGameOver()
	{
		GD.Print("Struggles: Show Game Over Emitted");
		EmitSignal(SignalName.GameOver);
	}

	public void _NextScene()
	{
		if(Trivia is null)
		{
			EmitSignal(SignalName.NextScene);
		} else 
		{
			EmitSignal(SignalName.LoadTrivia, Trivia);
		}
	}

	public void ResetLevel()
	{
		character = GetNode<Character>("Character");
		
		character.Position = START_XY_POS;
		UiControlsVisible = true; //* will show again the UI Controls

		character.ResetSprite();
	}
}
