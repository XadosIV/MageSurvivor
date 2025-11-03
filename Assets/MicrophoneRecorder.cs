using System.IO;
using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{

    private AudioClip clip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            clip = Microphone.Start(null, false, 5, 44100);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            int length = Microphone.GetPosition(null);
            Microphone.End(null);

            int extraSamples = clip.frequency; // 1 seconde de plus
            int finalLength = Mathf.Min(length + extraSamples, clip.samples); // éviter de dépasser la taille max

            AudioClip clipTrimmed = AudioClip.Create("Trimmed", finalLength, clip.channels, clip.frequency, false);
            float[] samples = new float[finalLength * clip.channels];
            clip.GetData(samples, 0); // récupère les samples existants
                                      // Les samples supplémentaires restent à 0 = silence
            clipTrimmed.SetData(samples, 0);

            SaveWav(clipTrimmed, "Recording.wav");
        }

    }

    void SaveWav(AudioClip clip, string filename)
    {
        if (clip == null) return;

        var filepath = Path.Combine(Application.dataPath + "/MagieNoire", filename);
        Debug.Log(filepath);
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        using (var fileStream = new FileStream(filepath, FileMode.Create))
        {
            int hz = clip.frequency;
            int channels = clip.channels;
            int samplesCount = samples.Length;

            // Header WAV
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(36 + samplesCount * 2), 0, 4);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(16), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes((ushort)1), 0, 2);
            fileStream.Write(System.BitConverter.GetBytes((ushort)channels), 0, 2);
            fileStream.Write(System.BitConverter.GetBytes(hz), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(hz * channels * 2), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes((ushort)(channels * 2)), 0, 2);
            fileStream.Write(System.BitConverter.GetBytes((ushort)16), 0, 2);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(samplesCount * 2), 0, 4);

            // Données audio
            foreach (var sample in samples)
            {
                short intSample = (short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue);
                fileStream.Write(System.BitConverter.GetBytes(intSample), 0, 2);
            }
        }
    }

}
