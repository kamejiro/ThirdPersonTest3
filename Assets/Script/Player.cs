using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //参照定義
    public GameObject cam;
    Quaternion cameraRot;
    Quaternion characterRot;

    //変数定義
    float x, z;//移動の入力
    float speed = 0.1f;//移動速度
    float Xsensityvity = 3f, Ysensityvity = 3f;//感度
    bool cursorLock = true;//カーソルの状態
    float minX = -90f, maxX = 90f;//角度制限

    //初期化処理
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    //更新処理1
    //
    void Update()
    {
        //マウスの入力
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
        cameraRot = ClampRotation(cameraRot);

        //マウスの入力結果をカメラに反映
        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        //カーソルの状態を更新
        UpdateCursorLock();
    }

    //更新処理2
    //プレイヤーの動作を反映
    private void FixedUpdate()
    {
        x = 0;
        z = 0;
        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;
        transform.position += cam.transform.forward * z + cam.transform.right * x;
    }

    //カーソルの状態を更新する関数
    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //角度を制限する関数(オイラー核に直し、xのみclampしてまたquaternionに直す。)
    public Quaternion ClampRotation(Quaternion q)
    {
        //wで割る
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;
        //オイラーに直す
        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;
        //クランプする
        angleX = Mathf.Clamp(angleX, minX, maxX);
        //元に戻す
        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
