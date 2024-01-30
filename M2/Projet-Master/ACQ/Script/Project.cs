using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Project
{
    public List<Quiz> quizs = new();

    public Project(List<Quiz> quizs = null)
    {
        if (quizs != null)
            this.quizs = new(quizs);
    }
}

[System.Serializable]
public class Quiz
{
    public string name;
    public string title;
    public List<Question> questions = new();
    public bool useBeginPage = false;
    public string endPageText = "Thank you for your answers !";
    public string beginPageText = "This is a description of the quiz, or whatever you want.";
    public bool canGoBack = false;

    public Quiz()
    {
        name = "Quiz name";
        title = "Quiz description";
    }

    public Quiz(string title, List<Question> questions = null, bool useBeginPage = false, bool canGoBack = false, string endPageText = "Thank you for your answers !")
    {
        name = "Quiz name";
        this.title = title;
        this.useBeginPage = useBeginPage;
        this.endPageText = endPageText;
        if (questions != null)
            this.questions = new(questions);
        this.beginPageText = endPageText;
        this.canGoBack = canGoBack;
    }

    public Quiz(Quiz q)
    {
        this.name = q.name;
        this.title = q.title;
        this.questions = q.questions;
        this.useBeginPage = q.useBeginPage;
        this.beginPageText =q.beginPageText;
        this.endPageText=q.endPageText;
        this.canGoBack=q.canGoBack;
    }

    public bool is_equal(Quiz q)
    {
        return 
            this.name == q.name &&
            this.title == q.title &&
            this.useBeginPage == q.useBeginPage &&
            this.beginPageText == q.beginPageText &&
            this.endPageText == q.endPageText &&
            this.canGoBack == q.canGoBack;
    }
}

[System.Serializable]
public class Question
{
    public int order;
    public string title;
    public string question;
    public QuestionType type;
    public List<string> answersText;
    public List<string> answersImages;
    public string descImage = "";
    public bool is_slider = false;

    public Question()
    {
        order = 1;
        title = "Question " + order;
        question = "How ?";
        type = QuestionType.ImagesSelect;
        answersText = new();
        answersImages = new();
    }

    public Question(int order, string title, string question, QuestionType type = QuestionType.ImagesSelect)
    {
        this.order = order;
        this.title = title;
        this.question = question;
        this.type = type;
        answersText = new();
        answersImages = new();
    }

    public Question(int order)
    {
        this.order = order;
        this.title = "Question title";
        this.question = "Question text";
        this.type = QuestionType.YoN;
        answersText = new();
        answersImages = new();
    }

    public Question(int order, Question q)
    {
        this.order = order;
        this.title = q.title;
        this.question = q.question;
        this.type = q.type;
        this.answersText = new List<string>(q.answersText);
        this.answersImages = new List<string>(q.answersImages);
        this.descImage = q.descImage;
        this.is_slider = q.is_slider;
    }

    public bool is_equal(Question q)
    {
        return
            this.order == q.order &&
            this.title == q.title &&
            this.question == q.question &&
            this.type == q.type &&
            this.descImage == q.descImage &&
            this.is_slider == q.is_slider;
    }
}

//Creer des classe ou structure pour Scale et ImageSelect
public enum QuestionType
{
    ImagesSelect, Scale, YoN, Text
}
