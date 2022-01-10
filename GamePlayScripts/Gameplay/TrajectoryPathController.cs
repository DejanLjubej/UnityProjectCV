using UnityEngine;

public class TrajectoryPathController : MonoBehaviour
{
    public static int indexOfBounce;
    public static bool[] triggeredIndicators;
    
    public static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps, float velocityIterationModifier = 30f)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations * velocityIterationModifier;
        Vector2 moveStep = velocity * timestep;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
        for (int i = 0; i < steps; ++i)
        {
                moveStep += gravityAccel;
                pos += moveStep;
                results[i] = pos;
        }
        return results;
    }

    public static void BouncePlot(int number)
    {
        indexOfBounce = number;
    }

    void Awake()
    {
        indexOfBounce = int.MaxValue;
    }
}
