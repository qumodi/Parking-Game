using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    [SerializeField] private LayerMask _carLayerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.GamePaused)
        {
            CheckCarClick();
        }
    }

    void CheckCarClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, _carLayerMask))
            {
                Debug.Log(rayHit.collider.name);
                rayHit.collider.GetComponentInParent<Car>()?.OnClick.Invoke();
            }
        }

        if(Input.touches.Length == 0){
            return;
        }
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began){
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, _carLayerMask))
            {
                Debug.Log(rayHit.collider.name);
                rayHit.collider.GetComponentInParent<Car>()?.OnClick.Invoke();
            }
        }
    }
}