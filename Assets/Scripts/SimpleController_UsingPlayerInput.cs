using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

// Use a separate PlayerInput component for setting up input.
// I took all of the movement controls from the input system tutorial and edited them slightly
public class SimpleController_UsingPlayerInput : MonoBehaviour
{
    public GameObject capsule;
    public Manager manager;
    public float moveSpeed;
    public float rotateSpeed;

    private Vector2 m_Rotation;
    private Vector2 m_Look;
    private Vector2 m_Move;

    public float totalIllumination;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Look(m_Look);
        Move(m_Move);
        //Check lighting approximation
        if (CalculateLightExposure() > .01f)
            manager.Hunting();
        else
            manager.StopHunting();
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        capsule.transform.position += move * scaledMoveSpeed;
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }

    private float CalculateLightExposure()
    {
        float currentLightAmount = 0f;
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in lights)
        {
            if (light.enabled)
            {
                Vector3 direction = light.transform.position - transform.position;
                float distance = direction.magnitude;
                direction.Normalize();

                if (distance < light.range)
                {
                    float strength = 1.0f - (distance / light.range);

                    // Raycast to check for walls
                    if (Physics.Raycast(light.transform.position, -direction, out RaycastHit hit, distance))
                    {
                        Debug.DrawRay(light.transform.position, -direction * hit.distance, Color.red);
                        //Check if it hit player
                        if (hit.transform == capsule.transform)
                        {
                            currentLightAmount += light.intensity * strength;
                        }
                    }
                }
            }
        }
        return currentLightAmount;
    }
}
