using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public AudioMixer audioMixer;

	public void PlayButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene.
    }

    public void QuitButton() {
        Application.Quit();
    }

    public void AdjustMasterVolume(float volume) {
        if(!Application.isEditor)
            audioMixer.SetFloat("MasterVolume", volume);   
    }

    public void AdjustMusicVolume(float volume) {
        if (!Application.isEditor)
            audioMixer.SetFloat("MusicVolume", volume);
    }

    public void AdjustSoundVolume(float volume) {
        if (!Application.isEditor)
            audioMixer.SetFloat("SoundVolume", volume);
    }

}
