using Godot;
using System;

public partial class ChooseTrivia : Control
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
	public void _OnTrivia1Pressed()
	{
		LoadTrivia(1);
	}

	public void _OnTrivia2Pressed()
	{
		LoadTrivia(2);
	}

	public void _OnTrivia3Pressed()
	{
		LoadTrivia(3);
	}

	public void _OnTrivia4Pressed()
	{
		LoadTrivia(4);
	}

	public void _OnTrivia5Pressed()
	{
		LoadTrivia(5);
	}

	private void LoadTrivia(int stage)
	{
		GameState.GetInstance().CurrentLoadStage = stage;
		GameState.GetInstance().IsLoadProgress = false;
		GameState.GetInstance().IsLoadTrivias = true;

		string resourcePath = $"res://resources/trivia/Trivia{stage}.tres";
		var triviaResource = ResourceLoader.Load<TriviaResource>(resourcePath);

		var triviaScene = GD.Load<PackedScene>("res://scenes/Trivia.tscn");
		if (triviaScene != null)
		{
			Trivia triviaInstance = triviaScene.Instantiate<Trivia>();
			AddChild(triviaInstance);

			triviaInstance.TriviaResource = triviaResource;
		}
		else
		{
			GD.PrintErr($"Failed to load Trivia scene: {"res://scenes/Trivia.tscn"}");
		}
	}
	
}
