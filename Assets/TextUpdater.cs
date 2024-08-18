using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Throwable throwable = GetComponent<Throwable>();

        float velocity = effectiveVelocity(rb.velocity);
        int life = throwable.getTimeAlive();

        if (velocity != 0)
        {
            text.text = $"v={velocity} km.s<sup>-1</sup>\nage={life}s";
        }
    }

    public static float effectiveVelocity(Vector2 velocity)
    {
        return Mathf.Round(Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2)) * 100) / 100;
    }
}
