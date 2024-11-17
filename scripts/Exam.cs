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
	private int _totalScore = 0;
	private int _totalMistakes = 0;

	public override void _Ready()
	{
		questionNumber = GetNode<Label>("QuestionContainer/Number");
		question = GetNode<Label>("QuestionContainer/Question");
		picture = GetNode<TextureRect>("Picture");

		for (int i = 0; i < 4; ++i)
		{
			Button optionButton = GetNode<Button>("OptionContainer/Option" + (i + 1));
			options.Add(optionButton);

			optionButton.Connect("pressed", Callable.From(
				() =>
				{

					this.OnOptionSelected(optionButton);
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
			GameState gameState = GameState.GetInstance();
			gameState.TotalScore = _totalScore;
			gameState.TotalMistakes = _totalMistakes;
			
			var scoreScene = (PackedScene)GD.Load("res://scenes/Score.tscn");

			GetTree().ChangeSceneToPacked(scoreScene);
			return;
		}


		// Load the current question
		QuestionModel currentQuestion = exam.Questions[currentQuestionIndex];
		questionNumber.Text = $"Question {currentQuestionIndex + 1}";
		question.Text = currentQuestion.Question;

		picture.Texture = string.IsNullOrEmpty(currentQuestion.ImgPath)
				? null
				: GD.Load<Texture2D>(currentQuestion.ImgPath);

		for (int i = 0; i < options.Count; i++)
		{
			options[i].Text = currentQuestion.Options[i];
		}
	}

	private void OnOptionSelected(Button button)
	{

		QuestionModel currentQuestion = exam.Questions[currentQuestionIndex];
		string selectedOption = button.Text;

		if (selectedOption.Equals(currentQuestion.Answer))
		{
			GD.Print("Correct answer!");
			++_totalScore;
		}
		else
		{
			GD.Print("Incorrect answer!");
			++_totalMistakes;
		}

		currentQuestionIndex++;
		LoadExam();
	}
}
