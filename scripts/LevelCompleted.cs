using Godot;
using System;

public partial class LevelCompleted : Control
{
	private string questionScenePath = "res://scenes/Exam.tscn";
	private RichTextLabel textLabel;
	private string fullText = "[center][i]Congratulations, Genetic Explorer![/i] 🌟[/center]\n\n" +
														"You've unlocked all the parts of the [color=yellow][b]chromosomes[/b][/color] and mastered their secrets! From the [color=pink][i]double helix[/i][/color] to heredity’s [color=lightblue][b]blueprint[/b][/color], your journey revealed the [color=yellow][i]intricate design shaping life[/i][/color].\n\n" +
														"Your [color=lightblue][b]curiosity[/b][/color] and [color=pink][i]determination[/i][/color] shine brightly in the realm of science.\n\n" +
														"[center][color=yellow][b]Thank you[/b][/color] for completing this adventure—keep exploring [color=pink][i]biology’s wonders![/i][/color] 🌟[/center]";

	private Timer timer;
	private string currentText = "";
	private int currentIndex = 0;
	public override void _Ready()
	{
		textLabel = GetNode<RichTextLabel>("%Label");
		timer = GetNode<Timer>("Timer");

		timer.Timeout += OnTimerTimeout;
		timer.WaitTime = 0.01;
		timer.Start();
	}

	private void OnTimerTimeout()
	{
		if (currentIndex < fullText.Length)
		{
			currentText += fullText[currentIndex];
			currentIndex++;
			textLabel.Text = currentText;
		}
		else
		{
			// Stop the timer once all letters are revealed
			timer.Stop();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	// _on_back_menu_button_pressed
	public void _OnBackMenuButtonPressed()
	{
		var examScene = (PackedScene)GD.Load(questionScenePath);
		GetTree().ChangeSceneToPacked(examScene);
	}
	public void _OnNarrationFinished()
	{
		GetNode<Button>("%BackMenuButton").Visible = true;
	}
}
