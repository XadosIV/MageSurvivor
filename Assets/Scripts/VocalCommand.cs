using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;
public class VocalCommand : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;
    public bool printLanguage = true;

    [Header("UI")]
    public Button button;
    public Text buttonText;
    public Text outputText;

    private string _buffer;

    private void Awake()
    {
        whisper.OnNewSegment += OnNewSegment;

        microphoneRecord.OnRecordStop += OnRecordStop;

        button.onClick.AddListener(OnButtonPressed);
    }


    private void OnButtonPressed()
    {
        if (!microphoneRecord.IsRecording)
        {
            microphoneRecord.StartRecord();
            buttonText.text = "Stop";
        }
        else
        {
            microphoneRecord.StopRecord();
            buttonText.text = "Record";
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        buttonText.text = "Record";
        _buffer = "";

        var sw = new Stopwatch();
        sw.Start();

        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null || !outputText)
            return;

        var time = sw.ElapsedMilliseconds;
        var rate = recordedAudio.Length / (time * 0.001f);

        var text = res.Result;
        if (printLanguage)
            text += $"\n\nLanguage: {res.Language}";

        print(text);
    }

    private void OnNewSegment(WhisperSegment segment)
    {
        if (!streamSegments || !outputText)
            return;

        _buffer += segment.Text;
        outputText.text = _buffer + "...";
    }
}
