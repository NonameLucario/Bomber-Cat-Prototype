using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    public Transform msg—ontainer;
    public GameObject msgPrefab;

    private void Awake()
    {
        instance = this;
        SendMsg("CNSL", "Hello!");
    }

    public void SendMsg(string _who, string _msg)
    {
        GameObject newMsgObg = Instantiate(msgPrefab, msg—ontainer);
        newMsgObg.GetComponent<MessageUI>().Init($"{_who}: {_msg}");
    }
}
