using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

	public override void _Ready()
	{
		bgAudioPlayer = GetNode<AudioStreamPlayer>("%BGAudioPlayer");
		_gameOverDialog = GetNode<GameOverDialog>("%GameOverDialog");
		pauseMenu = GetNode<PauseMenu>("%PauseMenu");

		pauseMenu.Connect(nameof(PauseMenu.Unpause), Callable.From(Unpause));
		_gameOverDialog.Connect(nameof(GameOverDialog.ButtonPressed), Callable.From<bool>(OnGameOverDialogPressed));

		// Load saved progress
		currentIndex = LoadProgress();
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

			// Save progress for the current level
			SaveProgress(currentIndex);
		}
		else
		{
			var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
			GetTree().ChangeSceneToPacked(menuScene);
		}

		_gameOverDialog.Position = new Vector2(0, GAME_OVER_DIALOG_INIT_POS_Y);  //* hide the button UI
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
		if (currentInstance != null)
		{
			RemoveChild(currentInstance);
			currentInstance.QueueFree();
		}

		string currScenePath = scenePaths[index];
		currentScene = (PackedScene)ResourceLoader.Load(currScenePath);
		if (currentScene != null)
		{
			currentInstance = currentScene.Instantiate<Struggles>();
			AddChild(currentInstance);

			// * Make Sure the Scene is behind all of Game's Scenes
			MoveChild(currentInstance, 0);

			// Connect signals
			currentInstance.Connect(nameof(Struggles.GameOver), Callable.From(OnGameOver));

			if (currentInstance is Struggles struggles)
			{
				struggles.Connect(nameof(Struggles.NextScene), Callable.From(_NextScene));
				struggles.Connect(nameof(Struggles.LoadTrivia), Callable.From<TriviaResource>(LoadTriviaScene));
				struggles.Connect(nameof(Struggles.Pause), Callable.From(() => pauseMenu.Show()));
				pauseMenu.CurrentLevelName = struggles.LevelName;
			}

			for (int i = 1; i <= 5; i++)
			{
				if (currScenePath.Contains(i.ToString()))
				{
					EvolveCharacter(i);
					break;
				}
			}

			ChangeBgm(AudioEnum.Level);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePaths[index]}");
		}
	}


	private void EvolveCharacter(int level)
	{
		GD.Print("Evolved Level called");
		// Retrieve the current character and its icon
		CharacterBody2D character = currentInstance.GetNodeOrNull<CharacterBody2D>("Character");
		Sprite2D icon = character?.GetNodeOrNull<Sprite2D>("Icon");

		// Evolve the character
		CharacterBody2D newCharacter = currentInstance.GetNodeOrNull<CharacterBody2D>("Character");
		Sprite2D newIcon = newCharacter?.GetNodeOrNull<Sprite2D>("Icon");

		// Update the new icon's frame based on the level
		if (newIcon != null && icon != null && level > 1)
		{
			newIcon.FrameCoords = (level < 4)
					? new Vector2I(level, 0)
					: new Vector2I(3, 0);
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


	private void PlaceCurrentInstance()
	{
		MoveChild(currentInstance, 0);
	}

	public void _NextScene()
	{
		if (currentIndex < scenePaths.Count - 1)
		{
			currentIndex++;

			// Save progress before loading the next scene
			SaveProgress(currentIndex);

			LoadScene(currentIndex);
		}
		else
		{
			// All scenes completed, load a summary or question scene
			var completeScene = GD.Load<PackedScene>(completeScenePath);

			if (completeScene != null)
			{
				GD.Print("All stages completed! Proceeding to the question scene.");
				RemoveOldScene();

				GetTree().ChangeSceneToPacked(completeScene);
			}
			else
			{
				GD.PrintErr($"Failed to load the question scene at path: {completeScenePath}");
			}
		}
	}




	private void LoadTriviaScene(TriviaResource triviaResource)
	{
		if (currentInstance != null)
		{
			RemoveOldScene();
		}

		var triviaScene = GD.Load<PackedScene>(triviaScenePath);
		if (triviaScene != null)
		{
			Trivia triviaInstance = triviaScene.Instantiate<Trivia>();
			AddChild(triviaInstance);

			triviaInstance.TriviaResource = triviaResource;

			// Connect Trivia button signal to proceed
			triviaInstance.GetNode<Button>("PanelContainer/ScrollContainer/MarginContainer/VBoxContainer/ProceedButton").Connect("pressed", Callable.From(OnTriviaCompleted));
		}
		else
		{
			GD.PrintErr($"Failed to load Trivia scene at path: {triviaScenePath}");
		}
	}

	private void OnTriviaCompleted()
	{
		GD.Print("Trivia completed!");
		RemoveOldScene(); // Properly clean up Trivia scene
		_NextScene();     // Proceed to the next stage
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
		string savePath = "user://save_game.json";  // Ensure you set the save path if not already defined

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