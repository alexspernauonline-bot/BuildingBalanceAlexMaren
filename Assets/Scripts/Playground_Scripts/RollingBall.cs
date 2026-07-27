using UnityEngine;

public class RollingBall : MonoBehaviour
{
    public class BallRoll : MonoBehaviour
    {
        public Transform moveBall;
        public float radius = 1.5f;

        private Vector3 lastPosition;

        void Start()
        {
            moveBall = transform.parent;

            //Speichern der aktuellen Position
            lastPosition = moveBall.position;
        }

        void LateUpdate()
        {
            //Ermittlung der in Update passierten Bewegung
            Vector3 movement = moveBall.position - lastPosition;

            if (movement.sqrMagnitude > 0.000001f)
            {
                //Achse, um die gedreht werden soll
                Vector3 axis = Vector3.Cross(Vector3.up, movement.normalized);

                //Anteil der Rotation basierend auf der Bewegung
                float angle = movement.magnitude / radius * Mathf.Rad2Deg;

                //Finale Rotation
                transform.Rotate(axis, angle, Space.World);
            }

            //Speichern der neuen Position
            lastPosition = moveBall.position;
        }
    }
}
