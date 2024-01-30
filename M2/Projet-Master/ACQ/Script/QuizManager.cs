using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject quiz;
    public List<GameObject> pages;
    public bool useBeginPage = false;
    private int pageIndex = 0;
    private string fileName;
    private bool firstTime = true;
    private bool reloading = false;

    private float[] answers = new float[1];
    private List<string> allAnswers = new List<string>();

    private UnityAction nextAction;
    private UnityAction exportAction;
    private UnityAction goBackAction;
    private UnityAction<bool> onValueChangedAction;

    private void Start()
    {
        string dateOfQuiz = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
        fileName =  quiz.name + " " + dateOfQuiz.Replace(":", "");
        answers = new float[pages.Count-1];
    }

    private void InitAction()
    {
        nextAction += NextPage;
        exportAction += ExportResult;
        goBackAction += GoBackPage;
        onValueChangedAction += checkForNextButton;

        pages[^1].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(exportAction);

        pages[0].transform.GetChild(useBeginPage?1:4).GetComponent<Button>().onClick.AddListener(nextAction);
        for (int i = 1; i < pages.Count - 1; i++)
        {
            pages[i].transform.GetChild(4).GetComponent<Button>().onClick.AddListener(nextAction);
        }

        for (int i = useBeginPage?1:0; i < pages.Count - 1; i++)
        {
            pages[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(goBackAction);
            GameObject radios = pages[i].transform.GetChild(2).gameObject;
            for (int j = 0; j < radios.transform.childCount; j++)
            {
                radios.transform.GetChild(j).GetComponent<Toggle>().onValueChanged.AddListener(onValueChangedAction);
            }
        }
    }

    private void Awake()
    {
        InitAction();
    }

    private void checkForNextButton(bool change)
    {
        if(!reloading)
            pages[pageIndex].transform.GetChild(4).GetComponent<Button>().interactable = change;
    }

    private void NextPage()
    {
        if(!useBeginPage || pageIndex != 0)
        {
            if (pages[pageIndex].transform.GetChild(2).gameObject.activeInHierarchy)
            {
                ToggleGroup toggleGroup = pages[pageIndex].transform.GetChild(2).GetComponent<ToggleGroup>();
                if (toggleGroup.ActiveToggles().ToList().Count <= 0)
                    return;
                Toggle toggle = toggleGroup.ActiveToggles().First();
                answers[pageIndex] = toggle.gameObject.transform.GetSiblingIndex();
            }
            else
            {
                Slider slider = pages[pageIndex].transform.GetChild(6).GetChild(1).GetComponent<Slider>();
                answers[pageIndex] = slider.value;
            }
        }
        pages[pageIndex].SetActive(false);
        pageIndex++;
        pages[pageIndex].SetActive(true);
    }

    private void GoBackPage()
    {
        if (!useBeginPage || pageIndex != 0)
        {
            ToggleGroup toggleGroup = pages[pageIndex].transform.GetChild(2).GetComponent<ToggleGroup>();
            if (toggleGroup.ActiveToggles().ToList().Count > 0)
            {
                Toggle toggle = toggleGroup.ActiveToggles().First();
                answers[pageIndex] = toggle.gameObject.transform.GetSiblingIndex();
            }
            pages[pageIndex].SetActive(false);
            pageIndex--;
            pages[pageIndex].SetActive(true);
        }
    }

    private void RefreshAnswerInCanvas()
    {
        for (int i= useBeginPage ? 1 : 0; i< pages.Count-1; i++)
        {
            pages[i].SetActive(true);
            if (pages[i].transform.GetChild(2).gameObject.activeInHierarchy)
            {
                ToggleGroup toggleGroup = pages[i].transform.GetChild(2).GetComponent<ToggleGroup>();
                Toggle toggle = toggleGroup.ActiveToggles().First();
                toggle.isOn = false;
                pages[i].transform.GetChild(4).GetComponent<Button>().interactable = false;
            }
            else
            {
                Slider slider = pages[i].transform.GetChild(6).GetChild(1).GetComponent<Slider>();
                slider.value = (slider.maxValue + slider.minValue) / 2;
            }
            pages[i].SetActive(false);
        }
    }

    private void ExportResult()
    {
        if(firstTime)
        {
            string firstLine = "" + pages[useBeginPage ? 1 : 0].name;
            for (int i = useBeginPage?2:1; i < pages.Count - 1; i++)
            {
                firstLine += ";" + pages[i].name;
            }
            allAnswers.Add(firstLine);
        }

        string line = "" + answers[useBeginPage?1:0];
        for (int i = useBeginPage ? 2 : 1; i<answers.Length; i++)
        {
            line += ";" + answers[i];
        }
        allAnswers.Add(line);
        string folderPath = Path.Combine(Application.dataPath, "Data_CSV");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, fileName + ".csv");
        File.WriteAllLines(filePath, allAnswers);

        answers = new float[pages.Count - 1];
        reloading = true;
        RefreshAnswerInCanvas();
        reloading = false;
        pages[pageIndex].SetActive(false);
        pageIndex = 0;
        pages[pageIndex].SetActive(true);
        quiz.SetActive(false);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        firstTime = false;
    }
}