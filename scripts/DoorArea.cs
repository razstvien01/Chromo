using Godot;
using System;

public partial class DoorArea : Area2D
{
	[Signal]
	public delegate void NextSceneEventHandler();
	// Called when the node enters the scene tree for the first time.
	private bool _doNextScene = false;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_doNextScene)
			EmitSignal(SignalName.NextScene);
	}
	public void _OnDoorAreaBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D character)
		{
			_doNextScene = true;
		}
	}
}
