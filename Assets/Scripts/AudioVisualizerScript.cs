using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizerScript : MonoBehaviour {

    public LineRendererDataScript[] RendererData;
    public Text                     NameSongLabel;
    public Text                     TimeLabel;

    private const int QSamples = 1024;

    float[] _samples;
    float[] _freqBand;

    private AudioSource _audioSource;

    void Start()
    {
        //Initialize buffers
        _samples    = new float[QSamples];
        _freqBand    = new float[RendererData.Length];
        _audioSource = GetComponent<AudioSource>();
        NameSongLabel.text = _audioSource.clip.name;
    }

    void Update()
    {
        TimeLabel.text = _audioSource.time.ToString();

        //Get the audio data
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);

        int count = 0;

        //Calculate an average value for a frequency range
        for (var i = 0; i < RendererData.Length; ++i)
        {
            float average = 0;
            int sampleCount = _samples.Length / RendererData.Length;

            for (var j = sampleCount * i; j < sampleCount * i + sampleCount; ++j) {
                if (count < _samples.Length)
                    average += _samples[count] * (count + 1);
                ++count;
            }

            average /= sampleCount;

            _freqBand[i] = average;

            //Add a new value each frame to renderer script
            RendererData[i].AddPoint(_freqBand[i]);
        }
    }

    public void Quit() {
        Application.Quit();
    }
}