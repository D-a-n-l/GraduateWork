using UnityEngine;

public class CableComponent : MonoBehaviour
{
	#region Class members
	[Header("Main config")]

	[SerializeField, Space(5)] 
	private Transform endPoint;

	[SerializeField, Space(5)] 
	private Material cableMaterial;

	[Header("Cable config")]
	// Cable config
	[SerializeField, Space(5)] 
	private float cableLength = 0.5f;

	[SerializeField, Space(5)] 
	private int totalSegments = 5;

	[SerializeField, Space(5)] 
	private float segmentsPerUnit = 2f;

	[SerializeField, Space(5)] 
	private float cableWidth = 0.1f;

	[Header("Solver config")]
	// Solver config
	[SerializeField, Space(5)] 
	private int verletIterations = 1;

	[SerializeField, Space(5)] 
	private float solverIterations = 1;

	//[Range(0,3)]
	[SerializeField, Space(5)] 
	private float stiffness = 1f;

	private LineRenderer line;

	private CableParticle[] points;

	private int segments = 0;

	private float segmentLength;

	#endregion


	#region Initial setup

	private void Start()
	{
		InitCableParticles();
		InitLineRenderer();

		segmentLength = cableLength / segments;
	}

	/**
	 * Init cable particles
	 * 
	 * Creates the cable particles along the cable length
	 * and binds the start and end tips to their respective game objects.
	 */
	private void InitCableParticles()
	{
		// Calculate segments to use
		if (totalSegments > 0)
			segments = totalSegments;
		else
			segments = Mathf.CeilToInt (cableLength * segmentsPerUnit);

		Vector3 cableDirection = (endPoint.position - transform.position).normalized;
		float initialSegmentLength = cableLength / segments;
		points = new CableParticle[segments + 1];

		// Foreach point
		for (int pointIdx = 0; pointIdx <= segments; pointIdx++) {
			// Initial position
			Vector3 initialPosition = transform.position + (cableDirection * (initialSegmentLength * pointIdx));
			points[pointIdx] = new CableParticle(initialPosition);
		}

		// Bind start and end particles with their respective gameobjects
		CableParticle start = points[0];
		CableParticle end = points[segments];

		start.Bind(this.transform);
		end.Bind(endPoint.transform);
	}

	/**
	 * Initialized the line renderer
	 */
	private void InitLineRenderer()
	{
		if (gameObject.GetComponent<LineRenderer>() == false)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
			line.startWidth = cableWidth;
			line.endWidth = cableWidth;
			line.positionCount = segments + 1;
			line.material = cableMaterial;
			//line.GetComponent<Renderer>().enabled = true; Зачем?
		}
    }

	#endregion


	#region Render Pass

	private void Update()
	{
		RenderCable();
	}

	/**
	 * Render Cable
	 * 
	 * Update every particle position in the line renderer.
	 */
	private void RenderCable()
	{
		for (int pointIdx = 0; pointIdx < segments + 1; pointIdx++) 
		{
			line.SetPosition(pointIdx, points[pointIdx].Position);
		}
	}

	#endregion


	#region Verlet integration & solver pass

	private void FixedUpdate()
	{
		for (int verletIdx = 0; verletIdx < verletIterations; verletIdx++) 
		{
			VerletIntegrate();
			SolveConstraints();
		}
	}

	/**
	 * Verler integration pass
	 * 
	 * In this step every particle updates its position and speed.
	 */
	private void VerletIntegrate()
	{
		Vector3 gravityDisplacement = Time.fixedDeltaTime * Time.fixedDeltaTime * Physics.gravity;

		foreach (CableParticle particle in points) 
		{
			particle.UpdateVerlet(gravityDisplacement);
		}
	}

	/**
	 * Constrains solver pass
	 * 
	 * In this step every constraint is addressed in sequence
	 */
	private void SolveConstraints()
	{
		// For each solver iteration..
		for (int iterationIdx = 0; iterationIdx < solverIterations; iterationIdx++) 
		{
			SolveDistanceConstraint();
			SolveStiffnessConstraint();
		}
	}

	#endregion


	#region Solver Constraints

	/**
	 * Distance constraint for each segment / pair of particles
	 **/
	private void SolveDistanceConstraint()
	{
		//float segmentLength = cableLength / segments;

		for (int SegIdx = 0; SegIdx < segments; SegIdx++) 
		{
			CableParticle particleA = points[SegIdx];
			CableParticle particleB = points[SegIdx + 1];

			// Solve for this pair of particles
			SolveDistanceConstraint(particleA, particleB, segmentLength);
		}
	}
		
	/**
	 * Distance Constraint 
	 * 
	 * This is the main constrains that keeps the cable particles "tied" together.
	 */
	private void SolveDistanceConstraint(CableParticle particleA, CableParticle particleB, float segmentLength)
	{
		// Find current vector between particles
		Vector3 delta = particleB.Position - particleA.Position;
		// 
		float currentDistance = delta.magnitude;
		float errorFactor = (currentDistance - segmentLength) / currentDistance;
		
		// Only move free particles to satisfy constraints
		if (particleA.IsFree() && particleB.IsFree()) 
		{
			particleA.Position += errorFactor * 0.5f * delta;
			particleB.Position -= errorFactor * 0.5f * delta;
		} 
		else if (particleA.IsFree()) 
		{
			particleA.Position += errorFactor * delta;
		} 
		else if (particleB.IsFree()) 
		{
			particleB.Position -= errorFactor * delta;
		}
	}

	/**
	 * Stiffness constraint
	 **/
	private void SolveStiffnessConstraint()
	{
		float distance = (points[0].Position - points[segments].Position).magnitude;

		if (distance > cableLength) 
		{
			foreach (CableParticle particle in points) 
			{
				SolveStiffnessConstraint(particle, distance);
			}
		}	
	}

	/**
	 * TODO: I'll implement this constraint to reinforce cable stiffness 
	 * 
	 * As the system has more particles, the verlet integration aproach 
	 * may get way too loose cable simulation. This constraint is intended 
	 * to reinforce the cable stiffness.
	 * // throw new System.NotImplementedException ();
	 **/
	private void SolveStiffnessConstraint(CableParticle cableParticle, float distance)
	{
	

	}

	#endregion
}
