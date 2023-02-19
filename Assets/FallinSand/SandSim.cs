using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;

public class SandSim : MonoBehaviour
{
    public int size;

    public Renderer output;

    int pcount;
    int step;

    private NativeArray<byte> pixels;
    private NativeArray<byte> rules;
    private Color[] pixelColors;
    private Texture2D texture;

    Coroutine Simulation;

    public Color[] Colors = new Color[]
    {
        new Color(0f,0f,0f),
        new Color(0f,0f,1f),
        new Color(1f,1f,1f),
        new Color(1f,0f,1f),
    };

    private void Awake()
    {
        pcount = size * size;
        rules = new NativeArray<byte>(256, Allocator.Persistent);
        pixels = new NativeArray<byte>(pcount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        rules[1] = 1;
        rules[2] = 2;
        rules[3] = 3;
        pixelColors = new Color[pcount];
        texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;
        output.material.mainTexture = texture;
        Restart();
    }

    private void Restart()
    {
        for (int i = 0; i < pcount; i++)
        {
            float roll = Random.Range(0f, 1f);
            pixels[i] = (byte)((roll > .5f + Mathf.Sin(((i%size)/(float)size) * Mathf.PI * Random.Range(1f, 26f)) * .15f) ? ((roll > .7f) ? ((roll > .85f) ? 2 : 3) : 1) : 0);
        }
        DrawSim();
    }

    private IEnumerator Simulate()
    {
        while (true)
        {
            yield return new WaitForSeconds(.02f);
            StepSim();
            DrawSim();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Simulation == null)
                Simulation = StartCoroutine(Simulate());
            else
            {
                StopCoroutine(Simulation);
                Simulation = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);
            Vector2 v = ((Vector2)Input.mousePosition) / new Vector2(Screen.width, Screen.height);
            Debug.Log(v);
            int x = Mathf.RoundToInt(v.x * (float)size);
            int y = Mathf.RoundToInt(v.y * (float)size);
            for (int i = 0; i < size; i++)
                pixels[size * i + x] = 0;
            for (int i = 0; i < size; i++)
                pixels[size *  y+ i] = 0;

            DrawSim();
        }
    }

    //make async
    void StepSim()
    {
        SimulateJob job = new SimulateJob
        {
            pixels = pixels,
            rules = rules,
            size = size,
            step = step,
        };

        JobHandle handle = job.Schedule();

        handle.Complete();

        step++;
    }

    //replace with compute shader blit
    void DrawSim()
    {
        for (int i = 0; i < pcount; i++) pixelColors[i] = Colors[rules[pixels[i]]];
        texture.SetPixels(pixelColors);
        texture.Apply();
    }

    private void OnDestroy()
    {
        rules.Dispose();
        pixels.Dispose();
        Destroy(texture);
    }
}
