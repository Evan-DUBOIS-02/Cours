using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class JsonReader : MonoBehaviour
{
    //Path to the save directory
    public static string path = Path.Combine("Assets", "ACQ", "Questionnaires");

    /// <summary>
    /// Load all the data from save files
    /// </summary>
    /// <returns>Project object</returns>
    public static Project Load()
    {
        Project project = new();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return project;
        }

        //Find all the saves and load them into the project object
        foreach (string directory in Directory.GetDirectories(path))
        {
            string pathFile = Path.Combine(directory, "quiz.json");

            StreamReader reader = new(pathFile);
            Quiz quiz = new();
            JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), quiz);

            //Change the name of the quiz in function of the dir name if not the same
            string dirName = Path.GetFileName(directory);
            if (quiz.name != dirName)
                quiz.name = dirName;

            project.quizs.Add(quiz);
            reader.Close();

            //TODO : add reading of img directory and verify if exist
        }
        return project;
    }

    /// <summary>
    /// Save the project object into multiple json file to save each quiz data
    /// </summary>
    /// <param name="project"></param>
    public static void Save(Project project)
    {
        foreach (Quiz quiz in project.quizs)
        {
            string pathDir = Path.Combine(path, quiz.name);

            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            StreamWriter writer = new(Path.Combine(pathDir, "quiz.json"));
            writer.Write(JsonUtility.ToJson(quiz, true));
            writer.Close();

            string pathImg = Path.Combine(pathDir, "img");
            if (!Directory.Exists(pathImg))
            {
                Directory.CreateDirectory(pathImg);
            }
        }
    }

    public static bool CheckExistingQuiz(string quiz_name)
    {
        return Directory.Exists(Path.Combine(path, quiz_name));
    }

#if UNITY_EDITOR
    public static void MoveQuiz(string last_name, string current_name)
    {
        FileUtil.MoveFileOrDirectory(Path.Combine(path, last_name), Path.Combine(path, current_name));
    }

    public static void DeleteQuiz(string quiz_name)
    {
        string quiz_path = Path.Combine(path, quiz_name);
        FileUtil.DeleteFileOrDirectory(quiz_path);
        string quiz_path_meta = Path.Combine(path, quiz_name + ".meta");
        FileUtil.DeleteFileOrDirectory(quiz_path_meta);
        AssetDatabase.Refresh();
    }

    public static void ImportQuizFile(string source_path)
    {
        // lecture du fichier
        StreamReader reader = new(source_path);
        Quiz quiz = new();
        JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), quiz);
        // creation du dossier en fonction du nom du quiz + import du fichier
        Directory.CreateDirectory(Path.Combine(path, quiz.name));
        Directory.CreateDirectory(Path.Combine(path, quiz.name, "img"));

        FileUtil.CopyFileOrDirectory(source_path, Path.Combine(path, quiz.name, "quiz.json"));
}

    public static string ImportImg(string source_path, string quizz_name)
    {
        // creation du dossier en fonction du nom du quiz + import du fichier
        string filename = Path.GetFileName(source_path);

        // On vérifie que le dossier img existe
        string pathDir = Path.Combine(path, quizz_name);
        string pathImg = Path.Combine(pathDir, "img");
        if (!Directory.Exists(pathImg))
        {
            Directory.CreateDirectory(pathImg);
        }

        // si l'image n'a pas deja ete importe
        if (!File.Exists(Path.Combine(path, quizz_name, "img", filename))) 
            FileUtil.CopyFileOrDirectory(source_path, Path.Combine(path, quizz_name, "img", filename));
        return filename;
    }
#endif
}