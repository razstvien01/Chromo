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

	private PackedScene currentScene;
	private Struggles currentInstance;
	private int currentIndex = 0;
	private AudioStreamPlayer bgAudioPlayer;
	private GameOverDialog _gameOverDialog;

	private List<string> scenePaths = new List<string>
	{
		// "res://scenes/stages/Tutorial.tscn",
		// "res://scenes/stages/Stage_1.tscn",
		// "res://scenes/stages/Stage_2.tscn",
		// "res://scenes/stages/Stage_3.tscn",
		"res://scenes/stages/Stage_4.tscn",
		"res://scenes/stages/Stage_5.tscn"
	};

	private List<string> audio = new List<string>
	{
		"res://assets/Audio/BG/731713__antenalosmusic__sinister-instrumental-music.wav",
		"res://assets/Audio/BG/670039__seth_makes_sounds__chill-background-music.wav"
	};

	private const string DOOR_AREA_PATH = "scenes\\area\\DoorArea.tscn";

	public override void _Ready()
	{
		bgAudioPlayer = GetNode<AudioStreamPlayer>("%BGAudioPlayer");
		_gameOverDialog = GetNode<GameOverDialog>("%GameOverDialog");

		_gameOverDialog.Connect(nameof(GameOverDialog.ButtonPressed), Callable.From<bool>(OnGameOverDialogPressed));

		LoadScene(currentIndex);
	}

	private void OnGameOverDialogPressed(bool yesClicked)
	{
		if (yesClicked)
		{
			ChangeBgm(AudioEnum.Level);
			currentInstance.ResetLevel();
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

		currentScene = (PackedScene)ResourceLoader.Load(scenePaths[index]);
		if (currentScene != null)
		{
			currentInstance = currentScene.Instantiate<Struggles>();
			AddChild(currentInstance);
			
			// * Make Sure the Scene is behind all of Game's Scenes
			MoveChild(currentInstance, 0);

			// Connect signals
			currentInstance.Connect(nameof(Struggles.GameOver), Callable.From(OnGameOver));

			Area2D doorArea = currentInstance.GetNode<DoorArea>("DoorArea");
			doorArea?.Connect("NextScene", Callable.From(_NextScene));

			ChangeBgm(AudioEnum.Level);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePaths[index]}");
		}
	}

	private void RemoveOldScene()
	{
		RemoveChild(currentInstance);
		currentInstance.QueueFree();
	}

	private void PlaceCurrentInstance()
	{
		MoveChild(currentInstance, 0);
	}


	public void _OnNewSceneReady()
	{
		// * Connect door area signal
		GD.Print("_OnNewSceneReady called");
	}

	public void _NextScene()
	{
		// * Extract the character's values
		CharacterBody2D character = currentInstance.GetNodeOrNull<CharacterBody2D>("Character");
		Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");

		currentIndex = (currentIndex + 1) % scenePaths.Count;
		LoadScene(currentIndex);

		// * Evolve the character
		CharacterBody2D newCharacter = currentInstance.GetNodeOrNull<CharacterBody2D>("Character");
		Sprite2D newIcon = newCharacter.GetNodeOrNull<Sprite2D>("Icon");

		if (newIcon != null && icon != null)
		{
			newIcon.FrameCoords = (icon.FrameCoords.X < 4) ? new Vector2I(icon.FrameCoords.X + 1, 0) : new Vector2I(icon.FrameCoords.X, 0);
			
		}
	}
}
