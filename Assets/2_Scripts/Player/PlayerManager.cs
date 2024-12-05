using System.Collections; //기존 namespace
using System.Collections.Generic;//기존 namespace
using UnityEngine; //기존 namespace
using UnityEngine.SceneManagement; //기존 namespace

[ExecuteInEditMode]
public class PlayerManager : AbstractPlayer
{
    public static PlayerManager instance;

    //업데이트좀 제대로 돼라
    /// <summary>
    /// 생명주기함수
    /// </summary>
    #region
    void Awake()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
        isMoblie = false;
        ConrirmOn.sprite = Sprite[0];
#endif
        // WebGL이면서 모바일일때
        if (Application.isMobilePlatform)
        {
            joystick.enabled = true;
            webglBtn.SetActive(true);
            isMoblie = true;
            ConrirmOn.sprite = Sprite[1];
        }
        //WebGL이면서 컴퓨터 일때
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

        //관리자모드 부스터
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
    /// 플레이어 OnTrigger
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
                Debug.Log("리스트에 없으므로 종료");
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
                Debug.Log("리스트1 빼기");
                list.RemoveAt(1);
            }
        }
    }
    #endregion

    /// <summary>
    /// 플레이어 이동 관리 추상메서드
    /// playerSpeed값으로 이동속도 조절 가능
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
    /// 플레이어 애니메이션 관리 추상메서드
    /// </summary>
    #region
    public override void AnimController()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // 좌우 키 입력이 있을 때
            if (inputVec.x != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirX", inputVec.x);
                anim.SetFloat("DirY", 0);
            }
            // 상하 키 입력이 있을 때
            else if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }

        //좌우 방향키를 땠을 때
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)
             || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            inputVec.x = 0;
        }

        //상하 방향키를 땠을 때
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
            // 상하 키 입력이 있을 때
            if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }
    }


    #endregion


    /// <summary>
    /// 플레이어가 다른 오브젝트랑 상호작용하는 추상메서드
    /// 오브젝트랑 상호작용시 isTrigger체크되어있는 오브젝트의 색상 변경 메서드
    /// </summary>
    /// <param name="apply"></param>
    /// <param name="collision"></param>
    public override void ObjInteract(bool apply, Collider2D collision)
    {
        if (collision != null)
        {
            Renderer rend = collision.GetComponent<Renderer>();
            if (rend != null) //닿은 오브젝트의 renderer가 존재한다면
            {
                //UpdateOutline(apply, rend);
                if (apply) //OnTriggerEnter의 값이 true이면서 HashSet에 아무것도 없을때
                {
                    rend.material = outlineMaterial; //OutLine 메테리얼 적용
                    Debug.Log("색 변하는중");
                }

                else //OnTriggerEnter의 값이 false 혹은 HashSet에 값이 있다면
                {
                    rend.material = originalMaterial;
                    Debug.Log("색 변하는중");
                }
            }
        }
    }

    /// <summary>
    /// 플레이어 속성 값 세팅하는 추상메서드
    /// </summary>
    #region
    public override void SetValues()
    {
        canMove = true;
        //playerSpeed = 150f; //기존속도 150f;
    }
    #endregion
}