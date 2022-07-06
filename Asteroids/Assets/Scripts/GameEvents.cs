using System.Collections;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] private IEvent[] events;
    [SerializeField] private int minTime, maxTime;
    private int time;

   
    private void Start()
    {
        events = new IEvent[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            events[i] = transform.GetChild(i).GetComponent<IEvent>();
            events[i].gameEvents = this;
        }

        time = Random.Range(minTime, maxTime);
        StartCoroutine(DecreaseTime());
    }

    public void OnEventStop()
    {        
        StartCoroutine(DecreaseTime());
    }
    private void StartEvent()
    {
        int randomEvent = Random.Range(0, events.Length - 1);
        Debug.Log("Event " + events[randomEvent].ToString());
        events[randomEvent].eventTransform.gameObject.SetActive(true);
        events[randomEvent].StartEvent();
    }

    private IEnumerator DecreaseTime()
    {
        yield return new WaitForSeconds(1);
        time--;

        if (time == 0)
        {
            time = Random.Range(minTime, maxTime);
            StartEvent();
            yield break;
        }

        StartCoroutine(DecreaseTime());
    } 


}
