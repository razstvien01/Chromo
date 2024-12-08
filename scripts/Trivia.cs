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
	private ScrollContainer scrollContainer;
	private bool scrollAdjusting = false;
	private double elapsedTime = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		triviaText = GetNode<Label>("%Trivia");
		triviaTitle = GetNode<Label>("%Title");
		triviaImage = GetNode<TextureRect>("%Image");
		triviaNarration = GetNode<AudioStreamPlayer>("%Narration");

		scrollContainer = GetNode<ScrollContainer>("%TriviaScroll");
		scrollContainer.ScrollStarted += () => {
			scrollAdjusting = true;
		};

		scrollContainer.ScrollEnded += () => {
			scrollAdjusting = false;
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		if(!scrollAdjusting && elapsedTime > 1) {
			scrollContainer.ScrollVertical += 3; //Scroll down
			elapsedTime -= 1;
		}
	}
	public void _OnProceedButtonPressed(){
		GD.Print("Proceed Button Pressed.");
		QueueFree(); 
	}
}