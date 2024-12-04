using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
	private const int GAME_OVER_DIALOG_INIT_POS_Y = -500;

	public enum AudioEnum
	{
		GameOver = 0,
		Level = 1
	}

	private PauseMenu pauseMenu;
	private PackedScene currentScene;
	private Struggles currentInstance;
	private int currentIndex = 0;
	private AudioStreamPlayer bgAudioPlayer;
	private GameOverDialog _gameOverDialog;
	private bool isTriviaActive = false;
	private string triviaScenePath = "res://scenes/Trivia.tscn";
	private string completeScenePath = "res://scenes/LevelCompleted.tscn";
	private string savePath = "user://save_game.json";

	private List<string> scenePaths = new List<string>
	{
		"res://scenes/stages/Tutorial.tscn",
		"res://scenes/stages/Stage_1.tscn",
		"res://scenes/stages/Stage_2.tscn",
		"res://scenes/stages/Stage_3.tscn",
		"res://scenes/stages/Stage_4.tscn",
		"res://scenes/stages/Stage_5.tscn"
	};

	private List<string> audio = new List<string>
	{
		"res://assets/Audio/BG/731713__antenalosmusic__sinister-instrumental-music.wav",
		"res://assets/Audio/BG/670039__seth_makes_sounds__chill-background-music.wav"
	};

	//* This will be set from the Menu scene when transitioning
	public override void _Ready()
    {
        bgAudioPlayer = GetNode<AudioStreamPlayer>("%BGAudioPlayer");
        _gameOverDialog = GetNode<GameOverDialog>("%GameOverDialog");
        pauseMenu = GetNode<PauseMenu>("%PauseMenu");

        pauseMenu.Connect(nameof(PauseMenu.Unpause), Callable.From(Unpause));
        _gameOverDialog.Connect(nameof(GameOverDialog.ButtonPressed), Callable.From<bool>(OnGameOverDialogPressed));

        // Use GameState to determine progress
        currentIndex = GameState.GetInstance().IsLoadProgress ? LoadProgress() : 0;
        LoadScene(currentIndex);
    }
	private void Unpause()
	{
		currentInstance.UiControlsVisible = true;
	}

	private void OnGameOverDialogPressed(bool yesClicked)
	{
		if (yesClicked)
		{
			ChangeBgm(AudioEnum.Level);
			currentInstance.ResetLevel();

			SaveProgress(currentIndex); // Save progress for the current level
		}
		else
		{
			GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/Menu.tscn"));
		}

		_gameOverDialog.Position = new Vector2(0, GAME_OVER_DIALOG_INIT_POS_Y); //* Hide the button UI
	}

	private void OnGameOver()
	{
		GD.Print("Game: Show Game Over Received");
		_gameOverDialog.ShowFromAbove();
		ChangeBgm(AudioEnum.GameOver);
	}

	private void ChangeBgm(AudioEnum audioEnum)
	{
		AudioStream newBg = ResourceLoader.Load<AudioStream>(audio[(int)audioEnum]);
		bgAudioPlayer.Stream = newBg;
		bgAudioPlayer.Play();
	}

	private void LoadScene(int index)
	{
		if (index < 0 || index >= scenePaths.Count)
		{
			GD.PrintErr($"Invalid scene index: {index}");
			return;
		}

		RemoveOldScene();

		string currScenePath = scenePaths[index];
		currentScene = GD.Load<PackedScene>(currScenePath);
		if (currentScene != null)
		{
			currentInstance = currentScene.Instantiate<Struggles>();
			AddChild(currentInstance);
			MoveChild(currentInstance, 0); //* Ensure scene is behind UI

			currentInstance.Connect(nameof(Struggles.GameOver), Callable.From(OnGameOver));

			if (currentInstance is Struggles struggles)
			{
				struggles.Connect(nameof(Struggles.NextScene), Callable.From(_NextScene));
				struggles.Connect(nameof(Struggles.LoadTrivia), Callable.From<TriviaResource>(LoadTriviaScene));
				struggles.Connect(nameof(Struggles.Pause), Callable.From(() => pauseMenu.Show()));
				pauseMenu.CurrentLevelName = struggles.LevelName;
			}

			ChangeBgm(AudioEnum.Level);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {currScenePath}");
		}
	}

	private void RemoveOldScene()
	{
		if (currentInstance != null)
		{
			RemoveChild(currentInstance);
			currentInstance.QueueFree();
			currentInstance = null;
		}
	}

	public void _NextScene()
	{
		if (currentIndex < scenePaths.Count - 1)
		{
			currentIndex++;
			SaveProgress(currentIndex);
			LoadScene(currentIndex);
		}
		else
		{
			var completeScene = GD.Load<PackedScene>(completeScenePath);
			if (completeScene != null)
			{
				GetTree().ChangeSceneToPacked(completeScene);
			}
			else
			{
				GD.PrintErr($"Failed to load completion scene: {completeScenePath}");
			}
		}
	}

	private void LoadTriviaScene(TriviaResource triviaResource)
	{
		RemoveOldScene();
		var triviaScene = GD.Load<PackedScene>(triviaScenePath);
		if (triviaScene != null)
		{
			Trivia triviaInstance = triviaScene.Instantiate<Trivia>();
			AddChild(triviaInstance);

			triviaInstance.TriviaResource = triviaResource;
			triviaInstance.GetNode<Button>("PanelContainer/ScrollContainer/MarginContainer/VBoxContainer/ProceedButton")
				.Connect("pressed", Callable.From(OnTriviaCompleted));
		}
		else
		{
			GD.PrintErr($"Failed to load Trivia scene: {triviaScenePath}");
		}
	}

	private void OnTriviaCompleted()
	{
		RemoveOldScene();
		_NextScene();
	}

	private void SaveProgress(int stageIndex)
	{
		var saveData = new Godot.Collections.Dictionary<string, int>
		{
				{ "stageIndex", stageIndex }
		};

		// Convert the dictionary to a JSON string
		string jsonString = Json.Stringify(saveData);

		using (var file = FileAccess.Open("user://save_game.json", FileAccess.ModeFlags.Write))
		{
			file.StoreString(jsonString);
		}
	}

	private int LoadProgress()
	{
		if (!FileAccess.FileExists(savePath))
		{
			GD.Print("No save file found. Starting a new game.");
			return 0; // Start from the beginning
		}

		using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		var saveData = Json.ParseString(content);

		GD.Print(saveData);  // To print the entire data object, e.g., { "stageIndex": 2 }
		GD.Print("Type of saveData: " + saveData.GetType().Name);  // Type of saveData: Variant

		// Safely cast saveData to a Dictionary<string, Variant>
		var saveDict = saveData.As<Godot.Collections.Dictionary<string, Variant>>();

		// Check if saveDict is valid and contains "stageIndex"
		if (saveDict != null && saveDict.ContainsKey("stageIndex"))
		{
			// Safely convert the value associated with "stageIndex" to an int
			int stageIndex = saveDict["stageIndex"].As<int>();
			GD.Print("Loaded stage index: " + stageIndex);
			return stageIndex;
		}
		else
		{
			GD.Print("No stageIndex found or invalid save data. Starting from the beginning.");
			return 0;
		}
	}
}