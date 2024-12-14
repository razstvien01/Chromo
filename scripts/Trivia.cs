using Godot;
using System;
using System.Collections.Generic;

public partial class Trivia : Control
{
	private TriviaResource _triviaResource;
	private Dictionary<int, string[]> triviaImages = new Dictionary<int, string[]>
{
		{ 1, new string[] { "res://assets/Trivias/chromosomes.png" } },
		// { 2, new string[] { "dog", "cat", "rabbit" } },
		// { 3, new string[] { "dog", "cat", "rabbit" } },
		// { 4, new string[] { "dog", "cat", "rabbit" } },
		// { 5, new string[] { "dog", "cat", "rabbit" } },
};

	public TriviaResource TriviaResource
	{
		get => _triviaResource;
		set
		{
			_triviaResource = value;
			if (_triviaResource is null) return;

			triviaText.Text = _triviaResource.Title + _triviaResource.Trivia;
			triviaTitle.Text = _triviaResource.Title;
			GetNode<Label>("PanelContainer/TriviaScroll/MarginContainer/VBoxContainer/Trivia").Text = _triviaResource.Trivia;
			// triviaImage.Texture = _triviaResource.Image;
			triviaNarration.Stream = _triviaResource.Narration;
			triviaAnimation.SpeedScale = _triviaResource.TriviaAnimationSpeed;
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
	private Button proceedButton;
	private AnimationPlayer triviaAnimation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		triviaText = GetNode<Label>("%Trivia");
		triviaTitle = GetNode<Label>("%Title");
		triviaImage = GetNode<TextureRect>("%Image");
		triviaNarration = GetNode<AudioStreamPlayer>("%Narration");
		proceedButton = GetNode<Button>("ProceedButton");
		triviaAnimation = GetNode<AnimationPlayer>("TriviaAnimation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void _OnProceedButtonPressed()
	{
		GD.Print("Proceed Button Pressed.");
		QueueFree();
	}

	void _OnNarrationFinished()
	{
		proceedButton.Visible = true;
		GetNode<PanelContainer>("PanelContainer").Visible = true;
		triviaTitle.Visible = true;
		AddTextureRect(1);
	}

	private void AddTextureRect(int key)
	{
		// Path to the container
		VBoxContainer vbox = GetNode<VBoxContainer>("PanelContainer/TriviaScroll/MarginContainer/VBoxContainer");

		if (!triviaImages.ContainsKey(key))
		{
			GD.PrintErr($"Key {key} does not exist in triviaImages dictionary.");
			return;
		}

		string[] imagePaths = triviaImages[key];

		foreach (string imagePath in imagePaths)
		{
			// Load the texture
			Texture2D texture = GD.Load<Texture2D>(imagePath);
			if (texture == null)
			{
				GD.PrintErr($"Failed to load texture at path: {imagePath}");
				continue;
			}

			// Create a new TextureRect
			TextureRect textureRect = new TextureRect
			{
				Texture = texture,
				StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
				// SizeFlagsHorizontal = (int)Control.SizeFlags.Expand,
				// SizeFlagsVertical = (int)Control.SizeFlags.Expand
			};

			// Add the TextureRect to the VBoxContainer
			vbox.AddChild(textureRect);
		}
	}
}