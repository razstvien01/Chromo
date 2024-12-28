using Godot;
using System;

public partial class Score : Control
{
	public int TotalScore { get; set; }
	public int TotalMistakes { get; set; }
	private string _baseScorePath = "ScoreContainer";
	private Label _scoreLabel;
	private double PassingPercentage = 0.6;
	public override void _Ready()
	{
		GameState gameState = GameState.GetInstance();

		GD.Print("Total Score: " + TotalScore);
		GD.Print("Total Mistakes: " + TotalMistakes);

		_scoreLabel = GetNode<Label>(_baseScorePath + "/" + "ScoreLabel");
		_scoreLabel.Text = TotalScore + "" + gameState.TotalScore;

		Label feedback = GetNode<Label>("%Feedback");
		AudioStreamPlayer feedbackPlayer = GetNode<AudioStreamPlayer>("%FeedbackAudio");
		double passingScore = Mathf.Round(gameState.TotalScore * PassingPercentage);
		AudioStream feedbackAudio;

		if(TotalScore >= passingScore)
		{
			feedback.Text = "Congratulations, You passed the exam!";
			feedbackAudio = ResourceLoader.Load<AudioStream>("res://assets/Audio/Narration/YOU ARE AWESOME (EXAM PASSED).wav");
		}
		else
		{
			feedback.Text = " You failed the exam, better luck next time!";
			feedbackAudio = ResourceLoader.Load<AudioStream>("res://assets/Audio/Narration/YOU CAN DO IT BETTET NEXT TIME (IF FAILED EXAM).wav");
		}

		feedbackPlayer.Stream = feedbackAudio;
		feedbackPlayer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnBackMenuButtonPressed()
	{
		var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
		GetTree().ChangeSceneToPacked(menuScene);
	}
	// _on_replay_button_pressed
	public void _OnReplayButtonPressed()
	{
		var examScene = (PackedScene)GD.Load("res://scenes/Exam.tscn");
		GetTree().ChangeSceneToPacked(examScene);
	}
}