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
    //����� UI������Ʈ
    public FixedJoystick joystick; //���̽�ƽ ������
    public GameObject webglBtn; //������ ����ϴ� ��ư

    //�÷��̾� ��ȣ�ۿ� �ȳ� Sprite
    public Sprite[] Sprite; //�����,��ǻ�Ϳ� ���� �ٸ� �̹��� sprite
    public SpriteRenderer ConrirmOn;

    //��ȣ�ۿ��� ���� ���׸���
    public Material originalMaterial;
    public Material outlineMaterial;

    //��ȣ�ۿ�Ǵ� ������Ʈ �޾ƿ� ����Ʈ
    [HideInInspector]
    public List<Collider2D> list = new List<Collider2D>();

    [HideInInspector]
    public string lastMapName; //�� �̵��� ���Ǵ� ����
    [HideInInspector]
    public string currentMapName; //�� �̵��� ���Ǵ� ����

    [HideInInspector]
    public int isSceneCount = 0; //���� ���� �� ī��Ʈ ����
    [HideInInspector]
    public float playerSpeed = 150f;
    
    [HideInInspector]
    public Vector2 inputVec;

    [Header("*[ Boolen Type Variable ]*")]
    public bool canMove; //�˾�â On/Off�� ���� �÷��̾� �̵��� �����ϴ� bool��
    //[HideInInspector]
    public bool isTransfer; //�� �̵��� üũ�ϴ� bool��
    //[HideInInspector]
    protected bool isMoblie; //��������� �ƴ��� �Ǵ��ϴ� bool��

    
    public abstract void Move(); //�÷��̾� �̵� �޼��� - Unity
    public abstract void JoystickMove(); //�÷��̾� �̵� �޼��� - WebGL(���̽�ƽ)
    public abstract void AnimController(); //�÷��̾� �ִϸ��̼� ȸ�� �޼���
    public abstract void SetValues(); //�÷��̾� �Ӽ� �� ���� �޼���
    public abstract void ObjInteract(bool apply, Collider2D collision); //������Ʈ ��ȣ�ۿ�(����ȯ) ���� �޼���
}