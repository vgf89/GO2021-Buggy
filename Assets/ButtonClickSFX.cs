using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSFX : MonoBehaviour
{
    [SerializeField] string soundEffect = "click";
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(sfx);
    }

    void sfx() {
        AudioManager.PlaySFX(soundEffect);
    }
}
