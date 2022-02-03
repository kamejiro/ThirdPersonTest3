using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�Q�ƒ�`
    public GameObject cam;
    Quaternion cameraRot;
    Quaternion characterRot;

    //�ϐ���`
    float x, z;//�ړ��̓���
    float speed = 0.1f;//�ړ����x
    float Xsensityvity = 3f, Ysensityvity = 3f;//���x
    bool cursorLock = true;//�J�[�\���̏��
    float minX = -90f, maxX = 90f;//�p�x����

    //����������
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    //�X�V����1
    //
    void Update()
    {
        //�}�E�X�̓���
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
        cameraRot = ClampRotation(cameraRot);

        //�}�E�X�̓��͌��ʂ��J�����ɔ��f
        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        //�J�[�\���̏�Ԃ��X�V
        UpdateCursorLock();
    }

    //�X�V����2
    //�v���C���[�̓���𔽉f
    private void FixedUpdate()
    {
        x = 0;
        z = 0;
        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;
        transform.position += cam.transform.forward * z + cam.transform.right * x;
    }

    //�J�[�\���̏�Ԃ��X�V����֐�
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

    //�p�x�𐧌�����֐�(�I�C���[�j�ɒ����Ax�̂�clamp���Ă܂�quaternion�ɒ����B)
    public Quaternion ClampRotation(Quaternion q)
    {
        //w�Ŋ���
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;
        //�I�C���[�ɒ���
        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;
        //�N�����v����
        angleX = Mathf.Clamp(angleX, minX, maxX);
        //���ɖ߂�
        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
