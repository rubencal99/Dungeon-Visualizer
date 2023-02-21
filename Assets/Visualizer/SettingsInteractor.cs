using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsInteractor : MonoBehaviour
{
    public MapGenerator mapGenerator;

    [SerializeField]
    private float auxiliaryPercent;

    public float AuxiliaryPercent
    {
        get { return auxiliaryPercent; }
        set { auxiliaryPercent = value; }
    }
    

}
