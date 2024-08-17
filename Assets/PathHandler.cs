using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PathHandler : MonoBehaviour
{
    public static PathHandler Instance;
    static GameObject clone = null;

    static List<Graviton> gravitons = new List<Graviton>();
    public static bool isBeingAimed;
    void Awake()
    {
        Instance = this;
    }
    public static void StartVisualizingPath(GameObject throwableGameObject)
    {
        GravityHandler.isSimulatingLive = false;

        throwableGameObject.GetComponent<Rigidbody2D>().simulated = false;
        if (clone == null)
            clone = Instantiate(throwableGameObject, throwableGameObject.transform.position, Quaternion.identity);

        clone.GetComponent<SpriteRenderer>().enabled = false;

        GravityHandler.attractees.Remove(throwableGameObject.GetComponent<Rigidbody2D>());
        GravityHandler.attractors.Remove(throwableGameObject.GetComponent<Rigidbody2D>());
        GravityHandler.attractees.Add(clone.GetComponent<Rigidbody2D>());
        Physics2D.simulationMode = SimulationMode2D.Script;

    }
    public static void VisualizePath(GameObject throwableGameObject, Vector3 force)
    {
        clone.transform.position = throwableGameObject.transform.position;
        Rigidbody2D cloneRigidBody = clone.GetComponent<Rigidbody2D>();
        throwableGameObject.GetComponentInChildren<TrailRenderer>().enabled = false;

        cloneRigidBody.velocity = throwableGameObject.GetComponent<Rigidbody2D>().velocity;
        cloneRigidBody.AddForce(force, ForceMode2D.Impulse);///////////
        clone.GetComponent<Rigidbody2D>().simulated = true;

        List<Vector3> pathPoints = new List<Vector3>();
        int simulationSteps = 300;
        for (int i = 1; i < simulationSteps; i++)
        {
            GravityHandler.SimulateGravities();
            Physics2D.Simulate(Time.fixedDeltaTime);

            pathPoints.Add(cloneRigidBody.transform.position);
        }

        //set stored positions from simulation as linerenderer positions
        LineRenderer linePath = clone.GetComponent<LineRenderer>();
        linePath.enabled = true;
        linePath.positionCount = pathPoints.Count;
        var gradient = new Gradient();

        // Blend color from red at 0% to blue at 100%
        var colors = new GradientColorKey[2];
        colors[0] = new GradientColorKey(Color.black, 0.0f);
        colors[1] = new GradientColorKey(Color.black, 1.0f);

        // Blend alpha from opaque at 0% to transparent at 100%
        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(0.7f, 0.0f);
        alphas[1] = new GradientAlphaKey(0.0f, 1.0f);

        gradient.SetKeys(colors, alphas);
        linePath.colorGradient = gradient;
        linePath.SetPositions(pathPoints.ToArray());
    }

    public static void StopVisualizingPath(GameObject throwableGameObject)
    {
        GravityHandler.isSimulatingLive = true;
        Debug.Log("Stop visualize");
        throwableGameObject.GetComponent<Rigidbody2D>().simulated = true;

        clone.GetComponent<Rigidbody2D>().simulated = false;
        GravityHandler.attractees.Remove(clone.GetComponent<Rigidbody2D>());
        GravityHandler.attractors.Remove(clone.GetComponent<Rigidbody2D>());
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        LineRenderer linePath = clone.GetComponent<LineRenderer>();
        linePath.enabled = false;

        throwableGameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
    }
}
