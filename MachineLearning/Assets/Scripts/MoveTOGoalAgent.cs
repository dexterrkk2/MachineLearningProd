using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class MoveTOGoalAgent : Agent
{
    public Transform target;
    float moveSpeed = 3f;
    Vector3 startPosition;
    public Material winMaterial;
    public Material loseMaterial;
    public MeshRenderer floorMeshRenderer;
    public void Start()
    {
        startPosition = transform.localPosition;
    }
    public override void OnEpisodeBegin()
    {
        startPosition = new Vector3(Random.Range(-2f,10), startPosition.y, Random.Range(-2f, +5f));
        target.localPosition = new Vector3(Random.Range(-1,11), startPosition.y, Random.Range(-10f, 10));
        transform.localPosition = startPosition;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuosActions = actionsOut.ContinuousActions;
        continuosActions[0] = Input.GetAxisRaw("Horizontal");
        continuosActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            floorMeshRenderer.material = winMaterial;
            SetReward(+1f);
            EndEpisode();
        }
        else if(other.CompareTag("Wall"))
        {
            floorMeshRenderer.material = loseMaterial;
            SetReward(-1f);
            EndEpisode();
        }
    }
}
