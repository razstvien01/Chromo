using Godot;
using System;

public partial class Struggles : Node2D
{
	private GameOverDialog _gameOverDialog;
	public override void _Ready()
	{
		this._gameOverDialog = GetNode<GameOverDialog>("%GameOverDialog");
	}
	
	public override void _Process(double delta)
	{
	}
	
	public void _ShowGameOver(){
		GetTree().CreateTween().TweenProperty(this._gameOverDialog, "position", Vector2.Zero, 1f).SetEase(Tween.EaseType.In);
		
		
	}
}
