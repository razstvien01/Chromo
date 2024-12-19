using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
	private const int GAME_OVER_DIALOG_INIT_POS_Y = -520;

	public enum AudioEnum
	{
		GameOver = 0,
		Level = 1
	}

	private PauseMenu pauseMenu;
	private PackedScene currentScene;
	private Struggles currentInstance;
	private MiniTrivia miniTrivia;
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
		pauseMenu.Connect(nameof(PauseMenu.Restart), Callable.From(OnRestart));
		_gameOverDialog.Connect(nameof(GameOverDialog.ButtonPressed), Callable.From<bool>(OnGameOverDialogPressed));

		if (GameState.GetInstance().CurrentLoadStage != -1)
		{
			LoadScene(GameState.GetInstance().CurrentLoadStage);
			return;
		}

		// Use GameState to determine progress
		currentIndex = GameState.GetInstance().IsLoadProgress ? LoadProgress() : 0;
		miniTrivia = GetNode<MiniTrivia>("%MiniTrivia");
		miniTrivia.Connect(nameof(MiniTrivia.MiniTriviaEnd), Callable.From<string>(OnMiniTriviaEnd));

		LoadScene(currentIndex);
	}
	private void EvolveCharacterIcon(CharacterBody2D character, int level)
	{
		if (character == null)
		{
			GD.PrintErr("Character is null. Cannot evolve the icon.");
			return;
		}

		Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");

		if (icon != null)
		{
			// Calculate new frame coordinate based on level
			int newX = icon.FrameCoords.X + level;

			// Ensure the value doesn't exceed 4
			newX = Mathf.Min(newX, 4);

			// Update the icon's FrameCoords
			icon.FrameCoords = new Vector2I(newX, 0);

			GD.Print($"Icon evolved to frame X: {icon.FrameCoords.X}");
		}
		else
		{
			GD.PrintErr("Icon node not found in character.");
		}
	}


	private void Unpause()
	{
		currentInstance.UiControlsVisible = true;
	}

	private void OnRestart()
	{
		ChangeBgm(AudioEnum.Level);
		currentInstance.ResetLevel();

		SaveProgress(currentIndex); // Save progress for the current level
	}

	private void OnGameOverDialogPressed(bool yesClicked)
	{
		if (yesClicked)
		{
			OnRestart();
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
				struggles.Connect(nameof(Struggles.LoadMiniTrivia), Callable.From<string>(LoadMiniTrivia));
				pauseMenu.CurrentLevelName = struggles.LevelName;
				
				CharacterBody2D character = struggles.GetNodeOrNull<CharacterBody2D>("Character");
				if (character != null)
				{
					EvolveCharacterIcon(character, index);
				}
				else
				{
					GD.PrintErr("Character node not found in the current scene.");
				}
			}

			ChangeBgm(AudioEnum.Level);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {currScenePath}");
		}
	}

	private void LoadMiniTrivia(string animationName) {
		miniTrivia.Show();
		miniTrivia.PlayAnimation(animationName);
	}

	private void OnMiniTriviaEnd(string animationName) {
		miniTrivia.Hide();

		if (currentInstance is Struggles struggles) {
			struggles.UiControlsVisible = true;
			struggles.HideMiniTrivia(animationName);
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
				if (FileAccess.FileExists(savePath))
				{
					DeleteSaveProgress();
				}
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
		if (GameState.GetInstance().CurrentLoadStage != -1)
		{
			GameState.GetInstance().CurrentLoadStage = -1;

			// Load the new scene
			var mainMenuPackedScene = GD.Load<PackedScene>("res://scenes/Menu.tscn");
			if (mainMenuPackedScene != null)
			{
				var mainMenuInstance = mainMenuPackedScene.Instantiate();

				// QueueFree the current scene to detach it properly
				var currentScene = GetTree().CurrentScene;
				if (currentScene != null)
				{
					currentScene.QueueFree();
				}

				// Set the new scene
				GetTree().Root.AddChild(mainMenuInstance);
				GetTree().CurrentScene = mainMenuInstance; // Avoid setting directly if already in tree

				var childScenePacked = GD.Load<PackedScene>("res://scenes/ChooseLevel.tscn");
				if (childScenePacked != null)
				{
					var childSceneInstance = childScenePacked.Instantiate();
					mainMenuInstance.AddChild(childSceneInstance);
				}

				return;
			}
			else
			{
				GD.PrintErr("Failed to load the main menu scene.");
			}

			return;
		}


		RemoveOldScene();
		var triviaScene = GD.Load<PackedScene>(triviaScenePath);
		if (triviaScene != null)
		{
			Trivia triviaInstance = triviaScene.Instantiate<Trivia>();
			AddChild(triviaInstance);

			triviaInstance.TriviaResource = triviaResource;
			triviaInstance.GetNode<Button>("ProceedButton")
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

		// Safely cast saveData to a Dictionary<string, Variant>
		var saveDict = saveData.As<Godot.Collections.Dictionary<string, Variant>>();

		if (saveDict != null && saveDict.ContainsKey("stageIndex"))
		{
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
	private void DeleteSaveProgress()
	{
		// Check if the save file exists
		if (FileAccess.FileExists(savePath))
		{
			// Use FileAccess to remove the file
			DirAccess.RemoveAbsolute(savePath);
			GD.Print($"Save file at {savePath} has been deleted.");
		}
		else
		{
			GD.Print($"Save file at {savePath} does not exist.");
		}
	}

}
