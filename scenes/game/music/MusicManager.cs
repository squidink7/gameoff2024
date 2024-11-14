using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MusicManager : Node
{
    [Export] public float FadeDuration = 2.0f;

    private Dictionary<string, AudioStreamPlayer> tracks = new Dictionary<string, AudioStreamPlayer>();
    private List<string> trackNames = new List<string> { "amb", "lim", "cloud", "nice choir", "eeire", "song1", "spooky choir", "kamlib" };
    private int currentTrackIndex = 0;

    public override void _Ready()
    {
        GD.Print("Initializing MusicManager...");

        // Initialize the tracks dictionary with AudioStreamPlayers
        foreach (var child in GetChildren())
        {
            if (child is AudioStreamPlayer player)
            {
                tracks[player.Name] = player;
                player.VolumeDb = -80; // Start with muted volume
                GD.Print($"Added track: {player.Name}");
            }
        }

        // Start playing the first track
        if (trackNames.Count > 0)
        {
            GD.Print($"Starting first track: {trackNames[currentTrackIndex]}");
            PlayTrack(trackNames[currentTrackIndex]);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            NextTrack();
        }
    }

    // Function to play a track with fade-in
    public void PlayTrack(string trackName)
    {
        if (!tracks.ContainsKey(trackName))
        {
            GD.Print($"Track not found: {trackName}");
            return;
        }

        GD.Print($"Playing track: {trackName}");

        // Fade out all other tracks
        foreach (var name in tracks.Keys)
        {
            if (name != trackName)
            {
                FadeOut(tracks[name]);
            }
        }

        // Fade in the desired track
        FadeIn(tracks[trackName]);
    }

    private async void FadeIn(AudioStreamPlayer player)
    {
        player.Play();
        float startVolume = player.VolumeDb;
        float endVolume = 0;
        float timePassed = 0.0f;

        while (timePassed < FadeDuration)
        {
            await Task.Delay(100);
            timePassed += 0.1f;
            float t = timePassed / FadeDuration;
            player.VolumeDb = Mathf.Lerp(startVolume, endVolume, t);
        }
    }

    private async void FadeOut(AudioStreamPlayer player)
    {
        float startVolume = player.VolumeDb;
        float endVolume = -80;
        float timePassed = 0.0f;

        while (timePassed < FadeDuration)
        {
            await Task.Delay(100);
            timePassed += 0.1f;
            float t = timePassed / FadeDuration;
            player.VolumeDb = Mathf.Lerp(startVolume, endVolume, t);
        }

        player.Stop();
    }

    private void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % trackNames.Count;
        GD.Print($"Switching to track: {trackNames[currentTrackIndex]}");
        PlayTrack(trackNames[currentTrackIndex]);
    }
}