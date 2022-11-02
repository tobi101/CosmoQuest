using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public Camera _camera;
    GameObject[] type1;
    GameObject[] type2;
    GameObject[] type3;
    GameObject[] type_col;
    GameObject endCanvas;

    float deltaMouseX;
    float deltaMouseY;

    Vector3 firstObjectPosition;

    bool isVertical;
    bool isCollision = false;

    AudioSource _audioSource;

    public void Start()
    {
        type1 = GameObject.FindGameObjectsWithTag("Type1");
        type2 = GameObject.FindGameObjectsWithTag("Type2");
        type3 = GameObject.FindGameObjectsWithTag("Type3");
        type_col = GameObject.FindGameObjectsWithTag("Collider");
        endCanvas = GameObject.FindGameObjectWithTag("EndCanvas");

        _audioSource = GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        delta = Vector2.zero;
        // Debug.Log(gameObject.transform.name.ToString() + "Object was dragged");
        foreach(GameObject obj in type1)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }

        foreach (GameObject obj in type2)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }

        foreach (GameObject obj in type3)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }

        foreach (GameObject obj in type_col)
        {
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        firstObjectPosition = gameObject.transform.position;

        deltaMouseX = Mathf.Abs(eventData.delta[0]);
        deltaMouseY = Mathf.Abs(eventData.delta[1]);

        if (deltaMouseX > deltaMouseY)
        {
            isVertical = false;
        }
        else
        {
            isVertical = true;
        }

        _audioSource.Play();
    }

    Vector2 delta;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newestMousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y));
        delta = eventData.delta;
        // Выпускаем лучи из взятого квадрата: вверх, вниз, вправо и влево.
        // Проверяем есть ли пересечение с объектами на расстоянии 1.7 от центра объекта.
        // Если есть, в ту сторону проход запрещен.

        if (isCollision == true)
        {
            OnEndDrag(eventData);
            Debug.Log("CHECK!");
            eventData.pointerDrag = null;
            isCollision = false;
            return;
        }

        if (isVertical == false)
        { 
            gameObject.transform.position = new Vector3(newestMousePosition.x, firstObjectPosition.y, 0);
        }
        else
        {
            gameObject.transform.position = new Vector3(firstObjectPosition.x, newestMousePosition.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log(gameObject.transform.name.ToString() + "Object was undragged");
        foreach (GameObject obj in type1)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        foreach (GameObject obj in type2)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        foreach (GameObject obj in type3)
        {
            if (gameObject.name != obj.name)
            {
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        foreach (GameObject obj in type_col)
        {
            obj.GetComponent<BoxCollider2D>().isTrigger = false;
        }

        StartCoroutine(WinCondition());
    }

    IEnumerator WinCondition()
    {
        yield return new WaitForSeconds(0.1f);

        int type1Counter = 0;
        int type2Counter = 0;
        int type3Counter = 0;

        foreach (GameObject obj in type1)
        {
            if (obj.transform.position.x > -3.215f & obj.transform.position.x < -3.195f) { type1Counter++; }
        }

        foreach (GameObject obj in type2)
        {
            if (obj.transform.position.x > -0.015f & obj.transform.position.x < 0.015f) { type2Counter++; }
        }

        foreach (GameObject obj in type3)
        {
            if (obj.transform.position.x > 3.195f & obj.transform.position.x < 3.215f) { type3Counter++; }
        }

        Debug.Log("Type1 done: " + type1Counter);
        Debug.Log("Type2 done: " + type2Counter);
        Debug.Log("Type3 done: " + type3Counter);

        if (type1Counter == 5 & type2Counter == 5 & type3Counter == 5)
        {
            endCanvas.GetComponent<CanvasGroup>().alpha = 1;
            endCanvas.GetComponent<CanvasGroup>().interactable = true;
            endCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.GetComponent<ClickManager>() != null | collider.gameObject.name.StartsWith("Block"))
        {
            Debug.Log("Collider: " + collider.gameObject.name);
            if ((isVertical == true & delta.y > 0 & transform.position.y < collider.transform.position.y) | 
                (isVertical == true & delta.y < 0 & transform.position.y > collider.transform.position.y) |
                (isVertical == false & delta.x > 0 & transform.position.x < collider.transform.position.x) |
                (isVertical == false & delta.x < 0 & transform.position.x > collider.transform.position.x))
            {
                isCollision = true;
            }
        }
    }
}
