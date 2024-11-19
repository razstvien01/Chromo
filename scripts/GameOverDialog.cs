using Godot;
using System;

public partial class GameOverDialog : Control
{
	private AudioStreamPlayer sfxPlayer;
	private AudioStreamPlayer loseSfxPlayer;

	[Signal]
	public delegate void ButtonPressedEventHandler(bool yesClicked);

	public override void _Ready()
	{
		sfxPlayer = GetNode<AudioStreamPlayer>("%SfxPlayer");
		loseSfxPlayer = GetNode<AudioStreamPlayer>("%LoseSfxPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowFromAbove() {
		loseSfxPlayer.Play();
		GetTree().CreateTween().TweenProperty(this, "position", Vector2.Zero, 1f).SetEase(Tween.EaseType.In);
	}

	public void _OnYesPressed()
	{
		sfxPlayer.Play();
		EmitSignal(SignalName.ButtonPressed, true);
	}

	public void _OnNoPressed()
	{
		sfxPlayer.Play();
		EmitSignal(SignalName.ButtonPressed, false);
	}
}
