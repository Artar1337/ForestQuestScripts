using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ComputerWindowOpener : MonoBehaviour
{
    //���� ��� ���������
    public RectTransform windowToOpen;
    //ID ������ ��� �������� � �������
    public string textIDToShow;
    //������������ �� ����� � ��������
    public bool showInTheNote = false;
    //��� �����: �������� ��������� (��������� � ������)
    public bool localVideo = true;
    //��� �����: ������ � ��������� ������ / URL ��� ��������
    public string videoReference;
    //��� �����: �����-����� (� �������� ������)
    private ComputerVideoPlayStopPause player = null;
    //��� ��������: ������ ������ TMP � resizer
    private TMPro.TMP_Text note = null;

    //�������� �� ������ ������� �� ���������� ������
    public bool isFlash = false;

    //title ���� (��� ��������� �����)
    private TMPro.TMP_Text title = null;
    //title ������� ������
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
            //����� � ����� ������ ������ ������� ������� ������
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
