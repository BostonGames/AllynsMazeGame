using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] GameObject hoverFill;
    [SerializeField] GameObject hoverBorder;

    private AudioSource buttonAudioSource;
    [SerializeField] AudioClip buttonHoverSound;

    private void Awake()
    {
        hoverFill.SetActive(false);
        hoverBorder.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonHoverSound()
    {
        buttonAudioSource.PlayOneShot(buttonHoverSound);
    }

    public void OnButtonHover0()
    {
        hoverBorder.SetActive(true);
        hoverFill.SetActive(true);
    }

    public void OnButtonExit0()
    {
        hoverBorder.SetActive(false);
        hoverFill.SetActive(false);
    }
}
