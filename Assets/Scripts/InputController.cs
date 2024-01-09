using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Inputs inputs;

    private int ReverseButton(int button)
    {
        if (button % 2 == 0)
            button += 1;
        else
            button -= 1;
        return button;
    }

    public bool GetButton (int button, bool reverse, bool once)
    {
        bool pressed = false;

        if (reverse)
        {
            button = ReverseButton(button);
        }

        if (inputs.Axis[button])
        {
            if (button % 2 == 1)
                pressed = Input.GetAxis(inputs.Buttons[button]) >= inputs.AxisThreshold;
            else
                pressed = Input.GetAxis(inputs.Buttons[button]) <= 0f - inputs.AxisThreshold;
        }
        else
        {
            if (once)
                pressed = Input.GetButtonDown(inputs.Buttons[button]);
            else
                pressed = Input.GetButton(inputs.Buttons[button]);
        }
        
        return pressed;
    }

    public float GetAxis(int button, bool reverse)
    {
        float value = 0f;

        if (reverse)
        {
            button = ReverseButton(button);
        }

        if (inputs.Axis[button])
        {
            if (button % 2 == 1)
                value = Input.GetAxis(inputs.Buttons[button]);
            else
                value = 0f - Input.GetAxis(inputs.Buttons[button]);
        }
        else
        {
            if (Input.GetButton(inputs.Buttons[button]))
                value += 1f;
            button = ReverseButton(button);
            if (Input.GetButton(inputs.Buttons[button]))
                value -= 1f;
        }

        return value;
    }
}
