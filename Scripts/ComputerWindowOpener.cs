using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ComputerWindowOpener : MonoBehaviour
{
    //окно для включения
    public RectTransform windowToOpen;
    //ID текста для загрузки в блокнот
    public string textIDToShow;
    //показывается ли текст в блокноте
    public bool showInTheNote = false;
    //для видео: является локальным (загружено в проект)
    public bool localVideo = true;
    //для видео: строка с локальным именем / URL для загрузки
    public string videoReference;
    //для видео: видео-плеер (в качестве кнопки)
    private ComputerVideoPlayStopPause player = null;
    //для блокнота: объект текста TMP и resizer
    private TMPro.TMP_Text note = null;

    //является ли иконка ссылкой на содержимое флешки
    public bool isFlash = false;

    //title окна (имя открытого файла)
    private TMPro.TMP_Text title = null;
    //title текущей иконки
    private TMPro.TMP_Text iconTitle = null;

    public void OpenWindow()
    {
        if (isFlash)
        {
            if (!ComputerMessageShower.instance.GetSender().IsFlashDriveInside())
            {
                ComputerMessageShower.instance.ShowMessage(StringResources.
                    instance.ElementAt(StringResources.LocalDictionaryType.UI, textIDToShow)[0]);
                return;
            }
            windowToOpen = ComputerMessageShower.instance.GetSender().flashWindow;
        }
        if (textIDToShow != "" && !showInTheNote && !isFlash)
        {
            ComputerMessageShower.instance.ShowMessage(StringResources.
                instance.ElementAt(StringResources.LocalDictionaryType.UI, textIDToShow)[0]);
            return;
        }
        if (windowToOpen.gameObject.activeInHierarchy && videoReference != "")
            return;
        windowToOpen.gameObject.SetActive(true);
        windowToOpen.SetAsLastSibling();
        if (title == null)
        {
            title = windowToOpen.transform.Find("TitleImage").transform.Find("Title").GetComponent<TMPro.TMP_Text>();
            iconTitle = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
        }
            
        if (showInTheNote)
        {
            title.text = iconTitle.text;
            if(note == null)
            {
                Transform tmp = windowToOpen.transform.Find("Scroll Area").transform;
                note = tmp.Find("Text").GetComponent<TMPro.TMP_Text>();
            }
            string s = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI, textIDToShow)[0];
            note.text = s;
            //нужно в любом случае именно сменить текущий индекс
            ComputerNoteTextResize.instance.Resize(1);
            //40 px
            ComputerNoteTextResize.instance.Resize(9);
            return;
        }
        if (videoReference != "")
        {
            title.text = iconTitle.text;
            if (player == null)
                player = windowToOpen.transform.Find("Controls").transform.
                Find("Play").GetComponent<ComputerVideoPlayStopPause>();
            player.Initialize(localVideo, videoReference);
            return;
        }
    }
}
