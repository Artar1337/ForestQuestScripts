using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComputerQuiz : MonoBehaviour
{

    public float timeForQuiz = 60f;
    public GameObject[] objectsToActivate;
    public bool mathTask = false;
    public bool pictureTask = false;

    private float currentTime = 1f;
    private bool isPlaying = false;
    private bool playerWon = false;
    private int questionsAnswered = 0, questionsNeed;

    //math
    private TMPro.TMP_InputField input;
    private int correctAnswer;

    //picture task
    private Image image;
    private Dictionary<Sprite, string[]> pictures;
    private string[] imageAnswers;

    private ToggleGroup toggles;
    private int correctToggle, currentQ;
    private TMPro.TMP_Text[] answers;
    private TMPro.TMP_Text question, currentQText, timeText;
    private Dictionary<string, KeyValuePair<string[], int>> dictionary;
    private HashSet<int> listedQuestions;
    private System.Random random;

    private void Awake()
    {
        random = new System.Random();
        if (!pictureTask)
        {
            question = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
            currentQText = transform.Find("Number").GetComponent<TMPro.TMP_Text>();
        }
        else
        {
            image = transform.Find("Image").GetComponent<Image>();
        }
        if(pictureTask || mathTask)
        {
            input = transform.Find("Input").GetComponent<TMPro.TMP_InputField>();
            input.onSubmit.RemoveAllListeners();
        }
            

        timeText = transform.Find("Time").GetComponent<TMPro.TMP_Text>();
        Button button = transform.Find("OK").GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            if (!isPlaying)
                return;
            if (pictureTask)
            {
                string ans = input.text;
                if (ans == "")
                    return;

                foreach(string s in imageAnswers)
                {
                    if(ans == s.ToLowerInvariant())
                    {
                        //игра завершена
                        playerWon = true;
                        isPlaying = false;
                        //сообщение об этом
                        ComputerMessageShower.instance.ShowMessage(StringResources.
                            instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_win_picture")[0]);
                        if (objectsToActivate == null)
                            return;
                        foreach (GameObject obj in objectsToActivate)
                            obj.SetActive(true);
                        return;
                    }
                }
                //ответ неверен
                isPlaying = false;
                //сообщение об этом
                ComputerMessageShower.instance.ShowMessage(StringResources.
                    instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_fail_answer")[0]);
                return;
            }
            if (mathTask)
            {
                string ans = input.text;
                if (ans == "")
                    return;
                int answer;
                try
                {
                    answer = System.Convert.ToInt32(ans);
                }
                //ловим любые ошибки перевода числа в строку, делаем число равным минус 2 миллиарда
                catch
                {
                    answer = System.Int32.MinValue;
                }
                //на вопрос ответили правильно
                if (answer == correctAnswer)
                {
                    questionsAnswered++;
                    if (questionsAnswered >= questionsNeed)
                    {
                        //игра завершена
                        playerWon = true;
                        isPlaying = false;
                        //сообщение об этом
                        ComputerMessageShower.instance.ShowMessage(StringResources.
                            instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_win_math")[0]);
                        if (objectsToActivate == null)
                            return;
                        foreach (GameObject obj in objectsToActivate)
                            obj.SetActive(true);
                        return;
                    }
                    SetRandomMathQuestion();
                    return;
                }
                //ответ неверен
                isPlaying = false;
                //сообщение об этом
                ComputerMessageShower.instance.ShowMessage(StringResources.
                    instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_fail_answer")[0]);
                return;
            }

            Toggle toggle = toggles.GetFirstActiveToggle();
            if (toggle == null)
                return;
            int index = System.Convert.ToInt32(toggle.gameObject.name);
            //на вопрос ответили правильно
            if(index == correctToggle)
            {
                questionsAnswered++;
                if (questionsAnswered >= questionsNeed)
                {
                    //игра завершена
                    playerWon = true;
                    isPlaying = false;
                    //сообщение об этом
                    ComputerMessageShower.instance.ShowMessage(StringResources.
                        instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_win")[0]);
                    if (objectsToActivate == null)
                        return;
                    foreach (GameObject obj in objectsToActivate)
                        obj.SetActive(true);
                    return;
                }
                SetRandomQuestion();
                return;
            }
            //ответ неверен
            isPlaying = false;
            //сообщение об этом
            ComputerMessageShower.instance.ShowMessage(StringResources.
                instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_fail_answer")[0]);
        });
        if(pictureTask || mathTask)
        {
            input.onSubmit.RemoveAllListeners();
            input.onSubmit.AddListener((listenerValue) => { 
                button.onClick.Invoke(); 
                input.ActivateInputField(); 
            });
        }
        //формируем словарь для pictureTask
        if (pictureTask)
        {
            pictures = new Dictionary<Sprite, string[]>();
            string[] src = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "task3");
            if (src == null)
                throw new System.Exception("'task3' item in string resource file must not be null!");
            if (src.Length == 0)
                throw new System.Exception("'task3' item in string resource file must not contain 0 lines!");

            for(int i = 0; i < src.Length; i++)
            {
                string[] s = src[i].Split(System.Environment.NewLine.ToCharArray());
                if (s.Length < 2)
                    throw new System.Exception("Line number " + i + " in task3 should contain more than 1 value!");
                Sprite file = StringResources.instance.GetSpriteByName(s[0]);
                if(file == null)
                    throw new System.Exception("First value in line number " + i + " in task3 should contain valid sprite name from resources!");
                //добавляем спрайт и всё, кроме имени спрайта, в ответы
                pictures.Add(file, s.Skip(1).Take(s.Length - 1).ToArray());
            }
            return;
        } 
        if (mathTask)
        {
            questionsNeed = 10;
            return;
        }
            
        questionsNeed = 5;
        //формируем словарь из заданий
        dictionary = new Dictionary<string, KeyValuePair<string[], int>>();
        string[] source = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "task1");
        if(source == null)
            throw new System.Exception("'task1' item in string resource file must not be null!");
        if (source.Length % 3 != 0 || source.Length == 0)
            throw new System.Exception("'task1' item in string resource file must contain line number multiple of three!");

        for(int i = 0; i < source.Length; i += 3)
        {
            string q = source[i];
            string[] ans = source[i + 1].Split(System.Environment.NewLine.ToCharArray());
            List<string> nonEmptyAnswers = new List<string>();
            foreach (string s in ans)
                if (s.Length > 0)
                    nonEmptyAnswers.Add(s);
            ans = nonEmptyAnswers.ToArray();
            if (ans.Length != 4)
                throw new System.Exception("Line number " + (i + 2).ToString() + 
                    " must contain 4 answers, contains " + ans.Length);
            int right = -1;
            try
            {
                right = System.Convert.ToInt32(source[i + 2]);
            }
            catch
            {
                throw new System.Exception("Line number " + (i + 3).ToString() +
                    " must contain an integer, not a " + source[i + 2]);
            }
            if (right > 4 || right < 1)
                throw new System.Exception("Line number " + (i + 3).ToString() +
                    " must contain a value from 1 to 4, not a " + source[i + 2]);
            if (dictionary.ContainsKey(q))
            {
                throw new System.Exception("Multiply keys detected: " + q);
            }
            dictionary.Add(q, new KeyValuePair<string[], int>(ans, right - 1));
        }

        toggles = transform.Find("Answers").GetComponent<ToggleGroup>();
        List<TMPro.TMP_Text> tmp = new List<TMPro.TMP_Text>();
        for(int i = 0; i < toggles.transform.childCount; i++)
        {
            tmp.Add(toggles.transform.GetChild(i).transform.
                Find("Text").GetComponent<TMPro.TMP_Text>());
        }
        answers = tmp.ToArray();
    }

    private void SetRandomQuestion()
    {
        toggles.SetAllTogglesOff();

        int value;
        while (true)
        {
            value = random.Next(0, dictionary.Count);
            if (!listedQuestions.Contains(value))
                break;
        }
        listedQuestions.Add(value);

        KeyValuePair<string, KeyValuePair<string[], int>> pair = dictionary.ElementAt(value);
        question.text = pair.Key;
        int i = 0;
        foreach(TMPro.TMP_Text answer in answers)
        {
            answer.text = pair.Value.Key[i];
            i++;
        }
        correctToggle = pair.Value.Value;

        currentQ++;
        currentQText.text = currentQ.ToString() + ".";
    }

    private void SetRandomMathQuestion()
    {
        int signType = random.Next(0, 4), lop, rop;
        string q;
        input.text = "";
        input.Select();
        switch (signType)
        {
            // +
            case 0:
                lop = random.Next(-100, 101);
                rop = random.Next(-100, 101);
                correctAnswer = lop + rop;
                q = lop + " + ";
                if (rop < 0)
                    q += "(" + rop + ")";
                else
                    q += rop;
                break;
            // -
            case 1:
                lop = random.Next(-100, 101);
                rop = random.Next(-100, 101);
                correctAnswer = lop - rop;
                q = lop + " - ";
                if (rop < 0)
                    q += "(" + rop + ")";
                else
                    q += rop;
                break;
            // *
            case 2:
                lop = random.Next(-10, 11);
                rop = random.Next(-10, 11);
                correctAnswer = lop * rop;
                q = lop + " * ";
                if (rop < 0)
                    q += "(" + rop + ")";
                else
                    q += rop;
                break;
            // /
            case 3:
                rop = random.Next(-10, 11);
                while (rop == 0)
                    rop = random.Next(-10, 11);
                lop = random.Next(-10, 11) * rop;
                correctAnswer = lop / rop;
                q = lop + " / ";
                if (rop < 0)
                    q += "(" + rop + ")";
                else
                    q += rop;
                break;
            default:
                throw new System.Exception("Critical error generating equation!");
        }
        question.text = q;
        currentQ++;
        currentQText.text = currentQ.ToString() + ".";
    }

    private void SetRandomImageQuestion()
    {
        int index = random.Next(0, pictures.Count);
        image.sprite = pictures.ElementAt(index).Key;
        imageAnswers = pictures.ElementAt(index).Value;
        input.text = "";
        input.Select();
    }

    private void OnEnable()
    {
        isPlaying = false;
        if (playerWon)
        {
            //сообщение о том, что игрок уже выиграл в игру
            ComputerMessageShower.instance.ShowMessage(StringResources.
                instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_win_already")[0]);
            gameObject.SetActive(false);
            return;
        }
        currentTime = timeForQuiz;
        isPlaying = true;
        currentQ = 0;
        questionsAnswered = 0;
        if (mathTask)
        {
            correctAnswer = System.Int32.MaxValue;
            SetRandomMathQuestion();
            return;
        }
        if (pictureTask)
        {
            SetRandomImageQuestion();
            return;
        }
        correctToggle = -1;
        listedQuestions = new HashSet<int>();
        SetRandomQuestion();
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            currentTime -= Time.fixedDeltaTime;
            if (currentTime < 0f)
            {
                isPlaying = false;
                //сообщение о том, что время вышло
                ComputerMessageShower.instance.ShowMessage(StringResources.
                    instance.ElementAt(StringResources.LocalDictionaryType.UI, "task_fail_time")[0]);
            }
            timeText.text = currentTime.ToString("0.00");
        }
    }
}
