using Godot;

public partial class PauseMenu : Control
{
	[Signal]
	public delegate void UnpauseEventHandler();
	[Signal]
	public delegate void RestartEventHandler();


	public string CurrentLevelName {
		set {
			Label currentLabel = GetNode<Label>("%CurrentLevel");
			currentLabel.Text = value;
		}
	}

	public override void _Ready()
	{
		Button returnBtn = GetNode<Button>("%Return");
		returnBtn.Pressed += () => 
		{
			EmitSignal(SignalName.Unpause);
			Hide();
		};

		Button returnToMainMenuBtn = GetNode<Button>("%ReturnToMainMenu");
		returnToMainMenuBtn.Pressed += () =>
		{
			var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
			GetTree().ChangeSceneToPacked(menuScene);
		};

		Button restartBtn = GetNode<Button>("%Restart");
		restartBtn.Pressed += () =>
		{
			EmitSignal(SignalName.Restart);
			Hide();
		};
	}
}
