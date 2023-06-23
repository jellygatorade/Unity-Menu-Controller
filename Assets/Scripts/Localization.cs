using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class Localization : MonoBehaviour
{
    [SerializeField] private ViewController ViewController;

    private LocalizeStringEvent LocalizeStringEventRef;
    private Locale EnLocale;
    private Locale EsLocale;

    [SerializeField]
    private UnityEvent OnReadyAction;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Establish available locales
        //
        // These could also be accessed with 
        // LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.English);
        // LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.Spanish);
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var Locale = LocalizationSettings.AvailableLocales.Locales[i];
            switch (Locale.name)
            {
                case "English (en)":
                    EnLocale = LocalizationSettings.AvailableLocales.Locales[i];
                    break;

                case "Spanish (es)":
                    EsLocale = LocalizationSettings.AvailableLocales.Locales[i];
                    break;
            }
        }

        // Set initial locale to English
        // ApplyLocale(EnLocale);
        
        // For splitscreen - Initialize reference to check locale override status within ApplyLocaleOverride
        LocalizeStringEventRef = ViewController.GetComponentInChildren<LocalizeStringEvent>();
        
        // For splitscreen - Set initial locale override to English for all descendants of ViewController.gameObject
        //ApplyLocaleOverride(EnLocale, ViewController.gameObject);

        // Invoke post initialization action (remove a loading screen)
        OnReadyAction?.Invoke();
    }

    // Toggle locale for entire game
    public void ToggleLocale()
    {
        if (LocalizationSettings.SelectedLocale == EnLocale)
        {
            ApplyLocale(EsLocale);
        }
        else if (LocalizationSettings.SelectedLocale == EsLocale)
        {
            ApplyLocale(EnLocale);
        }
    }

    // For splitscreen usage - toggle locale override for all descendants of ViewController.gameObject
    public void ToggleLocaleOverride() 
    {
        if (LocalizeStringEventRef.StringReference.LocaleOverride == EnLocale)
        {
            ApplyLocaleOverride(EsLocale, ViewController.gameObject);
        }
        else if (LocalizeStringEventRef.StringReference.LocaleOverride == EsLocale)
        {
            ApplyLocaleOverride(EnLocale, ViewController.gameObject);
        }
	}

    // Apply locale to entire game
    private void ApplyLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;
    }

    // For splitscreen usage - override locale per canvas by looping through each LocalizeStringEvent component within a canvas
    private void ApplyLocaleOverride(Locale locale, GameObject canvasRoot)
    {
        foreach(var LocalizeStringEvent in canvasRoot.GetComponentsInChildren<LocalizeStringEvent>(true)) // Pass includeInactive param as true to get inactive UI strings too
        {
            LocalizeStringEvent.StringReference.LocaleOverride = locale;
        }
    }

}
