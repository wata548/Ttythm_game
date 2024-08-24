using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    float power;
    const float deadLine = -6;

    private void Start()
    {
        power = ChartReader.Instance.power;
    }

    void Update()
    {
        transform.position =  new Vector2(transform.position.x, transform.position.y - power * Time.deltaTime);
        if(transform.position.y < deadLine)
        {
            Destroy(this.gameObject);
        }
    }
}
