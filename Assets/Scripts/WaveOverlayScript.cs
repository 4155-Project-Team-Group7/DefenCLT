using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveOverlayScript : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] TextMeshProUGUI phaseUI;

    private void OnGUI()
    {
        waveUI.text = Spawner.currentWave.ToString();

        if (Spawner.inBuildMode == true)
        {
            phaseUI.text = "Build Phase";
        }
        else
        {
            phaseUI.text = "Fight Phase";
        }
    }
}
