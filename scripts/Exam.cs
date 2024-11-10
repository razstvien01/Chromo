using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class Exam : Control
{
	private const string QUESTIONS_PATH = "res://questions.json";
	private Label questionNumber;
	private Label question;
	private TextureRect picture;
	private List<Button> options = new List<Button>();
	private ExamModel exam;
	private int currentQuestionIndex = 0;

	public override void _Ready()
	{
		questionNumber = GetNode<Label>("QuestionContainer/Number");
		question = GetNode<Label>("QuestionContainer/Question");
		picture = GetNode<TextureRect>("Picture");

		for (int i = 0; i < 4; ++i)
		{
			Button optionButton = GetNode<Button>("OptionContainer/Option" + (i + 1));
			options.Add(optionButton);

			// TODO Connect the button's pressed signal to a function to handle answers
			optionButton.Connect("pressed", Callable.From(
				() =>
				{
					this.OnOptionSelected(i);
				}
				)
			);
		}

		LoadJson();
		LoadExam();
	}

	private void LoadJson()
	{
		FileAccess file = FileAccess.Open(QUESTIONS_PATH, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			GD.PrintErr("Failed to open the JSON file.");
			return;
		}

		string jsonContent = file.GetAsText();
		file.Close();

		exam = JsonConvert.DeserializeObject<ExamModel>(jsonContent);
	}

	private void LoadExam()
	{
		if (currentQuestionIndex >= exam.Questions.Count)
		{
			GD.Print("Exam completed!");
			return;
		}

		QuestionModel currentQuestion = exam.Questions[currentQuestionIndex];

		questionNumber.Text = $"Question {currentQuestionIndex + 1}";
		question.Text = currentQuestion.Question;

		if (!string.IsNullOrEmpty(currentQuestion.ImgPath))
		{
			var imageTexture = GD.Load<Texture2D>(currentQuestion.ImgPath);
			picture.Texture = imageTexture;
		}
		else
		{
			picture.Texture = null;
		}

		for (int i = 0; i < options.Count; i++)
		{
			options[i].Text = currentQuestion.Options[i];
		}
	}

	private void OnOptionSelected(int selectedOptionIndex)
	{
		QuestionModel currentQuestion = exam.Questions[currentQuestionIndex];

		if (currentQuestion.Options[selectedOptionIndex] == currentQuestion.Answer)
		{
			GD.Print("Correct answer!");
		}
		else
		{
			GD.Print("Incorrect answer!");
		}

		currentQuestionIndex++;
		LoadExam();
	}
}
