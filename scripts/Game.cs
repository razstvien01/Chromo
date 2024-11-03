using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
	private PackedScene currentScene;
	private Node2D currentInstance;
	private int currentIndex = 0;
	private AudioStreamPlayer bgAudioPlayer;

	private List<string> scenePaths = new List<string>
	{
		"res://scenes/stages/Tutorial.tscn",
		"res://scenes/stages/Stage_1.tscn"
	};

	private List<string> audio = new List<string>
	{
		"res://assets/Audio/BG/670039__seth_makes_sounds__chill-background-music.wav"
	};

	private const string DOOR_AREA_PATH = "scenes\\area\\DoorArea.tscn";

	public override void _Ready()
	{
		bgAudioPlayer = GetNode<AudioStreamPlayer>("%BGAudioPlayer");
		
		LoadScene(currentIndex);
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
			// * Add new current scene
			currentInstance = currentScene.Instantiate<Node2D>();
			AddChild(currentInstance);

			// * Connect door area signal
			Area2D doorArea = currentInstance.GetNode<DoorArea>("DoorArea");
			doorArea?.Connect("NextScene", Callable.From(_NextScene));

			//Play Audio
			AudioStream newBg = ResourceLoader.Load<AudioStream>(audio[0]);
			bgAudioPlayer.Stream = newBg;
			bgAudioPlayer.Play();
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePaths[index]}");
		}
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
			newIcon.FrameCoords = (icon.FrameCoords.X < 4) ? new Vector2I(icon.FrameCoords.X + 1, icon.FrameCoords.Y) : new Vector2I(icon.FrameCoords.X, icon.FrameCoords.Y);
		}
	}
}
