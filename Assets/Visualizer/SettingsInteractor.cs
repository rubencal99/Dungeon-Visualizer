using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsInteractor : MonoBehaviour
{
    public MapGenerator mapGenerator;

    [SerializeField]
    private float auxiliaryPercent;
    [SerializeField]
    private Text auxiliaryText;

    public float AuxiliaryPercent
    {
        get { return auxiliaryPercent; }
        set 
        { 
            auxiliaryPercent = (int)value; 
            auxiliaryText.text = "Auxiliary Percent: " + (int)value;
            mapGenerator.auxiliaryPercent = (int)value;
        }
    }
    
    [SerializeField]
    private float rewardPercent;
    [SerializeField]
    private Text rewardPercentText;

    public float RewardPercent
    {
        get { return rewardPercent; }
        set 
        { 
            rewardPercent = (int)value; 
            rewardPercentText.text = "Reward Percent: " + (int)value;
            mapGenerator.rewardPercent = (int)value;
        }
    }

    [SerializeField]
    private int rewardRoom;
    [SerializeField]
    private Text rewardRoomText;

    public string RewardRoom
    {
        get { return rewardRoom.ToString(); }
        set 
        { 
            rewardRoom = int.Parse(value); 
            rewardRoomText.text = "Reward Rooms: " + int.Parse(value);
            mapGenerator.MaxRewardRooms = int.Parse(value);
        }
    }

    [SerializeField]
    private bool bossRoomToggle;

    public bool BossRoomToggle
    {
        get { return bossRoomToggle; }
        set 
        { 
            bossRoomToggle = value; 
            mapGenerator.hasBoss = value;
        }
    }

    [SerializeField]
    private bool doorRoomToggle;

    public bool DoorRoomToggle
    {
        get { return doorRoomToggle; }
        set 
        { 
            doorRoomToggle = value;
        }
    }

}
