using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropContrrol : MonoBehaviour
{
    private Vector2 originalPosition;
    public GameObject particleSys;

    public GameObject bot;
    private bool isOverBot = false;

    public bool isOver = false;

    private void Start()
    {
        originalPosition = transform.position; //lưu vị trí hiện tại của game object vào cái biến mình mới tạo
    }

    private void Update()
    {
        if (isOver == false)
        {
            CheckIfOverBot();
            if (isOverBot)
            {
                bot.GetComponent<Animator>().speed = 1; //Tiếp tục chạy animation
            }
            else
            {
                bot.GetComponent<Animator>().speed = 0; //Dừng animation lại
            }
        }
    }

    private void OnMouseDown() //Gọi khi player nhấn chuột xuống - 1 lần
    {
        Debug.Log("đổi lại sprite thành túi bột đang đổ");
        if (particleSys != null)
        {
            particleSys.SetActive(true);
        }
    }

    private void OnMouseDrag() //Gọi khi player kéo chuột khắp màn hình - liên tục
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos); //Di chuyển vật theo con trỏ chuột


        //if bột đang chạm vào jam => order in layer của bột sẽ set thành 20
    }

    private void OnMouseUp() //Gọi khi player thả chuột ra - 1 lần
    {
        transform.position = originalPosition; //set vị trí game object về đúng chỗ của cái biến mình đã lưu
        Debug.Log("đổi lại sprite thành túi bột dựng đứng ban đầu");

        if (particleSys != null)
        {
            particleSys.SetActive(false);
        }
        //order in layer của bột sẽ set thành 0
    }

    private void CheckIfOverBot()
    {
        Collider2D bowlCol = bot.GetComponent<Collider2D>();
        if (bowlCol != null)
        {
            isOverBot = bowlCol.OverlapPoint(transform.position);
        }
    }
}
