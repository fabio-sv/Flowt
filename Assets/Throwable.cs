using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Throwable : MonoBehaviour
{
    Vector3 throwVector;
    Rigidbody2D _rb;
    [SerializeField] Vector3 throwVelocity;
    public Vector3 velocity;
    private int timeAlive = 0;
    private float timer = 0f;
    private float delayAmount = 1;
    public GameObject gameManager;

    public int getTimeAlive() {  return timeAlive; }

    void Update()
    {
        velocity = _rb.velocity;

        if (TextUpdater.effectiveVelocity(velocity) == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= delayAmount)
        {
            timer = 0f;
            timeAlive++;
        }
    }
    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }
    //onmouse events possible thanks to monobehaviour + collider2d
    void OnMouseDown()
    {
        GameManager gm = gameManager.GetComponent<GameManager>();
        gm.interacted();

        CalculateThrowVector();

        //Debug.Log($"instance id: {this.gameObject.GetInstanceID()}");
        // Path.StartVisualizingPath(this.gameObject);
        PathHandler.StartVisualizingPath(this.gameObject);
    }
    void OnMouseDrag()
    {
        CalculateThrowVector(); 
        //Call after CalculateThrowVector() to work with updated throwVector value
        PathHandler.VisualizePath(this.gameObject,throwVector);
    }
    void CalculateThrowVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //doing vector2 math to ignore the z values in our distance.
        Vector2 distance = mousePos - this.transform.position;

        throwVector = -distance;    
        
        throwVelocity = throwVector;
    }
    void OnMouseUp()
    {
        //Has to be called before Throw() since simulation settings are reset in there
        PathHandler.StopVisualizingPath(this.gameObject);
        Throw();
    }
    public void Throw()
    {
        Debug.Log(_rb.velocity);
        _rb.AddForce(throwVector,ForceMode2D.Impulse);
        Debug.Log(_rb.velocity);
        this.GetComponent<Graviton>().IsAttractee = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Attractor") && !this.name.Contains("Clone")) // lol
        {
            Debug.Log($"Hit the attractor!!! {this.name}");
            Destroy(this.gameObject);

            GameManager gm = gameManager.GetComponent<GameManager>();
            gm.gameOver();
        }
    }
}
