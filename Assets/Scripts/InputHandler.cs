using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake() 
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (GameManager.Instance.currentPopper == GameManager.PopperType.Single)
        {
            var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
            if (!rayHit.collider) return;

            Debug.Log(rayHit.collider.gameObject.name);

            GameManager.Instance.DamageBubble(rayHit.collider.gameObject, true);
        }
        else
        {
            var results =  Physics2D.OverlapCircleAll(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) , 
                20);

            foreach (var result in results)
            {
                GameManager.Instance.DamageBubble(result.gameObject, true);
                Debug.Log(result.gameObject.name);
            }
        }
        
    }

    public void OnNext()
    {
        GameManager.Instance.currentPopper = GameManager.PopperType.Multiple;
    }

    public void OnPrevious()
    {
        GameManager.Instance.currentPopper = GameManager.PopperType.Single;
    }
}
