using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float fireCooldown = 0.3f;

    private float nextFireTime;

    private void OnEnable()
    {
        fireAction.action.Enable();
    }

    private void OnDisable()
    {
        fireAction.action.Disable();
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (fireAction.action.WasPressedThisFrame() && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireCooldown;

        GameObject bulletObject = bulletPool.Get(firePoint.position, Quaternion.identity);
        if (bulletObject.TryGetComponent(out Bullet bullet))
        {
            bullet.Launch(bulletPool, playerController.FacingDirection);
        }
    }
}
