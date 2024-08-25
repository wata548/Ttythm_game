using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    //scroll speed
    float power;

    //where note destroy coordinateY
    const float deadLine = -6;

    private void Start()
    {
        power = ChartReader.Instance.power;
    }

    void Update()
    {
        //linear move
        transform.position =  new Vector2(transform.position.x, transform.position.y - power * Time.deltaTime);

        //when note very low, destroy note
        if(transform.position.y <= deadLine)
        {
            Destroy(this.gameObject);
        }
    }
}
