using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    public bool isMainHandler = false;
    public bool performUpdate = false;
    //используется только для кнопок смены языка
    public StringResources.Localization languageToSet = StringResources.Localization.English;
    private Transform canvasContainer;
    private PlayerLookDirectionChecker currentChecker;
    private float currentKeyPressDelay = 0f;
    private bool menuOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Menu Canvas") != null)
            canvasContainer = GameObject.Find("Menu Canvas").transform;
        else
            canvasContainer = GameObject.Find("Main Canvas").transform;
        currentChecker = null;

        if (isMainHandler)
        {
            transform.Find("Loading").gameObject.SetActive(false);
            if (GameObject.Find("FPS Controller") != null)
            {
                currentChecker = GameObject.Find("FPS Controller").transform.
                    Find("Main Camera").GetComponent<PlayerLookDirectionChecker>();
            }
            if (transform.Find("Screenshot") != null)
                transform.Find("Screenshot").Find("Text").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "screenshot")[0];
            if (transform.Find("Loading") != null)
                transform.Find("Loading").Find("Text").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "loading")[0];
            Transform currentContainer = transform.Find("Main Container");
            if (currentContainer.Find("Start") != null)
                currentContainer.Find("Start").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "start")[0];
            currentContainer.Find("Settings").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "settings")[0];
            currentContainer.Find("Info").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "information")[0];
            if (currentContainer.Find("LanguagePopup") != null)
                currentContainer.Find("LanguagePopup").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "language")[0];
            if (currentContainer.Find("Exit") != null)
                currentContainer.Find("Exit").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "exit")[0];
            if (currentContainer.Find("Continue") != null)
                currentContainer.Find("Continue").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "continue")[0];
            if (currentContainer.Find("HowToPlay") != null)
                currentContainer.Find("HowToPlay").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "howtoplay")[0];
            if (currentContainer.Find("ToMenu") != null)
                currentContainer.Find("ToMenu").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "to_menu")[0];
            if(currentContainer.Find("About") != null)
                currentContainer.Find("About").GetComponent<TMPro.TMP_Text>().text =
                    StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "thanks")[0];

            currentContainer = transform.Find("Settings Container");
            currentContainer.Find("Exit").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "go_back")[0];
            currentContainer.Find("Resolution").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "resolution")[0];
            currentContainer.Find("Fullscreen").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "fullscreen")[0];
            currentContainer.Find("Volume").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "volume")[0];
            currentContainer.Find("Quality").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "quality")[0];
            currentContainer.Find("Sensibility").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "sensibility")[0];

            TMPro.TMP_Dropdown dropdown = currentContainer.Find("Resolution").
                Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options.Clear();
            foreach(Resolution r in Screen.resolutions)
            {
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(r.ToString()));
            }
            
            dropdown = currentContainer.Find("Quality").
                Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();
            dropdown.options.Clear();
            string[] options = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "quality");
            for (int i = 1; i < options.Length; i++)
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(options[i]));

            currentContainer = transform.Find("About Container");
            currentContainer.Find("Exit").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "go_back")[0];
            currentContainer.Find("About").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "about")[0];

            currentContainer = transform.Find("Help Container");
            if (currentContainer == null)
                return;
            currentContainer.Find("Exit").Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "go_back")[0];
            currentContainer.Find("About").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "help")[0];
        }
    }

    public void SetUI()
    {
        if (!isMainHandler || transform.Find("Settings Container") == null)
            return;

        Transform container = transform.Find("Settings Container").transform;

        //устанавливаем dropdown разрешения в нужное нам значение
        container.Find("Resolution").Find("Dropdown").
            GetComponent<TMPro.TMP_Dropdown>().value = Screen.resolutions.Length - 1;
        //устанавливаем dropdown качества
        container.Find("Quality").Find("Dropdown").
            GetComponent<TMPro.TMP_Dropdown>().value = PlayerPrefs.GetInt("quality");
        //устанавливаем togglebox полного экрана
        container.Find("Fullscreen").Find("Toggle").
            GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("fullscreen") > 0;
        //устанавливаем слайдер громкости
        container.Find("Volume").Find("Slider").
            GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("volume") / 100f;
        //устанавливаем слайдер чувствительности
        container.Find("Sensibility").Find("Slider").
            GetComponent<Slider>().value = PlayerPrefs.GetInt("mouse");
    }

    public void StartGame()
    {
        canvasContainer.Find("Loading").gameObject.SetActive(true);
        StartCoroutine(LoadForest());
    }

    private IEnumerator LoadForest()
    {
        yield return null;
        SceneManager.LoadScene("Forest", LoadSceneMode.Single);
    }

    private IEnumerator LoadMenu()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void ShowLanguageConfirmationField(bool show)
    {
        GameObject g = canvasContainer.Find("Main Container").Find("LanguagePopup").gameObject;
        g.SetActive(show);
        if (show)
        {
            g.transform.Find("OK").GetComponent<MenuButtonHandler>().languageToSet = this.languageToSet;
        }
    }

    //использовать только для кнопки ОК!
    public void SetLanguage()
    {
        PlayerPrefs.SetInt("l", (int)languageToSet);
        StringResources.instance.currentLocalization = this.languageToSet;
        Exit();
    }

    public void HowToPlay(bool open)
    {
        canvasContainer.Find("Main Container").gameObject.SetActive(!open);
        canvasContainer.Find("Help Container").gameObject.SetActive(open);
    }

    public void Settings(bool open)
    {
        canvasContainer.Find("Main Container").gameObject.SetActive(!open);
        canvasContainer.Find("Settings Container").gameObject.SetActive(open);
    }

    public void About(bool open)
    {
        canvasContainer.Find("Main Container").gameObject.SetActive(!open);
        canvasContainer.Find("About Container").gameObject.SetActive(open);
    }

    public void SetGlobalVolume(float volume)
    {
        if (volume < 0 || volume > 1)
            return;
        AudioListener.volume = volume;
        GameSettingsLoader.instance.UpdateKey("volume", (int)(volume * 100f));
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(Screen.resolutions[index].width,
            Screen.resolutions[index].height, Screen.fullScreen,
            Screen.resolutions[index].refreshRate);
        GameSettingsLoader.instance.UpdateKey("w", Screen.resolutions[index].width);
        GameSettingsLoader.instance.UpdateKey("h", Screen.resolutions[index].height);
        GameSettingsLoader.instance.UpdateKey("rate", Screen.resolutions[index].refreshRate);
    }

    public void SetResolution(int W, int H, int rate)
    {
        Screen.SetResolution(W, H, Screen.fullScreen, rate);
    }

    public void SetMouseFloatSensibility(float value)
    {
        GameSettingsLoader.instance.UpdateKey("mouse", (int)value);
        if (currentChecker != null)
        {
            currentChecker.mouseSensitivity = value;
        }
    }

    public static void SetMouseSensibility(int value)
    {
        GameSettingsLoader.instance.UpdateKey("mouse", (int)value);
    }

    public static int GetMouseSensibility()
    {
        if (!PlayerPrefs.HasKey("mouse"))
            PlayerPrefs.SetInt("mouse", 500);
        return PlayerPrefs.GetInt("mouse");
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        GameSettingsLoader.instance.UpdateKey("quality", index);
    }

    public void SetFullscreenMode(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if(fullscreen)
            GameSettingsLoader.instance.UpdateKey("fullscreen", 1);
        else
            GameSettingsLoader.instance.UpdateKey("fullscreen", 0);
    }

    public void ToMenu()
    {
        canvasContainer.Find("Loading").gameObject.SetActive(true);
        StartCoroutine(LoadMenu());
    }

    public void ContinueGame()
    {
        canvasContainer.Find("Crosshair").gameObject.SetActive(true);
        canvasContainer.Find("Main Container").gameObject.SetActive(false);
        Inventory.instance.EnableUserLook(true);
        Inventory.instance.Updating = true;
        //для объекта - perform update экземпляра menuOpened ставим в false
        canvasContainer.GetComponent<MenuButtonHandler>().menuOpened = false;
    }

    public void OpenMenu()
    {
        canvasContainer.Find("Crosshair").gameObject.SetActive(false);
        canvasContainer.Find("Main Container").gameObject.SetActive(true);
        canvasContainer.Find("Settings Container").gameObject.SetActive(false);
        canvasContainer.Find("About Container").gameObject.SetActive(false);
        if (Inventory.instance != null)
        {
            Inventory.instance.EnableUserLook(false);
            Inventory.instance.Updating = false;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!performUpdate)
            return;
        if (currentKeyPressDelay > 0f)
        {
            currentKeyPressDelay -= Time.deltaTime;
            return;
        }
        if (Input.GetAxis("Menu") > 0f && !menuOpened)
        {
            OpenMenu();
            menuOpened = true;
            currentKeyPressDelay = 1f;
        }
    }
}
