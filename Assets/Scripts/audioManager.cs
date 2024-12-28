using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public Sound[] sounds;
    private bool isMuted = false; // Trạng thái bật/tắt âm thanh

    // Khởi tạo âm thanh
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        PlaySound("nhacnen");
    }

    // Phát âm thanh
    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                return;
            }
        }
    }

    // Tắt hoặc bật âm thanh
    public void ToggleMute()
    {
        isMuted = !isMuted;
        foreach (Sound s in sounds)
        {
            s.source.mute = isMuted; // Bật/tắt âm thanh từng nguồn
        }
    }

    // Kiểm tra trạng thái hiện tại
    public bool IsMuted()
    {
        return isMuted;
    }
}
