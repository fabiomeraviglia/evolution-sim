using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSlider : MonoBehaviour
{
    public void SetSpeed(float speed)
    {

        SimulationParameters.game_speed = speed;
    }
}
