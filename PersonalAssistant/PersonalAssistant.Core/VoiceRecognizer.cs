using NAudio.Wave;
using Newtonsoft.Json;
using PersonalAssistant.Core.AssistantModels;
using Vosk;

namespace PersonalAssistant.Core;

public class VoiceRecognizer
{
    private readonly VoskRecognizer _voskRecognizer;
    private readonly WaveInEvent _waveIn;
    private readonly List<string> _stringParts;
    private DateTime _lastAcceptVoiceTime;
    private Timer _timer;
    private int _partsCount;
    public event EventHandler<string> VoiceRecognized;

    public VoiceRecognizer(Model languageModel, WaveInEvent waveIn)
    {
        _waveIn = waveIn;
        _stringParts = new List<string>();
        
        _voskRecognizer = new VoskRecognizer(languageModel, 16000);
        _voskRecognizer.SetMaxAlternatives(0);
        _voskRecognizer.SetWords(true);
    }

    public void Init()
    {
        _timer = new Timer(TimerCallback, null, 0, 200);
        Console.WriteLine("VoiceRecognizer initiated.");
    }

    private async void TimerCallback(object o)
    {
        if (_stringParts.Count != _partsCount)
        {
            _lastAcceptVoiceTime = DateTime.UtcNow;
            _partsCount = _stringParts.Count;
        }
        else
        {
            var canAsk = _stringParts.Any(x => !string.IsNullOrEmpty(x)) && (DateTime.UtcNow - _lastAcceptVoiceTime).TotalSeconds > 2;

            if (!canAsk)
            {
                return;
            }

            _waveIn.StopRecording();
            await _timer.DisposeAsync();

            var text = string.Join(" ", _stringParts.Where(x => !string.IsNullOrEmpty(x)));
            
            Console.WriteLine($"Text recognized: {text}");

            VoiceRecognized?.Invoke(this, text);

            _stringParts.Clear();
            _partsCount = 0;

            _voskRecognizer.Reset();
            _waveIn.StartRecording();
            _timer = new Timer(TimerCallback, null, 0, 200);
        }
    }

    public void StartListen()
    {
        if (_timer == null)
        {
            throw new Exception($"{nameof(VoiceRecognizer)} not initiated. Need call Init()");
        }
        
        _waveIn.DataAvailable += WaveIn_DataAvailable;
        _waveIn.StartRecording();
        
        Console.WriteLine("Listen started.");
    }
    
    public async Task StopListen()
    {
        if (_timer != null)
        {
            await _timer.DisposeAsync();
        }
        
        _waveIn.StopRecording();
        _waveIn.DataAvailable -= WaveIn_DataAvailable;
    }
    
    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        try
        {
            if (!_voskRecognizer.AcceptWaveform(e.Buffer, e.Buffer.Length))
            {
                return;
            }
            
            var query = JsonConvert.DeserializeObject<VoskPartialTextModel>(_voskRecognizer.PartialResult())?.Partial;

            if (string.IsNullOrEmpty(query))
            {
                return;
            }
            
            if (_stringParts.All(x => x != query))
            {
                _stringParts.Add(query);
            }
        }
        catch (Exception exception)
        {
            // ignored
        }
    }
}