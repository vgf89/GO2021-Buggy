using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    //public Sound[] songs;
    public Playlist[] playlists;
    private string currentPlaylist = "";
    private int currentSong;

    public string defaultPlaylist;

    private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();
    private Dictionary<string, Playlist> playListDict = new Dictionary<string, Playlist>();

    public static AudioManager instance;

    // Pool for playing one-shot sounds that need customizeable volumes etc at runtime (such as impact sounds)
    private int oneshotpoolsize = 4;
    private List<AudioSource> oneshotpool;
    private int oneshotpoolindex = 0;

    private List<AudioSource> loopSources;

    private Dictionary<string, int> playingLoops = new Dictionary<string, int>();

    private AudioSource songPlayer;


    void Awake()
    {
        // Singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
            s.source.loop = s.loop;

            soundDict[s.name] = s;
        }

        songPlayer = gameObject.AddComponent<AudioSource>();

        foreach(Playlist p in playlists) {
            playListDict[p.name] = p;
        }

        // Setup sound effect pool (so that sound effects can be played at various volumes, pitches, etc)
        oneshotpool = new List<AudioSource>();
        for (int i = 0; i < oneshotpoolsize; i++) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            oneshotpool.Add(source);
        }
    }

    public void Start()
    {
        if (defaultPlaylist == "") {
            return;
        }
        PlayPlaylist(defaultPlaylist);
    }

    void Update()
    {
        if (!songPlayer.isPlaying && currentPlaylist != "") {
            PlayPlaylist(currentPlaylist);
        }
    }

    static public void Play(string name) {
        instance.PlayInternal(name);
    }

    static public bool PlaySFX(string name, bool loop = false, float volume = 1f, float pitch = 1f, float panStereo = 0f, float spatialBlend = 0f, float reverbZoneMix = 1f) {
        return instance.PlaySFXInternal(name, loop, volume, pitch, panStereo, spatialBlend, reverbZoneMix);
    }

    static public void StopSFX(string name) {
        instance.StopSFXInternal(name);
    }

    private void PlayInternal(string name)
    {
        Sound s = soundDict[name];
        if (s == null) {
            return;
        }
        s.source.Play();
    }
    // If sound effect wasn't played, return true
    public bool PlaySFXInternal(string name, bool loop = false, float volume = 1f, float pitch = 1f, float panStereo = 0f, float spatialBlend = 0f, float reverbZoneMix = 1f)
    {
        if (name == null || name == "" || !soundDict.ContainsKey(name))
        {
            Debug.Log("Audiomanager: sound not found: \"" + name + "\"");
            return true;
        }

        Sound s = soundDict[name];
        if (s == null) {
            return true;
        }

        int tries = 0;
        while (oneshotpool[oneshotpoolindex].isPlaying) {
            oneshotpoolindex += 1;
            oneshotpoolindex %= oneshotpoolsize;
            tries++;
            if (tries > oneshotpoolsize) {
                return true;
            }
        }
        AudioSource source = oneshotpool[oneshotpoolindex];

        if (loop && playingLoops.ContainsKey(name)) {
            return true;
        }

        source.clip = s.clip;
        source.volume = s.volume * volume;
        source.pitch = s.pitch * pitch;
        source.panStereo = panStereo;
        source.spatialBlend = spatialBlend;
        source.reverbZoneMix = reverbZoneMix;
        source.loop = loop;
        source.Play();

        if (source.loop) {
            playingLoops.Add(name, oneshotpoolindex);
        }
        return false;
    }

    // Stop a looping sound from playing
    private void StopSFXInternal(string name) {
        if(playingLoops.ContainsKey(name)) {
            AudioSource source = oneshotpool[playingLoops[name]];
            source.Stop();
            playingLoops.Remove(name);
        }
    }

    public void Pause()
    {
        foreach(Sound s in sounds) {
            s.source.Pause();
        }

        foreach(AudioSource source in oneshotpool) {
            source.Pause();
        }

    }

    public void Resume()
    {
        foreach(Sound s in sounds) {
            s.source.UnPause();
        }

        foreach(AudioSource source in oneshotpool) {
            source.UnPause();
        }
    }

    public void PlayPlaylist(string name)
    {
        if (name == currentPlaylist && songPlayer.isPlaying) {
            return;
        }

        if (name != null && playListDict != null && !playListDict.ContainsKey(name)) {
            return;
        }
        Playlist p = playListDict[name];
        if (p == null) {
            return;
        }
        // If playlist isn't switching, pick a random song that's not the one that just played
        // Otherwise, just choose a random song
        if (name == currentPlaylist) {
            int i = currentSong;
            while (i == currentSong && p.songs.Length > 1) {
                i = Random.Range(0, p.songs.Length);
            }
            currentSong = i;
        } else {
            currentSong = Random.Range(0, p.songs.Length);
            currentPlaylist = name;
        }
        Sound s = p.songs[currentSong];

        songPlayer.volume = s.volume;
        if (songPlayer.clip != s.clip) { // If the clip is already playing, keep playing it.
            songPlayer.clip = s.clip;
            songPlayer.loop = false;
            songPlayer.Play();
        }
    }

    public void StopSong()
    {
        songPlayer.Stop();
    }

    public void PauseSong()
    {
        songPlayer.Pause();
    }

    public void ResumeSong()
    {
        songPlayer.UnPause();
    }
}
