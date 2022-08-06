using UnityEngine;
using UnityEngine.UI;

public class SubtitleReader : MonoBehaviour
{
    public float timePerCharacter = 0.05f;
    public float timeToRead = 2f;

    private float currentTime = -1f;
    private GameObject imageObj, textObj;
    private TMPro.TMP_Text text, noteText;
    private Image image;
    private GameObject note;

    #region Singleton
    public static SubtitleReader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Subtitle reader instance error!");
            return;
        }
        instance = this;
    }
    #endregion

    private void Start()
    {
        imageObj = transform.Find("Image").gameObject;
        textObj = transform.Find("Text").gameObject;
        transform.Find("Note").Find("PressE").GetComponent<TMPro.TMP_Text>().text = 
            StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, "e_to_continue")[0];
        image = imageObj.GetComponent<Image>();
        text = textObj.GetComponent<TMPro.TMP_Text>();
        note = transform.Find("Note").gameObject;
        noteText = note.transform.Find("Text").GetComponent<TMPro.TMP_Text>();
    }

    public void SpeakLine(string line, Color background)
    {
        text.text = line;
        image.color = background;

        imageObj.SetActive(true);
        textObj.SetActive(true);

        currentTime = timeToRead + timePerCharacter * line.Length;
    }

    public void SetNoteWithText(string message)
    {
        note.SetActive(true);
        noteText.text = message;
    }

    public void DisableNote()
    {
        note.SetActive(false);
    }

    public bool InReadMode()
    {
        return note.activeInHierarchy;
    }

    // Update is called once per 0.2s
    private void FixedUpdate()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.fixedDeltaTime;

            if (currentTime < 0f)
            {
                imageObj.SetActive(false);
                textObj.SetActive(false);
            }
        }
    }
}
