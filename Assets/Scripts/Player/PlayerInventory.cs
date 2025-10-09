using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerInventory : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private GameObject prefabBomb;
    private Rigidbody rbBomb;
    [SerializeField] private Transform targetFloating;
    private const float smoothFloat = 1f;
    private bool isBombFloating = false;
    private Vector3 velocityFloating;
    private bool isAlreadyBomb;
    private bool bombFlag;

    private float maxTimeToCreateBomb = 5f;
    private float timerToCreateBomb = 0f    ;

    [Header("Scraps")]
    [SerializeField]    
    private int scraps = 0;
    [SerializeField]
    private int maxScraps = 20;
    [Header("Flowers")]
    [SerializeField]
    private int flowers = 0;
    [SerializeField]
    private int maxFlowers = 10;

    private void Start()
    {
        UIManager.Instance.SetScraps—ount(scraps, maxScraps);
        UIManager.Instance.SetFlowers—ount(flowers, maxFlowers);

        InputManager.Instance.OnAltAttackStarted += StartCreateBomb;
        InputManager.Instance.OnAltAttackCanceled += CancelCreateBomb;
        InputManager.Instance.OnAttackStarted += ThrowBomb;

        EventManager.Instance.OnAddScarps += AddScraps;
        EventManager.Instance.OnAddFlowers += AddFlowers;
    }

    private void AddScraps(int addScraps)
    {
        scraps = Mathf.Clamp(scraps + addScraps, 0, maxScraps);
        UIManager.Instance.SetScraps—ount(scraps, maxScraps);
    }

    private void AddFlowers(int addFlowers)
    {
        flowers = Mathf.Clamp(flowers + addFlowers, 0, maxFlowers);
        UIManager.Instance.SetFlowers—ount(flowers, maxFlowers);
    }

    private void RmvScraps(int rmvScraps)
    {
        scraps = Mathf.Clamp(scraps - rmvScraps, 0, maxScraps);
        UIManager.Instance.SetScraps—ount(scraps, maxScraps);
    }

    private void RmvFlowers(int rmvFlowers)
    {
        flowers = Mathf.Clamp(flowers - rmvFlowers, 0, maxFlowers);
        UIManager.Instance.SetFlowers—ount(flowers, maxFlowers);
    }
    
    private void ProcessCreateBomb()
    {
        if (!bombFlag) return;
        if (isAlreadyBomb) return;
        timerToCreateBomb += Time.deltaTime;
        UIManager.Instance.ProcessTimerCreatBomb(timerToCreateBomb, maxTimeToCreateBomb);

        if(timerToCreateBomb >= maxTimeToCreateBomb) CreateBomb();
    }

    private void StartCreateBomb()
    {
        if (!IsEnoughScrapsAndFlowers()) return;
        timerToCreateBomb = 0f;
        UIManager.Instance.HideTimerCrearBomb(true);
        bombFlag = true;
    }

    private void CancelCreateBomb()
    {
        if (isAlreadyBomb) Destroy(rbBomb.gameObject);
        UIManager.Instance.HideTimerCrearBomb(false);
        bombFlag = false;
        isAlreadyBomb = false;
    }

    private void CreateBomb()
    {
        UIManager.Instance.HideTimerCrearBomb(false);
        bombFlag = false;
        isAlreadyBomb = true;
        GameObject objBomb = Instantiate(prefabBomb, targetFloating.position, Camera.main.transform.rotation);
        rbBomb = objBomb.GetComponent<Rigidbody>();
        objBomb.transform.parent = targetFloating;
        rbBomb.GetComponent<Collider>().isTrigger = true;
        rbBomb.isKinematic = false;
        rbBomb.useGravity = false;
        isBombFloating = true;
        isAlreadyBomb = true;
    }

    private void ThrowBomb()
    {
        if (!isAlreadyBomb) return;
        isAlreadyBomb = false;
        isBombFloating = false;
        rbBomb.isKinematic = false;
        rbBomb.useGravity = true;
        rbBomb.GetComponent<Collider>().isTrigger = false;
        rbBomb.linearDamping = 1f;
        rbBomb.AddRelativeForce(Vector3.forward * 7f, ForceMode.Impulse);
        rbBomb.GetComponent<DarkBox>().Use();
        rbBomb.transform.parent = null;
        rbBomb = null;
        RmvScraps(5);
        RmvFlowers(3);
        
    }
    private bool IsEnoughScrapsAndFlowers()
    {
        return scraps >= 5 && flowers >= 3;
    }

    //[Header("Bomb")]
    //[SerializeField] private GameObject prefabBomb;
    //[SerializeField] private Rigidbody rbBomb;
    //[SerializeField] private Transform targetFloating;
    //[SerializeField] private const float smoothFloat = 1f;
    //[SerializeField] private bool isBombFloating = false;
    //private Vector3 velocityFloating;
    //private bool isAlreadyBomb;

    //[Header("Scrap")]
    //[SerializeField]    
    //private int scrap = 0;
    //[SerializeField]
    //private int maxScrap = 5;
    //[Header("Flowers")]
    //[SerializeField]
    //private int flowers = 0;
    //[SerializeField]
    //private int maxFlowers = 15;


    //private void Start()
    //{
    //    UIManager.Instance.SetScrap—ount(scrap, maxScrap);

    //    InputManager.Instance.OnAttackStarted += ThrowBomb;

    //    EventManager.Instance.OnAddScarps += AddScarp;
    //    EventManager.Instance.OnTryCreateBomb += TryCreateBomb;
    //}
    private void Update()
    {
        ProcessCreateBomb();
        if (!rbBomb) return;
        ProcessFloatingBob(rbBomb.transform.position, targetFloating.position, rbBomb.transform.rotation, Camera.main.transform.rotation);
       
    }

    private void ProcessFloatingBob(Vector3 pos1, Vector3 pos2, Quaternion rot1, Quaternion rot2)
    {
        if (!isBombFloating) return;
        rbBomb.transform.position = Vector3.Lerp(pos1, pos2, smoothFloat);
        //if(Vector3.Distance(pos1, pos2) > 0.1f)
        //{
        //   Vector3 moveDir = pos2 - pos1;
        //   rbBomb.AddForce(moveDir * 10f);
        //}
        rbBomb.rotation = Quaternion.Lerp(rot1, rot2, smoothFloat);
    }

    //public void ThrowBomb()
    //{
    //    if (!rbBomb) return;
    //    isAlreadyBomb = false;
    //    isBombFloating = false;
    //    rbBomb.isKinematic = false;
    //    rbBomb.useGravity = true;
    //    rbBomb.GetComponent<Collider>().isTrigger = false;
    //    rbBomb.linearDamping = 1f;
    //    rbBomb.AddRelativeForce(Vector3.forward * 7f, ForceMode.Impulse);
    //    rbBomb.GetComponent<DarkBox>().Use();
    //    rbBomb.transform.parent = null;
    //    rbBomb = null;

    //}

    //private bool TryCreateBomb()
    //{
    //    if (!IsEnoughScrap()) return false;
    //    if(isAlreadyBomb) return false;
    //    RemoveScarp(5);
    //    RemoveScarp(3);
    //    GameObject objBomb = Instantiate(prefabBomb, targetFloating.position, Camera.main.transform.rotation);
    //    rbBomb = objBomb.GetComponent<Rigidbody>();
    //    objBomb.transform.parent = targetFloating;
    //    rbBomb.GetComponent<Collider>().isTrigger = true;
    //    rbBomb.isKinematic = false;//
    //    rbBomb.useGravity = false;
    //    isBombFloating = true;
    //    isAlreadyBomb = true;
    //    return true;
    //}

    //private bool IsEnoughScrap()
    //{
    //    return scrap >= 5 && flowers >= 3;
    //}

    //private void AddScarp(int add)
    //{
    //    scrap = Mathf.Clamp(scrap + add, 0, maxScrap);
    //    UIManager.Instance.SetScrap—ount(scrap, maxScrap);
    //}

    //private void RemoveScarp(int rmv)
    //{
    //    scrap -= rmv;
    //    UIManager.Instance.SetScrap—ount(scrap, maxScrap);
    //}

    //private void AddFlowers(int add)
    //{
    //    scrap = Mathf.Clamp(scrap + add, 0, maxScrap);
    //    UIManager.Instance.SetScrap—ount(scrap, maxScrap);
    //}

    //private void RemoveFlowers(int rmv)
    //{
    //    scrap -= rmv;
    //    UIManager.Instance.SetScrap—ount(scrap, maxScrap);
    //}
}
