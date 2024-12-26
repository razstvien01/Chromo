using Godot;
using System;
using System.Collections.Generic;

public partial class Trivia : Control
{
	private TriviaResource _triviaResource;

	private readonly Dictionary<int, string[]> triviaImages = new()
		{
				{ 1, new[] { "res://assets/Trivias/chromosomes.png", "res://assets/Trivias/23pairs.png" } },
		};
	private int triviaLevel;
	private RichTextLabel triviaText;
	// private Label triviaTitle;
	private TextureRect triviaImage;
	private AudioStreamPlayer triviaNarration;
	private AudioStreamPlayer congratulationsAudio;
	private Button proceedButton;
	private AnimationPlayer triviaAnimation;
	private AnimationPlayer imageAnimation;
	private string currImageAnimation;
	public TriviaResource TriviaResource
	{
		get => _triviaResource;
		set
		{
			_triviaResource = value;
			if (_triviaResource == null) return;

			UpdateTriviaContent();
		}
	}

	public override void _Ready()
	{
		InitializeNodes();
	}

	private void InitializeNodes()
	{
		triviaText = GetNode<RichTextLabel>("%Trivia");
		// triviaTitle = GetNode<Label>("%Title");
		triviaImage = GetNode<TextureRect>("%Image");
		triviaNarration = GetNode<AudioStreamPlayer>("%Narration");
		congratulationsAudio = GetNode<AudioStreamPlayer>("%CongratulationsAudio");

		congratulationsAudio.Finished += PlayNarration;

		proceedButton = GetNode<Button>("ProceedButton");
		triviaAnimation = GetNode<AnimationPlayer>("TriviaAnimation");
		imageAnimation = GetNode<AnimationPlayer>("ImageAnimation");
	}

	private void PlayCongratulationAudio() {
		congratulationsAudio.Stream = _triviaResource.CongratulationsAudio;
		congratulationsAudio.Play();
		GD.Print("Congratulations will trigger PlayNarration");
	}

	private void PlayNarration() {
		triviaNarration.Stream = _triviaResource.Narration;
		triviaNarration.Play();
	}

	private void UpdateTriviaContent()
	{
		triviaText.Text = _triviaResource.Trivia;
		GetNode<RichTextLabel>("PanelContainer/TriviaScroll/VBoxContainer/Trivia").Text = _triviaResource.Trivia;
		triviaAnimation.SpeedScale = _triviaResource.TriviaAnimationSpeed;
		
		GD.Print("Speed: " + triviaAnimation.SpeedScale);

		int triviaLevel = _triviaResource.TriviaLevel;
		PlayImageAnimation(triviaLevel);

		if(_triviaResource.CongratulationsAudio is not null) {
			PlayCongratulationAudio();
		} else {
			GD.Print("Narration Calling from UpdateTriviaContent");
			PlayNarration();
		}
	}

	private void PlayImageAnimation(int triviaLevel)
	{
		currImageAnimation = $"Trivia_{triviaLevel}";
		if (imageAnimation.HasAnimation(currImageAnimation))
		{
			imageAnimation.Play(currImageAnimation);
			GD.Print($"Playing animation: {currImageAnimation}");
		}
		else
		{
			GD.PrintErr($"Animation '{currImageAnimation}' does not exist in imageAnimation.");
		}
	}
	
	public void _OnProceedButtonPressed()
	{
		GD.Print("Proceed Button Pressed.");
		QueueFree();
	}

	private void _OnNarrationFinished()
	{
		proceedButton.Visible = true;
		SetTriviaPanelVisibility(true);
		// AddTriviaImages(1);
	}

	private void SetTriviaPanelVisibility(bool isVisible)
	{
		GetNode<PanelContainer>("PanelContainer").Visible = isVisible;
		// triviaTitle.Visible = isVisible;
	}

	private void AddTriviaImages(int key)
	{
		VBoxContainer vbox = GetNode<VBoxContainer>("PanelContainer/TriviaScroll/VBoxContainer");

		if (!triviaImages.TryGetValue(key, out string[] imagePaths))
		{
			GD.PrintErr($"Key {key} does not exist in triviaImages dictionary.");
			return;
		}

		foreach (string imagePath in imagePaths)
		{
			AddTextureRectToVBox(vbox, imagePath);
		}
	}

	private void AddTextureRectToVBox(VBoxContainer vbox, string imagePath)
	{
		Texture2D texture = GD.Load<Texture2D>(imagePath);
		if (texture == null)
		{
			GD.PrintErr($"Failed to load texture at path: {imagePath}");
			return;
		}

		TextureRect textureRect = CreateTextureRect(texture);
		vbox.AddChild(textureRect);
	}

	private TextureRect CreateTextureRect(Texture2D texture)
	{
		return new TextureRect
		{
			Texture = texture,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
		};
	}
	
	private void _OnImageAnimationFinished(string animation){
		currImageAnimation = animation;
		if (imageAnimation.HasAnimation(currImageAnimation))
		{
			imageAnimation.Play(currImageAnimation);
			GD.Print($"Playing animation: {currImageAnimation}");
		}
		else
		{
			GD.PrintErr($"Animation '{currImageAnimation}' does not exist in imageAnimation.");
		}
	}
}
