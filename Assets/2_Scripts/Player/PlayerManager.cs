using System.Collections; //���� namespace
using System.Collections.Generic;//���� namespace
using UnityEngine; //���� namespace
using UnityEngine.SceneManagement; //���� namespace

[ExecuteInEditMode]
public class PlayerManager : AbstractPlayer
{
    public static PlayerManager instance;

    //������Ʈ�� ����� �Ŷ�
    /// <summary>
    /// �����ֱ��Լ�
    /// </summary>
    #region
    void Awake()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL�� �ƴ� Unity�϶��� ���̽�ƽ off / unity �̵��Լ� ���
        isMoblie = false;
        ConrirmOn.sprite = Sprite[0];
#endif
        // WebGL�̸鼭 ������϶�
        if (Application.isMobilePlatform)
        {
            joystick.enabled = true;
            webglBtn.SetActive(true);
            isMoblie = true;
            ConrirmOn.sprite = Sprite[1];
        }
        //WebGL�̸鼭 ��ǻ�� �϶�
        else
        {
            isMoblie = false;
            ConrirmOn.sprite = Sprite[0];
        }

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        SetValues();
    }
    void Update()
    {
        AnimController();

        //�����ڸ�� �ν���
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //    playerSpeed = 150f;
        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //    playerSpeed = 1100f;
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            if (isMoblie)
            {
                JoystickMove();
            }

            else
            {
                Move();
            }
        }
    }
    #endregion


    /// <summary>
    /// �÷��̾� OnTrigger
    /// </summary>
    /// <param name="collision"></param>

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Interaction" || collision.tag == "OnlyOneTouch") && (list.Count == 0 || list.Count == 1))
        {
            if (!list.Contains(collision))
                list.Add(collision);

            if (list[0] == collision)
                ObjInteract(true, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interaction" || collision.tag == "OnlyOneTouch" || collision.tag == "NeverTouch")
        {
            if (list.Count == 0)
            {
                Debug.Log("����Ʈ�� �����Ƿ� ����");
                return;
            }

            if (list[0] == collision)
            {
                ObjInteract(false, list[0]);
                list.RemoveAt(0);
                if (list.Count == 1)
                {
                    ObjInteract(true, list[0]);
                }
            }
            if (list.Count == 2 && list[1] == collision)
            {
                Debug.Log("����Ʈ1 ����");
                list.RemoveAt(1);
            }
        }
    }
    #endregion

    /// <summary>
    /// �÷��̾� �̵� ���� �߻�޼���
    /// playerSpeed������ �̵��ӵ� ���� ����
    /// </summary>
    #region
    public override void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }

    public override void JoystickMove()
    {
        inputVec.x = joystick.Horizontal;
        inputVec.y = joystick.Vertical;

        Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }
    #endregion

    /// <summary>
    /// �÷��̾� �ִϸ��̼� ���� �߻�޼���
    /// </summary>
    #region
    public override void AnimController()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // �¿� Ű �Է��� ���� ��
            if (inputVec.x != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirX", inputVec.x);
                anim.SetFloat("DirY", 0);
            }
            // ���� Ű �Է��� ���� ��
            else if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }

        //�¿� ����Ű�� ���� ��
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)
             || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            inputVec.x = 0;
        }

        //���� ����Ű�� ���� ��
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            inputVec.y = 0;
        }

        if ((inputVec.x == 0 && inputVec.y == 0) || !canMove)
        {
            anim.SetBool("Walking", false);
        }

        else if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            if (inputVec.x != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirX", inputVec.x);

            }
            // ���� Ű �Է��� ���� ��
            if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }
    }


    #endregion


    /// <summary>
    /// �÷��̾ �ٸ� ������Ʈ�� ��ȣ�ۿ��ϴ� �߻�޼���
    /// ������Ʈ�� ��ȣ�ۿ�� isTriggerüũ�Ǿ��ִ� ������Ʈ�� ���� ���� �޼���
    /// </summary>
    /// <param name="apply"></param>
    /// <param name="collision"></param>
    public override void ObjInteract(bool apply, Collider2D collision)
    {
        if (collision != null)
        {
            Renderer rend = collision.GetComponent<Renderer>();
            if (rend != null) //���� ������Ʈ�� renderer�� �����Ѵٸ�
            {
                //UpdateOutline(apply, rend);
                if (apply) //OnTriggerEnter�� ���� true�̸鼭 HashSet�� �ƹ��͵� ������
                {
                    rend.material = outlineMaterial; //OutLine ���׸��� ����
                    Debug.Log("�� ���ϴ���");
                }

                else //OnTriggerEnter�� ���� false Ȥ�� HashSet�� ���� �ִٸ�
                {
                    rend.material = originalMaterial;
                    Debug.Log("�� ���ϴ���");
                }
            }
        }
    }

    /// <summary>
    /// �÷��̾� �Ӽ� �� �����ϴ� �߻�޼���
    /// </summary>
    #region
    public override void SetValues()
    {
        canMove = true;
        //playerSpeed = 150f; //�����ӵ� 150f;
    }
    #endregion
}