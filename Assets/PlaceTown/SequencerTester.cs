using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SequencerTester : MonoBehaviour
{
    public Sequencer Tasker;
    public Sequencer Tasker2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Tasker.Enqueue(ProcessAttack, 20, 200);
            Tasker.Enqueue(ProcessAttack, 30, 100);
            Tasker.Enqueue(ProcessAttack, 40, 100);
            Tasker.Enqueue(ProcessAttack, 50, 100);
            Tasker.Enqueue(ProcessAttack, 60, 100);
            Tasker.Enqueue(ProcessAttack, 70, 100);
            Tasker.Enqueue(ProcessAttack, 80, 100);
            Tasker2.Enqueue(CancelFirstSequence, 299);
        }

        if (Input.GetKeyDown(KeyCode.L))
            Tasker.Enqueue(Looper, Tasker, Tasker2, 22);

        if (Input.GetKeyDown(KeyCode.K))
        {
            Tasker.EscapeSequence();
            Tasker2.EscapeSequence();
        }    
    }

    async Task CancelFirstSequence(int d)
    {
        await Task.Delay(d);
        Tasker.EscapeSequence();
    }

    async Task ProcessAttack(int n, int d)
    {
        await Task.Delay(d);
        Debug.Log("attacked target for " + n);
    }

    async Task Looper(Sequencer a, Sequencer b, int c)
    {
        await Task.Delay(300);
        Debug.Log(c);
    }
}
