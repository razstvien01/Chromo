using Godot;
using System;

public partial class Trivia : Control
{
	private TriviaResource _triviaResource;
	public TriviaResource TriviaResource {
		get => _triviaResource;
		set {
			_triviaResource = value;
			if(_triviaResource is null) return;

			triviaText.Text = _triviaResource.Trivia;
			triviaTitle.Text = _triviaResource.Title;
			triviaImage.Texture = _triviaResource.Image;
			triviaNarration.Stream = _triviaResource.Narration;
			triviaNarration.Play();
		}
	}

	private Label triviaText;
	private Label triviaTitle;
	private TextureRect triviaImage;
	private AudioStreamPlayer triviaNarration;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		triviaText = GetNode<Label>("%Trivia");
		triviaTitle = GetNode<Label>("%Title");
		triviaImage = GetNode<TextureRect>("%Image");
		triviaNarration = GetNode<AudioStreamPlayer>("%Narration");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _OnProceedButtonPressed(){
		GD.Print("Proceed Button Pressed.");
		QueueFree(); 
	}
}