using Godot;
using System;

public partial class Menu : Control
{
	private Button _continueButton;

	public override void _Ready()
	{
		_continueButton = GetNode<Button>("%ContinueButton");
		UpdateContinueButtonState();
	}

	private void UpdateContinueButtonState()
	{
		bool saveExists = FileAccess.FileExists("user://save_game.json");
		_continueButton.Disabled = !saveExists;
		_continueButton.Visible = saveExists;
	}

	private void AddSceneToCurrent(string scenePath)
	{
		PackedScene sceneToAdd = GD.Load<PackedScene>(scenePath);
		if (sceneToAdd != null)
		{
			Node sceneInstance = sceneToAdd.Instantiate();
			AddChild(sceneInstance);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePath}");
		}
	}

	public override void _Process(double delta)
	{

	}

	private void ChangeScene(string scenePath, bool loadProgress)
	{
		GameState.GetInstance().IsLoadProgress = loadProgress;
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>(scenePath));
	}

	private void _OnStartButtonPressed()
	{
		ChangeScene("res://scenes/stages/Game.tscn", false);
	}

	private void _OnContinueButtonPressed()
	{
		ChangeScene("res://scenes/stages/Game.tscn", true);
	}

	private void _OnChooseLevelButtonPressed()
	{
		AddSceneToCurrent("res://scenes/ChooseLevel.tscn");
	}

	private void _OnTriviaButtonPressed()
	{
		AddSceneToCurrent("res://scenes/ChooseTrivia.tscn");
	}
	private void _OnExamPressed()
	{
		ChangeScene("res://scenes/Exam.tscn", false);
	}

	private void _OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
