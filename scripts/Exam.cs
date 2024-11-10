using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class Exam : Control
{
	string QUESTIONS_PATH = "res://questions.json";
	public override void _Ready()
	{
		LoadAndPrintJson();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Loadson()
	{
		// Open the file
		FileAccess file = FileAccess.Open(QUESTIONS_PATH, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			GD.PrintErr("Failed to open the JSON file.");
			return;
		}

		// Read and parse the JSON content
		string jsonContent = file.GetAsText();
		file.Close();

		ExamModel exam = JsonConvert.DeserializeObject<ExamModel>(jsonContent);
		
		foreach (var question in exam.Questions)
		{
			GD.Print($"Question: {question.Question}");
			GD.Print($"Answer: {question.Answer}");
			GD.Print($"Options: {string.Join(", ", question.Options)}");
			GD.Print($"Image Path: {question.ImgPath}");
		}
	}
}