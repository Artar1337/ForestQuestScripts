using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public bool changeImageColor = true;
    public float translationTime = 1f;
    public float waitTime = 0.5f;
    public Color startColor;
    private Color endColor;
    private Color currentColor;
    private float currentTime = -1f;
    private float step;
    private bool isPlaying = false;
    //0 + alpha
    //1 wait
    //2 - alpha
    private int stage = 0;
    private Image image;
    private TMPro.TMP_Text text;

    void Awake()
    {
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        if (changeImageColor)
            image = GetComponent<Image>();
        else
            text = GetComponent<TMPro.TMP_Text>();
    }

    public void InitiateAnimation()
    {
        step = 1f;
        gameObject.SetActive(true);
        currentTime = translationTime;
        stage = 0;
        currentColor = startColor;
        isPlaying = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isPlaying)
            return;

        currentTime -= Time.deltaTime;

        //меняем цвет
        if(stage != 1)
        {
            currentColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                currentColor.a + Time.deltaTime * step / translationTime);
            if (changeImageColor)
                image.color = currentColor;
            else
                text.color = currentColor;
        }
        
        if (currentTime < 0f)
        {
            if(stage == 0)
            {
                if (changeImageColor)
                    image.color = endColor;
                else
                    text.color = endColor;
                stage++;
                currentTime = waitTime;
            }
            else if(stage == 1)
            {
                stage++;
                step = -step;
                currentTime = translationTime;
            }
            else
            {
                stage = 0;
                isPlaying = false;
                if (changeImageColor)
                    image.color = startColor;
                else
                    text.color = startColor;
                gameObject.SetActive(false);
            }
        }
    }
}
