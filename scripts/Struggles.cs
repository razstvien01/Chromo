using Godot;
using System;
using System.Linq;

public partial class Struggles : Node2D
{
	private Vector2 START_XY_POS;

	[Export]
	public string LevelName { get; set; }
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
	[Signal]
	public delegate void LoadMiniTriviaEventHandler(string tiviaAnimationName);

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
		ShowAllMiniTrivia();
	}

	private void _on_mini_trivia_area_mini_trivia_started(string animationName) {
		UiControlsVisible = false;
		EmitSignal(SignalName.LoadMiniTrivia, animationName);
	}

	public void HideMiniTrivia(string miniTriviaAnimationName) {
		var miniTriviaArea = GetChildren().First(x => x is MiniTriviaArea miniTriviaArea && miniTriviaArea.AnimationName.Equals(miniTriviaAnimationName)) as MiniTriviaArea;
		miniTriviaArea.Disable();
	}

	public void ShowAllMiniTrivia() =>
		GetChildren().Where(x => x is MiniTriviaArea)
					 .Select(x => x as MiniTriviaArea)
					 .ToList()
					 .ForEach(x => {
						x.Enable();
					 });
	
}
