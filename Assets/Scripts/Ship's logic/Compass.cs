using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform Arrow;
    public Transform Ship;
    public float arrowDistance = 5;
    public Text distanceText;
    public float speed = .5f;

    private void Update()
    {
        Vector3 target = MissionTracker.Instance.Target.position;

        distanceText.text = Mathf.RoundToInt(Vector3.Distance(Ship.position, target)).ToString();
        var direction = (target - Ship.position) / (target - Ship.position).magnitude;
        Arrow.position = Ship.position + (direction.normalized * arrowDistance);

        Vector3 vectorToTarget = target - Ship.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Arrow.rotation = Quaternion.Slerp(Arrow.rotation, q, Time.deltaTime * speed);
    }
}
