using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
public class QuizCreator : MonoBehaviour
{
    public static GameObject quizPrefab;
    public static GameObject qYoNPrefab;
    public static GameObject qSliderPrefab;
    public static GameObject qImagesPrefab;

    private static string ACQPath;

    public static int Create(Quiz quiz)
    {
        if(quiz.questions.Count == 0)
        {
            Debug.LogError("There is no questions in your quiz !");
            return 2;
        }
        Init();
        //Loading error of the prefabs
        if (quizPrefab == null || qYoNPrefab == null || qSliderPrefab == null || qImagesPrefab == null)
            return 1;

        GameObject quizUI = Instantiate<GameObject>(quizPrefab);
        quizUI.name = quiz.name;
        GameObject BGUI = quizUI.transform.GetChild(0).gameObject;
        GameObject beginUI = BGUI.transform.GetChild(0).gameObject;
        GameObject endUI = BGUI.transform.GetChild(1).gameObject;

        List<GameObject> pages = new List<GameObject>();

        if (quiz.useBeginPage)
        {
            beginUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quiz.beginPageText;
            pages.Add(beginUI);
        }
        else
            beginUI.SetActive(false);

        endUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quiz.endPageText;

        quizUI.AddComponent<QuizManager>();
        QuizManager qM = quizUI.GetComponent<QuizManager>();

        bool first = true;
        int index = 1;
        foreach (Question q in quiz.questions)
        {
            switch (q.type)
            {
                case QuestionType.YoN : 
                    {
                        GameObject page = Instantiate<GameObject>(qYoNPrefab,BGUI.transform);
                        page.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = q.question;
                        page.name = q.title;

                        //Show description image
                        DescImageManager(q, quiz.name, page, new Vector2(1750, 750), new Vector2(0, -375));
                        HideOrShowPreviousButton(quiz.canGoBack, page);

                        pages.Add(page);
                        page.SetActive(false);
                        break;
                    }
                case QuestionType.Text :
                case QuestionType.Scale : 
                    {
                        GameObject page = Instantiate<GameObject>(qSliderPrefab, BGUI.transform);
                        page.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = q.question;
                        page.name = q.title;

                        if (!q.is_slider)
                        {
                            GameObject radio = page.transform.GetChild(2).gameObject;
                            GameObject button = radio.transform.GetChild(0).gameObject;
                            for (int i = 2; i < q.answersText.Count; i++)
                                Instantiate<GameObject>(button, radio.transform);
                            if (q.answersText.Count >= 2)
                            {
                                bool even = q.answersText.Count % 2 == 0;
                                for (int i = 0; i < q.answersText.Count; i++)
                                {
                                    radio.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = q.answersText[i];
                                    if (even)
                                        radio.transform.GetChild(i).transform.localPosition = new Vector3((q.answersText.Count / 2 - 1) * -200 - 100 + i * 200, 75, 0);
                                    else
                                        radio.transform.GetChild(i).transform.localPosition = new Vector3(((q.answersText.Count + 1) / 2 - 1) * -200 + i * 200, 75, 0);
                                }
                            }
                        }
                        else
                        {
                            GameObject sliderGo = page.transform.GetChild(6).gameObject;
                            Slider slider = sliderGo.transform.GetChild(1).GetComponent<Slider>();
                            float min = float.Parse(q.answersText[0]);
                            float max = float.Parse(q.answersText[1]);
                            slider.minValue = min;
                            slider.maxValue = max;
                            slider.value = (max + min) / 2;
                            if (q.answersText.Count > 2)
                            {
                                sliderGo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = q.answersText[2];
                                if (q.answersText.Count == 4)
                                    sliderGo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = q.answersText[3];
                                else
                                {
                                    sliderGo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = q.answersText[3];
                                    sliderGo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = q.answersText[4];
                                }
                            }
                            else
                            {
                                sliderGo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = slider.minValue.ToString();
                                sliderGo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
                                sliderGo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = slider.maxValue.ToString();
                            }
                            sliderGo.SetActive(true);
                            page.transform.GetChild(2).gameObject.SetActive(false);
                            page.transform.GetChild(4).GetComponent<Button>().interactable = true;
                        }

                        //Show description image
                        DescImageManager(q, quiz.name, page, new Vector2(1750, 750), new Vector2(0, -375));
                        HideOrShowPreviousButton(quiz.canGoBack, page);

                        pages.Add(page);
                        page.SetActive(false);
                        break;
                    }
                case QuestionType.ImagesSelect:
                    {
                        GameObject page = Instantiate<GameObject>(qImagesPrefab, BGUI.transform);
                        page.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = q.question;
                        page.name = q.title;

                        //Show description image
                        DescImageManager(q, quiz.name, page, new Vector2(1750, 600), new Vector2(0, -300));
                        HideOrShowPreviousButton(quiz.canGoBack, page);

                        GameObject radio = page.transform.GetChild(2).gameObject;
                        GameObject button = radio.transform.GetChild(0).gameObject;
                        for (int i = 2; i < q.answersImages.Count; i++)
                            Instantiate<GameObject>(button, radio.transform);
                        if (q.answersImages.Count >= 2)
                        {
                            bool even = q.answersImages.Count % 2 == 0;
                            int textsCount = q.answersText.Count;
                            for (int i = 0; i < q.answersImages.Count; i++)
                            {

                                string path = Path.Combine(ACQPath, "Questionnaires", quiz.name, "img", q.answersImages[i]);

                                if (File.Exists(path))
                                {
                                    byte[] imageBytes = File.ReadAllBytes(path);
                                    Texture2D tex = new Texture2D(2, 2);
                                    tex.LoadImage(imageBytes);
                                    tex.name = quiz.name + "_" + q.answersImages[i];
                                    radio.transform.GetChild(i).GetChild(0).GetComponent<RawImage>().texture = tex;
                                }
                                else
                                {
                                    Debug.LogWarning("L'image " + q.answersImages[i] + " est introuvable. L'image restera blanche.");
                                }

                                if(i< textsCount)
                                {
                                    radio.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = q.answersText[i];
                                }

                                if (even)
                                    radio.transform.GetChild(i).transform.localPosition = new Vector3((q.answersImages.Count / 2 - 1) * -300 - 100 + i * 300, 175, 0);
                                else
                                    radio.transform.GetChild(i).transform.localPosition = new Vector3(((q.answersImages.Count + 1) / 2 - 1) * -300 + i * 300, 175, 0);
                            }
                        }

                        pages.Add(page);
                        page.SetActive(false);
                        break;
                    }
            }
            if (first && !quiz.useBeginPage)
            {
                pages[^1].SetActive(true);
                HideOrShowPreviousButton(false, pages[^1]);
                first = false;
            }
            pages[^1].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = index.ToString() + " / " + quiz.questions.Count;
            index++;
        }
        if(quiz.useBeginPage)
        {
            HideOrShowPreviousButton(false, pages[1]);
        }
        pages[^1].transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Valider les réponses";
        pages.Add(endUI);
        qM.quiz = quizUI;
        qM.pages = pages;
        qM.useBeginPage = quiz.useBeginPage;

        string prefabPath = Path.Combine(ACQPath, "Quiz Prefabs");
        if (!Directory.Exists(prefabPath))
            Directory.CreateDirectory(prefabPath);

        string localPath = Path.Combine(prefabPath, quizUI.name + ".prefab");
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(quizUI, localPath, out bool prefabSuccess);
        if (!prefabSuccess)
        {
            Debug.LogError("Prefab failed to save");
            return 1;
        }
        return 0;
    }

    private static void HideOrShowPreviousButton(bool show, GameObject page)
    {
        if (!show)
            page.transform.GetChild(3).gameObject.SetActive(false);
    }

    private static void DescImageManager(Question q, string name, GameObject page, Vector2 sizeDelta, Vector2 pos)
    {
        if (q.descImage.Length != 0)
        {
            string path = Path.Combine(ACQPath, "Questionnaires", name, "img", q.descImage);

            if (File.Exists(path))
            {
                byte[] imageBytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);
                tex.name = name + "_" + q.descImage;
                page.transform.GetChild(1).GetComponent<RawImage>().texture = tex;
            }
            else
            {
                page.transform.GetChild(1).gameObject.SetActive(false);
                page.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = sizeDelta;
                page.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
                Debug.LogWarning("L'image " + q.descImage + " est introuvable. L'image de la question a été désactivé.");
            }
        }
        else
        {
            page.transform.GetChild(1).gameObject.SetActive(false);
            page.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = sizeDelta;
            page.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    private static void Init()
    {
        ACQPath = Directory.GetDirectories(Path.Combine(Application.dataPath), "ACQ", SearchOption.AllDirectories)[0];
        ACQPath = Path.Combine("Assets", Path.GetRelativePath("Assets", ACQPath));
        quizPrefab = Resources.Load(Path.Combine("Prefabs","Quiz")) as GameObject;
        qYoNPrefab = Resources.Load(Path.Combine("Prefabs","Question YoN")) as GameObject;
        qSliderPrefab = Resources.Load(Path.Combine("Prefabs","Question Slider")) as GameObject;
        qImagesPrefab = Resources.Load(Path.Combine("Prefabs","Question Images")) as GameObject;
    }
}
#endif