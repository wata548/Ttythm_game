using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    List<Key> keys = new List<Key>();
    float judgeLine = -1.75f;
    static float sum = 0f;
    static int count = 0;
    public float result = 0;
    InputManager()
    {
        keys.Add(new Key('d'));
        keys.Add(new Key('f'));
        keys.Add(new Key('j'));
        keys.Add(new Key('k'));
    }

    private void Update()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Down())
            {
                float check = 0;

                if (Note.presentChart[i].Count != 0)
                {
                    Note temp = Note.presentChart[i].Peek();
                    check = temp.GetCoor().y - judgeLine;
                    Note.presentChart[i].Dequeue();

                    Debug.Log($"{i + 1}line {check}mm");
                    count++;
                    sum += check;
                    result = check / count;
                }
            }
        }
    }

}
