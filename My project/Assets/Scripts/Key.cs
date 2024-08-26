using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    KeyCode key;
    [SerializeField] int limitMax = 100000000;
    [SerializeField] int press = 0;

    public bool keyDown;
    public bool keyPressingRaw;
    public bool keyUp;
    public int keyPressing;

    public Key(char key, int max = 10000)
    {
        key |= (char)32;

        this.key = (KeyCode)key;

        limitMax = max;
    }

    public Key(KeyCode key, int max = 10000)
    {
        this.key = key;

        limitMax = max;
    }

    public  void Set(char key, int max = 10000)
    {
        key |= (char)32;

        this.key = (KeyCode)key;

        limitMax = max;
    }

    public void Set(KeyCode key, int max = 10000)
    {
        this.key = key;

        limitMax = max;
    }

    public bool Down()
    {
        return Input.GetKeyDown(key);
    }

    public bool PressingRaw()
    {
        return Input.GetKey(key);
    }

    public int Pressing()
    {
        if (Input.GetKey(key))
        {
            press++;
        }
        else press = 0;

        return press;
    }

    public bool Up()
    {
        return Input.GetKeyUp(key);
    }
}
