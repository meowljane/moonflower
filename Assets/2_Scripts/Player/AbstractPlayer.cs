using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour
{
    //Rigidbody, Animator
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public Animator anim;

    [Header("*[ Direct Assignment ]*")]
    //모바일 UI오브젝트
    public FixedJoystick joystick; //조이스틱 프리팹
    public GameObject webglBtn; //웹에서 사용하는 버튼

    //플레이어 상호작용 안내 Sprite
    public Sprite[] Sprite; //모바일,컴퓨터에 각각 다른 이미지 sprite
    public SpriteRenderer ConrirmOn;

    //상호작용을 위한 메테리얼
    public Material originalMaterial;
    public Material outlineMaterial;

    //상호작용되는 오브젝트 받아올 리스트
    [HideInInspector]
    public List<Collider2D> list = new List<Collider2D>();

    [HideInInspector]
    public string lastMapName; //맵 이동에 사용되는 변수
    [HideInInspector]
    public string currentMapName; //맵 이동에 사용되는 변수

    [HideInInspector]
    public int isSceneCount = 0; //사운드 관리 씬 카운트 변수
    [HideInInspector]
    public float playerSpeed = 150f;
    
    [HideInInspector]
    public Vector2 inputVec;

    [Header("*[ Boolen Type Variable ]*")]
    public bool canMove; //팝업창 On/Off에 따라 플레이어 이동을 제한하는 bool값
    //[HideInInspector]
    public bool isTransfer; //씬 이동을 체크하는 bool값
    //[HideInInspector]
    protected bool isMoblie; //모바일인지 아닌지 판단하는 bool값

    
    public abstract void Move(); //플레이어 이동 메서드 - Unity
    public abstract void JoystickMove(); //플레이어 이동 메서드 - WebGL(조이스틱)
    public abstract void AnimController(); //플레이어 애니메이션 회전 메서드
    public abstract void SetValues(); //플레이어 속성 값 세팅 메서드
    public abstract void ObjInteract(bool apply, Collider2D collision); //오브젝트 상호작용(색변환) 관리 메서드
}