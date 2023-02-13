using UnityEngine;

[ExecuteAlways]
public class TestPlatform : MonoBehaviour
{
    [SerializeField] private Transform _plate;
    [SerializeField] private float _radius;
    [SerializeField] private float _grade;

    private void Update()
    {
        _plate.position = GetPosition(_grade, _radius);
        _plate.rotation = GetRotation(_grade);
    }

    private Quaternion GetRotation(in float grade) =>
        Quaternion.Euler(0, -grade - 22.5f, 0);

    private Vector3 GetPosition(float byArcGrade, float radius)
    {
        float posX = Mathf.Cos(byArcGrade * Mathf.Deg2Rad) * radius;
        float posZ = Mathf.Sin(byArcGrade * Mathf.Deg2Rad) * radius;
        return new Vector3(posX, 0, posZ);
    }
}
