using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class ACQWindow : EditorWindow
{
    // le projet contenant tout les quiz
    private static Project project;

    // les differentes interfaces affichables
    private bool showQuizList = true;                   // interface contenant tout les quiz du dossier questionnaire
    private bool showEraseQuizConfirm = false;          // interface de confirmation de supression de quiz
    private bool showCreateQuiz = false;                // interface de creation de quiz
    private bool showEditQuiz = false;                  // interface d'edition de quiz
    private bool showEditQuestion = false;              // interface d'edition de question
    private bool showEraseQuestionConfirm = false;      // interface de confirmation de supression de question
    private bool showNotQuizSaveConfirm = false;        // interface de confirmation de non-sauvegarde d'un quiz
    private bool showNotQuestionSaveConfirm = false;    // interface de confirmation de non-sauvegarde d'une question

    // variables tempons
    private Quiz quizTemp;                              // tempon du quiz courant
    private Question questionTemp;                      // tempon de la question courante
    public List<string> answersImagesTmp = new();       // tempon des images a ajouter a une question
    public List<string> answersTextTmp = new();         // tempon des reponses a ajouter a une question

    // sauvegarde de position
    private int currentQuizId = -1;                     // identifiant du quiz courant dans la liste des quiz du projet
    private int currentQuestionId = -1;                 // identifiant de la question courante dans la liste du quiz courant


    // autres variables
    private bool isNewQuestion = false;                 // variable permetant de savoir si la question courante et une nouvelle question ou une modification d'une ancienne
    static ACQWindow window;                            // variable de la fenetre permettant l'affichage
    private static Vector2 scrollPosition;              // variable permetant d'utiliser les scroll bar
    private static GUIStyle style;                      // variable permettant d'appliquer un style aux elements affiche
    private bool is_quiz_saved = true;                  // variable permettant de savoir si le quiz courant est sauvegarde ou non
    private bool is_question_saved = true;              // variable permettant de savoir si la question courante est sauvegardee ou non
    private bool show_save_error = false;               // variable permettant de savoir si une erreur doit etre affiche ou non (utilise lorsque l'on veut creer un quiz dont un autre du meme nom existe deja)
    int range_min;                                      // variable sauvegardant le minimum pour une question de type range
    int range_max;                                      // variable sauvegardant le maximum pour une question de type range
    string slider_begin_text = "";                      // dans le cas d'un slider, le text a gauche du slider
    string slider_middle_text = "";                     // dans le cas d'un slider, le text au milieu du slider
    string slider_end_text = "";                        // dans le cas d'un slider, le text a droite du slider

    // Fonction permettant l'affichage de la fenetre
    [MenuItem("ACQ/Editor")]
    public static void ShowWindow()
    {
        LoadProject();
        window = GetWindow<ACQWindow>("ACQ");
        window.Show();
    }

    // boucle principale de refresh (appele a chaque frame)
    void OnGUI()
    {
        // si on est sur la toute premiere frame, le projet n'existe pas
        if (project == null)
            // dans ce cas, on cre un projet et on load toute les formations possiblse contenu dans le dossier questionnaire
            LoadProject();

        // on charge un style par defaut
        style = new(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        // suivant la fenetre a afficher, on appelle la bonne fonction
        if (showQuizList)
        {
            ShowQuizList();
        }
        else if (showEraseQuizConfirm)
        {
            ShowEraseQuizConfirm();
        }
        else if (showCreateQuiz)
        {
            ShowCreateQuiz();
        }
        else if (showEditQuiz)
        {
            ShowEditQuiz();
        }
        else if (showEditQuestion)
        {
            ShowEditQuestion();
        }
        else if (showEraseQuestionConfirm)
        {
            ShowEraseQuestionConfirm();
        }
        else if (showNotQuizSaveConfirm)
        {
            ShowNotQuizSaveConfirm();
        }
        else if (showNotQuestionSaveConfirm)
        {
            ShowNotQuestionSaveConfirm();
        }
        else // erreur, on revient a l'affichage principale
        {
            showQuizList = true;
            showEraseQuizConfirm = false;
            showCreateQuiz = false;
            showEditQuiz = false;
            showEditQuestion = false;
            showEraseQuestionConfirm = false;
            showNotQuizSaveConfirm = false;
            showNotQuestionSaveConfirm = false;
        }
    }

    // fonction permettant de charger le projet
    private static void LoadProject()
    {
        project = JsonReader.Load();
    }

    // affichage de la liste des quiz presents dans le dossier questionnaires
    private void ShowQuizList()
    {
        // si la liste est vide
        if(project.quizs.Count == 0)
        {
            // bouton permettant de charger a nouveau le projet (utile si import manuel de quiz, cf manuel utilisateur)
            if (GUILayout.Button(new GUIContent(" Reload data", EditorGUIUtility.IconContent("Refresh").image)))
            {
                // on rafraichi le contenu des dossiers unity
                AssetDatabase.Refresh();
                // on recharge le projet
                LoadProject();
            }
            // message d'information
            GUILayout.Label("There is no quiz \n Create, Import one or try to Reload Data", style);
            GUILayout.BeginHorizontal();
            // bouton permettant de creer un nouveau quiz
            if (GUILayout.Button(new GUIContent(" Create Quiz", EditorGUIUtility.IconContent("Toolbar Plus").image)))
            {
                // on quitte l'interface actuelle
                showQuizList = false;
                // on passe a l'affichage de creation d'un quiz
                showCreateQuiz = true;
                // on instancie un nouveau tempon de quiz
                quizTemp = new Quiz();
                // on sais qu'il sera ajoute au bout de la list des quiz du projet
                currentQuizId = project.quizs.Count;
            }
            // bouton permettant d'importer un quiz
            if (GUILayout.Button(new GUIContent(" Import Quiz", EditorGUIUtility.IconContent("Download-Available").image)))
            {
                // on ouvre un navigateur et on recupere le fichier demande
                string path = EditorUtility.OpenFilePanel("Load new quiz", "", "json");
                // on import le quiz
                JsonReader.ImportQuizFile(path);
                // on rafraichi le contenu des dossiers unity
                AssetDatabase.Refresh();
                // on recharge le projet
                LoadProject();
            }
            GUILayout.EndHorizontal();
        }
        // si la liste n'est pas vide
        else
        {
            // Identique a juste au dessus {
            if (GUILayout.Button(new GUIContent(" Reload data", EditorGUIUtility.IconContent("Refresh").image)))
            {
                AssetDatabase.Refresh();
                LoadProject();
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent(" Create Quiz", EditorGUIUtility.IconContent("Toolbar Plus").image)))
            {
                showQuizList = false;
                showCreateQuiz = true;
                quizTemp = new Quiz();
                currentQuizId = project.quizs.Count;
            }
            if (GUILayout.Button(new GUIContent(" Import Quiz", EditorGUIUtility.IconContent("Download-Available").image)))
            {
                string path = EditorUtility.OpenFilePanel("Load new quiz", "", "json");
                JsonReader.ImportQuizFile(path);
                AssetDatabase.Refresh();
                LoadProject();
            }
            GUILayout.EndHorizontal();
            // }

            // message d'information
            GUILayout.Label("List of quiz:", style);
            // on place le debut d'une scroll bar
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            // pour chaque quiz du projet
            foreach (Quiz quiz in project.quizs)
            {
                GUILayout.BeginHorizontal();
                // on affiche son nom
                GUILayout.Label(quiz.name, style, GUILayout.Width(position.width * 0.20f));
                // on peut le modifier,
                if (GUILayout.Button(new GUIContent(" Edit", EditorGUIUtility.IconContent("d_SceneViewTools").image), GUILayout.Width(position.width * 0.25f)))
                {
                    // on creer une copie du questionnaire dans le tempon
                    quizTemp = new Quiz(quiz);
                    // on recupere son index dans la list
                    currentQuizId = project.quizs.IndexOf(quiz);
                    // on change d'affichage
                    showQuizList = false;
                    showEditQuiz = true;
                }
                Texture2D customIconExport = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/export.png");
                if (GUILayout.Button(new GUIContent(" Export", customIconExport), GUILayout.Width(position.width * 0.25f), GUILayout.Height(20)))
                {
                    QuizCreator.Create(quiz);
                }
                Texture2D customIconErase = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/delete_folder.png");
                if (GUILayout.Button(new GUIContent(" Erase", customIconErase), GUILayout.Width(position.width * 0.25f), GUILayout.Height(20)))
                {
                    // on change d'affichage
                    showEraseQuizConfirm = true;
                    showQuizList = false;
                    // on sauvegarde un pointeur sur le quiz dans le tempon (pour l'avoir sur l'autre interface)
                    quizTemp = quiz;
                }
                GUILayout.EndHorizontal();
            }
            // on place la fin de la scroll bar
            EditorGUILayout.EndScrollView();
        }
    }

    // affichage de confirmation de supression du quiz
    private void ShowEraseQuizConfirm()
    {
        // message d'information
        GUILayout.Label("Are you sure you want to erase \"" + quizTemp.name + "\" ?", style);
        GUILayout.BeginHorizontal();
        // on annule la manipulation
        if (GUILayout.Button(new GUIContent(" Cancel", EditorGUIUtility.IconContent("d_winbtn_win_close").image)))
        {
            // on revient a l'interface precedente
            showEraseQuizConfirm = false;
            showQuizList = true;
            // on recharge le projet
            LoadProject();
            // on enleve le pointeur
            quizTemp = null;
        }
        // on confirm la supression
        else if (GUILayout.Button(new GUIContent(" Confirm", EditorGUIUtility.IconContent("valid").image)))
        {
            // on suprimme le quiz
            JsonReader.DeleteQuiz(quizTemp.name);
            // on change d'interface
            showEraseQuizConfirm = false;
            showQuizList = true;
            // on recharge le projet
            LoadProject();
            // on enleve le pointeur
            quizTemp = null;
        }
        GUILayout.EndHorizontal();
    }

    // affichage de l'interface de creation d'un quiz
    private void ShowCreateQuiz()
    {
        // bouton pour revenir a l'affichage precedent
        if (GUILayout.Button(new GUIContent(" Back", EditorGUIUtility.IconContent("back").image)))
        {
            // changement d'affichage
            showCreateQuiz = false;
            showQuizList = true;
            // on recharge le projet
            LoadProject();
            // on sort de la fonction
            return;
        }

        // message d'information
        GUILayout.Label("Quiz informations:", style);

        // le nom du quiz + champ d'entree
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.name = GUILayout.TextField(quizTemp.name, 25, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // la description du quiz + champ d'entree
        GUILayout.BeginHorizontal();
        GUILayout.Label("Description:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.title = GUILayout.TextField(quizTemp.title, 25, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Begin page text:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.useBeginPage = GUILayout.Toggle(quizTemp.useBeginPage, "", GUILayout.Width(position.width * 0.04f));
        // si on veut une page de garde dans le quiz
        if (quizTemp.useBeginPage)
        {
            // on ajoute un text
            quizTemp.beginPageText = GUILayout.TextField(quizTemp.beginPageText, 100, GUILayout.Width(position.width * 0.605f));
        }
        GUILayout.EndHorizontal();

        // la page de fin du quiz + champ d'antree
        GUILayout.BeginHorizontal();
        GUILayout.Label("End page text:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.endPageText = GUILayout.TextField(quizTemp.endPageText, 100, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // permet de dire si on peut revenir en arriere durant le quiz ou non
        GUILayout.BeginHorizontal();
        GUILayout.Label("Can go back :", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.canGoBack = GUILayout.Toggle(quizTemp.canGoBack, "", GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // affichage d'une erreur
        if(show_save_error)
        {
            GUILayout.Label("Cannot save! Quiz with a same name already exist.", style);
        }

        // bouton permettant la creation du quiz
        if (GUILayout.Button(new GUIContent(" Create", EditorGUIUtility.IconContent("valid").image)))
        {
            // on verifie qu'un quiz de meme nom n'existe pas deja, dans ce cas, erreur
            if(JsonReader.CheckExistingQuiz(quizTemp.name))
            {
                show_save_error = true;
            }
            // sinon
            else
            {
                // on ajoute une copie du quiz tempon dans la liste des quiz du projet
                project.quizs.Add(new Quiz(quizTemp));
                // on sauvegarde le projet
                JsonReader.Save(project);
                AssetDatabase.Refresh();
                // on passe en mode edition du quiz
                showCreateQuiz = false;
                showEditQuiz = true;
                show_save_error = false;
            }
        }
    }

    // affichage de l'interfae d'edition d'un quiz
    private void ShowEditQuiz()
    {
        GUILayout.BeginHorizontal();
        // bouton pour revenir un arriere
        if (GUILayout.Button(new GUIContent(" Back", EditorGUIUtility.IconContent("back").image)))
        {
            GUILayout.EndHorizontal();
            // si on a pas sauvegarde
            if (!is_quiz_saved || !quizTemp.is_equal(project.quizs[currentQuizId]))
            {
                // on passe a l'interface de confirmation de non-sauvegarde
                showEditQuiz = false;
                showNotQuizSaveConfirm = true;
                return;
            } 
            // sinon
            else
            {
                // on revient en arriere
                questionTemp = null;
                showEditQuiz = false;
                showQuizList = true;
                LoadProject();
                return;
            }
            
        }
        // si on sauvegarde
        if (GUILayout.Button(new GUIContent("Save Change", EditorGUIUtility.IconContent("SaveAs").image)))
        {
            // le quiz est considere comme sauvegarde
            is_quiz_saved = true;
            // si on a renome le quiz
            if (project.quizs[currentQuizId].name != quizTemp.name)
            {
                // on le deplace
                JsonReader.MoveQuiz(project.quizs[currentQuizId].name, quizTemp.name);
                // on suprimme l'ancien
                JsonReader.DeleteQuiz(project.quizs[currentQuizId].name);
            }
            // on ecrase l'ancien quiz par le tempon
            project.quizs[currentQuizId] = quizTemp;
            // on sauvegarde le projet
            JsonReader.Save(project);
            AssetDatabase.Refresh();
            // on recreer un nouveau tempon a partir de la sauvegarde
            quizTemp = new Quiz(project.quizs[currentQuizId]);
        }
        GUILayout.EndHorizontal();
        // message d'information
        GUILayout.Label("Quiz informations:", style);
        // comme au moment de la creation {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.name = GUILayout.TextField(quizTemp.name, 25, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Title/Theme:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.title = GUILayout.TextField(quizTemp.title, 100, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Begin page text:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.useBeginPage = GUILayout.Toggle(quizTemp.useBeginPage, "", GUILayout.Width(position.width * 0.04f));
        if (quizTemp.useBeginPage)
        {
            quizTemp.beginPageText = GUILayout.TextField(quizTemp.beginPageText, 100, GUILayout.Width(position.width * 0.605f));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("End page text:", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.endPageText = GUILayout.TextField(quizTemp.endPageText, 100, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Can go back during test", style, GUILayout.Width(position.width * 0.30f));
        quizTemp.canGoBack = GUILayout.Toggle(quizTemp.canGoBack, "", GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();
        // }

        // si le quiz n'a pas encore de question
        if (quizTemp.questions.Count == 0)
        {
            // message d'information
            GUILayout.Label("There is no question in the quiz.", style);
            GUILayout.Label("Would you want to create one ?", style);
            // bouton pour ajouter une question
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); // on place une scroll bar fictive pour fixer le bouton en bas de page
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button(new GUIContent(" Add Question", EditorGUIUtility.IconContent("Toolbar Plus").image)))
            {
                // on change d'affichage
                showEditQuestion = true;
                showEditQuiz = false;
                // le quiz n'est plus sauvegarde
                is_quiz_saved = false;
                // c'est une nouvelle question et non une modification
                isNewQuestion = true;
                // on cree un tempon
                // ici la question sera automatiquement place en bout de file
                questionTemp = new Question(quizTemp.questions.Count);
                // on place les valeur de range de 0 a 5 automatiquement (pour les questions de type scale)
                range_min = 0;
                range_max = 5;
                // on met des tempon neuf dans le cas des autres type de question
                answersImagesTmp = new();
                answersTextTmp = new();
                // on vide les texts des slider
                slider_begin_text = "";
                slider_middle_text = "";
                slider_end_text = "";
            }
        }
        // si le quiz a deja des questions
        else
        {
            // variables permettant la manipulation des questions
            bool duplicate = false;     // permet de sauvegarde une demande de duplication
            bool switch_up = false;     // permet de sauvegarde une demande de changement d'ordre (la question passe au dessus)
            bool switch_down = false;   // permet de sauvegarde une demande de changement d'ordre (la question passe en dessous)
            // => nous sommes oblige de sauvegarder ces demande car nous ne pouvons pas modifier la liste des questions durant son parcours, Cf. parcours de la liste en dessous
            int switch_origin = -1;     // permet de sauvegarde la question dont on veut changer l'ordre
            GUILayout.Label("List of questions:", style);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            // pour chaque question de la liste des questions du quiz
            foreach (Question question in quizTemp.questions)
            {

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(position.width * 0.30f));
                // on affiche le nom de la question
                GUILayout.Label(question.title, style);
                GUILayout.EndVertical();
                Texture2D customIconEdit = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/edit.png");
                if (GUILayout.Button(new GUIContent("", customIconEdit), GUILayout.Width(position.width * 0.15f), GUILayout.Height(20)))
                {
                    // on vide les tempons
                    answersImagesTmp = new();
                    answersTextTmp = new();
                    range_min = 0;
                    range_max = 5;
                    slider_begin_text = "";
                    slider_middle_text = "";
                    slider_end_text = "";
                    // on va potentiellement modifier une question, donc le quiz n'est plus considere comme sauvegarde
                    is_quiz_saved = false;
                    // on change d'interface
                    showEditQuestion = true;
                    showEditQuiz = false;
                    // on recupere l'identifiant/l'ordre de la question courante
                    currentQuestionId = quizTemp.questions.IndexOf(question);
                    // ce n'est pas une nouvelle question
                    isNewQuestion = false;
                    // on cree un copie de la question dans notre tempon
                    questionTemp = new Question(quizTemp.questions.IndexOf(question), question);
                    // pre-remplissage des champs s'ils existent, suivant le type
                    // type scale
                    if (questionTemp.type == QuestionType.Scale && questionTemp.answersText.Count > 0)
                    {
                        range_min = int.Parse(questionTemp.answersText[0]);
                        
                        if (questionTemp.is_slider)
                        {
                            range_max = int.Parse(questionTemp.answersText[1]);
                            slider_begin_text = questionTemp.answersText[2];
                            slider_middle_text = questionTemp.answersText[3];
                            slider_end_text = questionTemp.answersText[4];
                        } else
                        {
                            range_max = int.Parse(questionTemp.answersText[questionTemp.answersText.Count - 1]);
                        }
                    }
                    // type image
                    else if(questionTemp.type == QuestionType.ImagesSelect && questionTemp.answersImages.Count > 0)
                    {
                        foreach(string img in questionTemp.answersImages)
                        {
                            answersImagesTmp.Add(img);
                        }
                        foreach(string text in questionTemp.answersText)
                        {
                            answersTextTmp.Add(text);
                        }
                    }
                    // type text
                    else if(questionTemp.type == QuestionType.Text && questionTemp.answersText.Count > 0)
                    {
                        foreach(string answer in questionTemp.answersText)
                        {
                            answersTextTmp.Add(answer);
                        }
                    }
                }
                Texture2D customIconDuplicate = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/duplicate.png");
                if (GUILayout.Button(new GUIContent("", customIconDuplicate), GUILayout.Width(position.width * 0.15f), GUILayout.Height(20)))
                {
                    // on modifie le quiz
                    is_quiz_saved = false;
                    // on cree une copie de la question
                    questionTemp = new Question(quizTemp.questions.Count, question);
                    // on garde le demande de duplication
                    duplicate = true;
                }

                Texture2D customIconUp = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/up.png");
                Texture2D customIconDown = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/down.png");
                GUI.enabled = quizTemp.questions.IndexOf(question) > 0;
                if (GUILayout.Button(new GUIContent("", customIconUp), GUILayout.Width(position.width * 0.09f), GUILayout.Height(20)))
                {
                    is_quiz_saved = false;
                    switch_up = true;
                    switch_origin = quizTemp.questions.IndexOf(question);
                }
                GUI.enabled = quizTemp.questions.IndexOf(question) < quizTemp.questions.Count - 1;
                if (GUILayout.Button(new GUIContent("", customIconDown), GUILayout.Width(position.width * 0.09f), GUILayout.Height(20)))
                {
                    is_quiz_saved = false;
                    switch_down = true;
                    switch_origin = quizTemp.questions.IndexOf(question);
                }
                GUI.enabled = true;

                Texture2D customIconDelete = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/delete.png");
                if (GUILayout.Button(new GUIContent("", customIconDelete), GUILayout.Width(position.width * 0.15f), GUILayout.Height(20)))
                {
                    questionTemp = question;
                    currentQuestionId = quizTemp.questions.IndexOf(question);
                    showEditQuiz = false;
                    showEraseQuestionConfirm = true;
                }

                GUILayout.EndHorizontal();
            }
            // si durant le parcours de la liste il y'a eu une demande de:
            // duplication
            if (duplicate)
            {
                // on ajoute la question copier precedement dans la liste des questions
                quizTemp.questions.Add(questionTemp);
                // on vide le tempon
                questionTemp = null;
                // on retir la demande
                duplicate = false;
            }
            // changement d'ordre (remonter)
            if (switch_up)
            {
                // on recupere une copie de la question du dessus et on change sont ordre avec "celle qui demande a monter"
                Question q = new Question(switch_origin, quizTemp.questions[switch_origin - 1]);
                // on place la question "qui demande a monter" au dessus dans la liste
                quizTemp.questions[switch_origin - 1] = quizTemp.questions[switch_origin];
                // on change la valeur de l'ordre de cette question
                quizTemp.questions[switch_origin - 1].order -= 1;
                // on place la question precedement copier a la place de "celle qui demande a monter"
                quizTemp.questions[switch_origin] = q;
                // on retir la demande
                switch_up = false;
            }
            // changement d'ordre (descendre)
            if (switch_down)
            {
                // meme principe que au dessus
                Question q = new Question(switch_origin, quizTemp.questions[switch_origin + 1]);
                quizTemp.questions[switch_origin + 1] = quizTemp.questions[switch_origin];
                quizTemp.questions[switch_origin + 1].order += 1;
                quizTemp.questions[switch_origin] = q;
                switch_down = false;
            }
            EditorGUILayout.EndScrollView();
            // bouton pour ajouter une question, CF. debut de la fonction
            if (GUILayout.Button(new GUIContent(" Add Question", EditorGUIUtility.IconContent("Toolbar Plus").image)))
            {
                is_quiz_saved = false;
                showEditQuestion = true;
                showEditQuiz = false;
                isNewQuestion = true;
                questionTemp = new Question(quizTemp.questions.Count);
                range_min = 0;
                range_max = 5;
                answersImagesTmp = new();
                answersTextTmp = new();
                slider_begin_text = "";
                slider_middle_text = "";
                slider_end_text = "";
            }
        }
    }
    
    // affichage de l'interface d'edition d'une question
    private void ShowEditQuestion()
    {
        GUILayout.BeginHorizontal();
        // retour en arriere, meme principe que pour quiz au niveau de la sauvegarde
        if (GUILayout.Button(new GUIContent(" Back", EditorGUIUtility.IconContent("back").image)))
        {
            if(!is_question_saved || !questionTemp.is_equal(quizTemp.questions[currentQuestionId]))
            {
                GUILayout.EndHorizontal();
                showEditQuestion = false;
                showNotQuestionSaveConfirm = true;
                return;
            } 
            else
            {
                questionTemp = null;
                GUILayout.EndHorizontal();
                showEditQuestion = false;
                showEditQuiz = true;
                return;
            }
            
        }

        // bouton pour sauvegarder la question
        if (GUILayout.Button(new GUIContent(" Save Change", EditorGUIUtility.IconContent("SaveAs").image)))
        {
            // la question est considere comme sauvegardee
            is_question_saved = true;
            GUILayout.EndHorizontal();

            /* 
            *       cette premiere partie prepare les reponses possibles a la question
            */

            // on vide les reponses
            questionTemp.answersText = new();
            questionTemp.answersImages = new();
            // s'il y'a une image de description
            if (questionTemp.descImage != "")
            {
                // on l'import dans le dossier img
                string filename = JsonReader.ImportImg(questionTemp.descImage, quizTemp.name);
                // on l'ajoute dans la question avec le nouveau chemin d'acces
                questionTemp.descImage = filename;
                AssetDatabase.Refresh();
            }
            // suivant le type de la question
            switch (questionTemp.type)
            {
                // si c'est une selection d'image
                case QuestionType.ImagesSelect:
                    // pour chaque image
                    foreach (string img in answersImagesTmp)
                    {
                        // on l'import dans le projet
                        string filename = JsonReader.ImportImg(img, quizTemp.name);
                        // on l'ajoute dans la question avec le nouveau chemin d'acces
                        questionTemp.answersImages.Add(filename);
                        AssetDatabase.Refresh();
                    }
                    // pour chaque text lie a l'image
                    foreach (string answer in answersTextTmp)
                    {
                        // on ajoute le text a la question
                        questionTemp.answersText.Add(answer);
                    }
                    break;
                // si c'est un scale
                case QuestionType.Scale:
                    // si les donnees en entree ne sont pas possibles, erreur
                    if(range_min > range_max)
                    {
                        Debug.LogError("Min value need to be under the max value.");
                        return;
                    }
                    // si c'est un format de slider
                    if(questionTemp.is_slider)
                    {
                        // on ajoute simplement:
                        // le minimum
                        questionTemp.answersText.Add(range_min.ToString());
                        // le maximum
                        questionTemp.answersText.Add(range_max.ToString());
                        // les text de debut, milieu et fin
                        questionTemp.answersText.Add(slider_begin_text);
                        questionTemp.answersText.Add(slider_middle_text);
                        questionTemp.answersText.Add(slider_end_text);
                    } 
                    // si c'est au format radio bouton
                    else
                    {
                        // pour chaque valeur entiere entre le minimum et le maximum
                        for (int i = range_min; i <= range_max; i++)
                        {
                            // on ajoute la reponse a la question
                            questionTemp.answersText.Add(i.ToString());
                            // si on depasse les limite on annule la sauvegarde avec une erreur
                            if (questionTemp.answersText.Count > 10)
                            {
                                Debug.LogError("Radio button limit size = 10. Please select a range values that contain at max 10 values.");
                                return;
                            }
                        }
                    }
                    break;
                // si c'est un type text
                case QuestionType.Text:
                    // on ajoute les reponses possible dans la liste
                    foreach(string answer in answersTextTmp)
                    {
                        questionTemp.answersText.Add(answer);
                    }
                    break;
                // sinon, on ne fait rien (meme si type YoN)
                default:
                    break;
            }

            /* 
            *       cette seconde partie sauvegarde reellement la question
            */

            // si il s'agit d'une nouvelle question a ajouter
            if (isNewQuestion)
            {
                // si on place la nouvelle question a un autre indice que en bout de file
                if (questionTemp.order < quizTemp.questions.Count)
                {
                    // on recupere la question a la place occupee par copie en chengeant l'ordre
                    Question q = new Question(quizTemp.questions.Count, quizTemp.questions[questionTemp.order]);
                    // on place la nouvelle question a l'indice voulu
                    quizTemp.questions[questionTemp.order] = questionTemp;
                    // on place l'ancienne question en bout de file
                    quizTemp.questions.Add(q);
                }
                else // si elle va bien en bout de file
                {
                    // on l'ajoute simplement
                    quizTemp.questions.Add(questionTemp);
                }
                // ce n'est plus une nouvelle question
                isNewQuestion = false;
            }
            // s'il s'agit d'une modification de question et si on a change l'ordre
            else if (questionTemp.order != currentQuestionId)
            {
                // on recupere la question a la place occupee par copie en chengeant l'ordre
                Question q = new Question(currentQuestionId, quizTemp.questions[questionTemp.order]);
                // on place la nouvelle question a l'indice voulu
                quizTemp.questions[questionTemp.order] = questionTemp;
                // on place l'ancienne question a la place de la question courante
                quizTemp.questions[currentQuestionId] = q;
                currentQuestionId = -1;
            }
            // dans les autres cas, on sauvegarde simplement
            else
            {
                quizTemp.questions[questionTemp.order] = questionTemp;
            }
            // on change d'inerface
            showEditQuestion = false;
            showEditQuiz = true;
            return;
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Question informations", style);

        // chanmp pour changer l'ordre de la question
        GUILayout.BeginHorizontal();
        GUILayout.Label("Order:", style, GUILayout.Width(position.width * 0.30f));
        questionTemp.order = EditorGUILayout.IntField(questionTemp.order, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // titre de la question (entete du fichier csv)
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:", style, GUILayout.Width(position.width * 0.30f));
        questionTemp.title = GUILayout.TextField(questionTemp.title, 25, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // text principale de la question
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Question:", style, GUILayout.Width(position.width * 0.30f));
        questionTemp.question = GUILayout.TextArea(questionTemp.question, 500, GUILayout.Width(position.width * 0.65f), GUILayout.Height(40));
        GUILayout.EndHorizontal();

        // image de description de la question
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image description: ", style, GUILayout.Width(position.width * 0.30f));
        // s'il n'y a pas d'image de description
        if (questionTemp.descImage == "")
            // on ajoute un bouton pour pouvoir en ajouter une
            if (GUILayout.Button(new GUIContent(" Add image", EditorGUIUtility.IconContent("Toolbar Plus").image), GUILayout.Width(position.width * 0.20f)))
            {
                // on recupere le fichier
                string path = EditorUtility.OpenFilePanel("Load new image", "", "png,jpg,svg");
                if (path != "")
                    questionTemp.descImage = path;
            }
        // s'il y'a deja une image
        if (questionTemp.descImage != "")
            // on ajoute un bouton pour la suprimmer
            if (GUILayout.Button(new GUIContent("Remove", EditorGUIUtility.IconContent("d_Toolbar Minus").image), GUILayout.Width(position.width * 0.20f)))
                questionTemp.descImage = "";
        // on affiche ne nom du fichier
        GUILayout.Label(questionTemp.descImage, GUILayout.Width(position.width * 0.545f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("");
        GUILayout.EndHorizontal();

        // le type de la question
        GUILayout.BeginHorizontal();
        GUILayout.Label("Question Type:", style, GUILayout.Width(position.width * 0.30f));
        questionTemp.type = (QuestionType)EditorGUILayout.EnumPopup(questionTemp.type, GUILayout.Width(position.width * 0.65f));
        GUILayout.EndHorizontal();

        // suivant le type de la question, affiche differents elements
        switch (questionTemp.type)
        {
            // si c'est une selection d'image
            case QuestionType.ImagesSelect:
                GUILayout.BeginHorizontal();
                GUILayout.Label("", style, GUILayout.Width(position.width * 0.60f));
                // si la la limite du nombre d'image n'est pas atteinte
                if (answersImagesTmp.Count < 6)
                {
                    // on ajoute un bouton pour en ajouter une
                    if (GUILayout.Button(new GUIContent(" Image File", EditorGUIUtility.IconContent("Toolbar Plus").image), GUILayout.Width(position.width * 0.35f)))
                    {
                        // la question n'est pas sauvegarde
                        is_question_saved = false;
                        // on recupere le fichier
                        string path = EditorUtility.OpenFilePanel("Load new img", "", "png,jpg,svg");
                        if (path != "")
                        {
                            // on l'ajoute l'image dans le tempon des reponses
                            answersImagesTmp.Add(path);
                            // on ajoute un text vide lie a l'image dans le tempon des reponses
                            answersTextTmp.Add(" ");
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", style, GUILayout.Width(position.width * 0.60f));
                    if (GUILayout.Button(new GUIContent(" Image Folder", EditorGUIUtility.IconContent("Toolbar Plus").image), GUILayout.Width(position.width * 0.35f)))
                    {
                        is_question_saved = false;
                        string path = EditorUtility.OpenFolderPanel("Load folder of img", "", "");
                        if(Directory.Exists(path))
                        {
                            foreach (var img in Directory.GetFiles(path))
                            {
                                if (answersImagesTmp.Count >= 6)
                                {
                                    break;
                                }
                                string extension = Path.GetExtension(img).ToLower();
                                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                                if (Array.IndexOf(imageExtensions, extension) != -1)
                                {
                                    answersImagesTmp.Add(img);
                                    answersTextTmp.Add(" ");
                                }
                            }
                        }
                    }
                }
                // si on a atteint la limite, on retir le bouton
                else
                {
                    GUILayout.Label("6 images max.", style);
                }

                GUILayout.EndHorizontal();
                // variable pour sauvegarder une demande de supression d'image
                bool remove_img = false;
                // l'image a suprimmer dans ce cas
                string img_to_remove = "";
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // variable pour gestion des switch
                int switch_origin = -1;
                bool switch_up = false; 
                bool switch_down = false;
                // pour chaque image dans la liste des reponses tempon
                for (int i = 0; i < answersImagesTmp.Count; i++)
                {
                    // on recupere le path de l'image
                    string img = answersImagesTmp[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", style, GUILayout.Width(position.width * 0.005f));
                    // switch des images
                    Texture2D customIconUp = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/up.png");
                    Texture2D customIconDown = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/down.png");
                    GUI.enabled = i > 0;
                    if (GUILayout.Button(new GUIContent("", customIconUp), GUILayout.Width(position.width * 0.1f), GUILayout.Height(20)))
                    {
                        is_question_saved = false;
                        switch_up = true;
                        switch_origin = i;
                    }
                    GUI.enabled = i < answersImagesTmp.Count - 1;
                    if (GUILayout.Button(new GUIContent("", customIconDown), GUILayout.Width(position.width * 0.1f), GUILayout.Height(20)))
                    {
                        is_quiz_saved = false;
                        switch_down = true;
                        switch_origin = i;
                    }
                    GUI.enabled = true;
                    // on affiche le nom/path
                    GUILayout.Label(img, style, GUILayout.Width(position.width * 0.58f));
                    // bouton pour suprimmer l'image
                    Texture2D customIconDelete = EditorGUIUtility.FindTexture("Assets/ACQ/Editor/Resources/Icons/delete.png");
                    if (GUILayout.Button(new GUIContent(" Remove", customIconDelete), GUILayout.Width(position.width * 0.15f), GUILayout.Height(20)))
                    {
                        // dans ce cas, la question n'est plus sauvegardee
                        is_question_saved = false;
                        // on enregistre la demande de supression
                        remove_img = true;
                        // on enregistre l'image a suprimmer
                        img_to_remove = img;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    // on ajoute un champ pour le text lie a l'image
                    answersTextTmp[i] = GUILayout.TextField(answersTextTmp[i], 25, GUILayout.Width(position.width * 0.735f));
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
                // si on a une demande de supression
                if (remove_img)
                {
                    // on suprimme l'image du tempon
                    answersImagesTmp.Remove(img_to_remove);
                    // on suprimme la demande
                    remove_img = false;
                    // on suprimme l'image a suprimmer
                    img_to_remove = "";
                }
                // changement d'ordre (remonter)
                if (switch_up)
                {
                    string img = answersImagesTmp[switch_origin];
                    string img_text = answersTextTmp[switch_origin];
                    string img_up = answersImagesTmp[switch_origin - 1];
                    string img_up_text = answersTextTmp[switch_origin - 1];

                    answersImagesTmp[switch_origin] = img_up;
                    answersTextTmp[switch_origin] = img_up_text;
                    answersImagesTmp[switch_origin - 1] = img;
                    answersTextTmp[switch_origin-1] = img_text;
                 
                    // on retir la demande
                    switch_up = false;
                }
                // changement d'ordre (descendre)
                if (switch_down)
                {
                    // meme principe que au dessus
                    string img = answersImagesTmp[switch_origin];
                    string img_text = answersTextTmp[switch_origin];
                    string img_up = answersImagesTmp[switch_origin + 1];
                    string img_up_text = answersTextTmp[switch_origin + 1];

                    answersImagesTmp[switch_origin] = img_up;
                    answersTextTmp[switch_origin] = img_up_text;
                    answersImagesTmp[switch_origin + 1] = img;
                    answersTextTmp[switch_origin + 1] = img_text;

                    switch_down = false;
                }
                break;
            // si la question est de type scale
            case QuestionType.Scale:
                GUILayout.BeginHorizontal();
                GUILayout.Label("");
                GUILayout.EndHorizontal();
                // champ d'entree pour le minimum et le maximum & slider mode
                GUILayout.BeginHorizontal();
                //GUILayout.Label("", style, GUILayout.Width(position.width * 0.1f));
                GUILayout.Label("Slider mode", style, GUILayout.Width(position.width * 0.30f));
                questionTemp.is_slider = GUILayout.Toggle(questionTemp.is_slider, "", GUILayout.Width(position.width * 0.10f));

                GUILayout.Label("from ", style, GUILayout.Width(position.width * 0.10f));
                range_min = EditorGUILayout.IntField("", range_min, GUILayout.Width(position.width * 0.1f));
                GUILayout.Label("to ", style, GUILayout.Width(position.width * 0.10f));
                range_max = EditorGUILayout.IntField("", range_max, GUILayout.Width(position.width * 0.1f));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Label("");
                GUILayout.EndHorizontal();

                // si la case slider est active
                if (questionTemp.is_slider)
                {
                    // on ajoute 3 champ poour les texts de debut, milieu et fin de slider
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", style, GUILayout.Width(position.width * 0.04f));
                    GUILayout.Label("Begin text", style, GUILayout.Width(position.width * 0.30f));
                    GUILayout.Label("Middle text", style, GUILayout.Width(position.width * 0.30f));
                    GUILayout.Label("End text", style, GUILayout.Width(position.width * 0.30f));
                    
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", style, GUILayout.Width(position.width * 0.04f));
                    slider_begin_text = EditorGUILayout.TextField("", slider_begin_text, GUILayout.Width(position.width * 0.30f));
                    slider_middle_text = EditorGUILayout.TextField("", slider_middle_text, GUILayout.Width(position.width * 0.30f));
                    slider_end_text = EditorGUILayout.TextField("", slider_end_text, GUILayout.Width(position.width * 0.30f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    
                    
                    GUILayout.EndHorizontal();
                }
                break;
            // si la question est de type text
            case QuestionType.Text:
                GUILayout.BeginHorizontal();
                GUILayout.Label("", style, GUILayout.Width(position.width * 0.65f));
                // si la limite de reponses possible n'est pas atteinte
                if (answersTextTmp.Count < 10) { 
                    // on place un bouton pour ajouter une reponse
                    if (GUILayout.Button(new GUIContent(" Add answer", EditorGUIUtility.IconContent("Toolbar Plus").image), GUILayout.Width(position.width * 0.30f)))
                    {
                        is_question_saved = false;
                        // on ajoute une reponse dans le tempon des reponses
                        answersTextTmp.Add("new answer");
                    }
                }
                // sinon, on retir le bouton d'ajout et on afficge un message
                else
                {
                    GUILayout.Label("10 answers max.", style, GUILayout.Width(position.width * 0.30f));
                }
                GUILayout.EndHorizontal();
                // permet d'enregistrer une demande de supression
                bool remove_answer = false;
                // permet d'enregistrer la reponse a suprimmer
                string answer_to_remove = "";
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // pour chaque reponse dans le tempon
                for (int i=0; i< answersTextTmp.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(position.width * 0.045f));
                    // on ajoure un champ d'entree
                    answersTextTmp[i] = GUILayout.TextField(answersTextTmp[i], 25, GUILayout.Width(position.width * 0.6f));
                    // un bouton pour suprimmer
                    if (GUILayout.Button(new GUIContent("Remove", EditorGUIUtility.IconContent("d_Toolbar Minus").image), GUILayout.Width(position.width * 0.30f)))
                    {
                        is_question_saved = false;
                        remove_answer = true;
                        answer_to_remove = answersTextTmp[i];
                    }
                    
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
                if (remove_answer)
                {
                    answersTextTmp.Remove(answer_to_remove);
                    remove_answer = false;
                    answer_to_remove = "";
                }
                break;

            default:
                break;
        }

        
    }

    // affichage de confirmation de non-enregistrement des question (meme principe que quiz)
    private void ShowEraseQuestionConfirm()
    {
        GUILayout.Label("Are you sure you want to erase \" Question: " + questionTemp.title + "\" ?", style);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent(" Cancel", EditorGUIUtility.IconContent("d_winbtn_win_close").image)))
        {
            showEraseQuestionConfirm = false;
            showEditQuiz = true;
            LoadProject();
            questionTemp = null;
        }
        else if (GUILayout.Button(new GUIContent(" Confirm", EditorGUIUtility.IconContent("valid").image)))
        {
            is_quiz_saved = false;
            quizTemp.questions.RemoveAt(currentQuestionId);
            showEraseQuestionConfirm = false;
            showEditQuiz = true;
            // si on supprime, on doit decaler l'ordre pour toute les questions
            int i = 0;
            foreach (Question question in quizTemp.questions)
            {
                question.order = i;
                i++;
            }

            LoadProject();
            questionTemp = null;
        }
        GUILayout.EndHorizontal();
    }

    private void ShowNotQuizSaveConfirm()
    {
        GUILayout.Label("Don't you want to save the changes you made to\"" + quizTemp.name + "\" ?", style);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Don't save"))
        {
            GUILayout.EndHorizontal();
            is_quiz_saved = true;
            showNotQuizSaveConfirm = false;
            showQuizList = true;
            LoadProject();
            return;
        }
        else if (GUILayout.Button("Cancel"))
        {
            GUILayout.EndHorizontal();
            showNotQuizSaveConfirm = false;
            showEditQuiz = true;
            return;
        }
        GUILayout.EndHorizontal();
    }

    // meme principe que ci-dessus
    private void ShowNotQuestionSaveConfirm()
    {
        GUILayout.Label("Do you want to save the changes you made to\"" + questionTemp.title + "\" ?", style);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Don't save"))
        {
            GUILayout.EndHorizontal();
            is_question_saved = true;
            showNotQuestionSaveConfirm = false;
            showEditQuiz = true;
            // on efface les tempons
            answersImagesTmp = new();
            answersTextTmp = new();
            questionTemp = null;
            return;
        }
        else if (GUILayout.Button("Cancel"))
        {
            GUILayout.EndHorizontal();
            showNotQuestionSaveConfirm = false;
            showEditQuestion = true;
            return;
        }
        GUILayout.EndHorizontal();
    }
}