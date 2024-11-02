using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
	private PackedScene currentScene;
	private Node2D currentInstance;
	private int currentIndex = 0;

	private List<string> scenePaths = new List<string>
	{
		"res://scenes/stages/Tutorial.tscn",
		"res://scenes/stages/Stage_1.tscn"
	};
	private const string DOOR_AREA_PATH = "scenes\\area\\DoorArea.tscn";

	public override void _Ready()
	{
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
			currentInstance = currentScene.Instantiate<Node2D>();
			AddChild(currentInstance);
			
			Area2D doorArea = currentInstance.GetNode<DoorArea>("DoorArea");
			doorArea?.Connect("NextScene", Callable.From(_NextScene));
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePaths[index]}");
		}
	}

	public void _NextScene()
	{
		currentIndex = (currentIndex + 1) % scenePaths.Count;
		LoadScene(currentIndex);
	}
}
