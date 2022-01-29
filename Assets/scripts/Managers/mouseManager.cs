using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

public class mouseManager : MonoBehaviour
{
    public static mouseManager Instance;
    public Texture2D point, doorway, attack, target, arrow;
    RaycastHit hitInfo;
    public event Action<Vector3> onMouseClicked;

    void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture() //切换鼠标贴图
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hitInfo))
        {
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                        break;
            }
        }
    }

    void MouseControl()
    {
        
        if (Input.GetMouseButtonDown(0)&&hitInfo.collider!=null)
        {
            
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                onMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
}

