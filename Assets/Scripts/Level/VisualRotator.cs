using UnityEngine;

public class VisualRotator : MonoBehaviour {
    [SerializeField] private float rotationSpeed;

    private Transform visual;

    private void Awake() {
        visual = transform.Find("Visual");
    }

    private void Update() {
        visual.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}