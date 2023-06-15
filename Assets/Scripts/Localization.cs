using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;

public class Localization : MonoBehaviour
{
    [SerializeField] ViewController ViewController;

    private LocalizeStringEvent LocalizeStringEventRef;
    private Locale EnLocale;
    private Locale EsLocale;

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

        // Initialize reference to check locale override status within ApplyLocaleOverride
        LocalizeStringEventRef = ViewController.GetComponentInChildren<LocalizeStringEvent>();

        // Set initial locale override to English
        ApplyLocaleOverride(EnLocale, ViewController.gameObject);
    }

    public void ChangeLang() 
    {
        if (LocalizeStringEventRef.StringReference.LocaleOverride == EnLocale)
        {
            ApplyLocaleOverride(EsLocale, ViewController.gameObject);
        }
        else if (LocalizeStringEventRef.StringReference.LocaleOverride == EsLocale)
        {
            ApplyLocaleOverride(EnLocale, ViewController.gameObject);
        }
        /*****************************************************************
        * Testing here
        *****************************************************************/
        // Debug.Log("hello!");
        // Debug.Log(LocalizationSettings.StringDatabase);
        // Debug.Log(LocalizeStringEventRef.StringReference.LocaleOverride);
        // Debug.Log(LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Home Canvas", "ModalContent", LocalizeStringEventRef.StringReference.LocaleOverride).Result);
        // Debug.Log(LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Home Canvas", "ModalContent", EnLocale).Result);
        // Debug.Log(LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Home Canvas", "ModalContent", EsLocale).Result);
        // Debug.Log(LocalizationSettings.StringDatabase.GetLocalizedString("Home Canvas", "ModalContent", EnLocale));
        // Debug.Log(LocalizationSettings.StringDatabase.GetLocalizedString("Home Canvas", "ModalContent", EsLocale));
        /*****************************************************************
        * End testing
        *****************************************************************/
	}

    // Override locale per canvas by looping through each LocalizeStringEvent component within a canvas
    private void ApplyLocaleOverride(Locale locale, GameObject canvasRoot)
    {
        foreach(var LocalizeStringEvent in canvasRoot.GetComponentsInChildren<LocalizeStringEvent>(true)) // Pass includeInactive param as true to get inactive UI strings too
        {
            LocalizeStringEvent.StringReference.LocaleOverride = locale;
        }
    }

}
