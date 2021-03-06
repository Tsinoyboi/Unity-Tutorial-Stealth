﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;

    private RectTransform rectTransform;
    private RawImage rawImage;

    private bool sceneStarting = true;

    void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        rawImage = GetComponent<RawImage>();
    }

    void Update ()
    {
        if (sceneStarting)
        {
            StartScene();
        }
    }

    void FadeToClear ()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack ()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene ()
    {
        FadeToClear();
        if (rawImage.color.a <= 0.05f)
        {
            rawImage.color = Color.clear;
            rawImage.enabled = false;
            sceneStarting = false;
        }
    }

    public void EndScene ()
    {
        rawImage.enabled = true;
        FadeToBlack();

        if (rawImage.color.a >= 0.95f)
        {
            Application.LoadLevel(1);
        }
    }
}
