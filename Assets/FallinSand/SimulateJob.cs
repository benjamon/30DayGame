using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public struct SimulateJob : IJob
{
    public NativeArray<byte> pixels;
    public NativeArray<byte> rules;
    public int size;
    public int step;

    public void Execute()
    {
        int pcount = size * size;
        byte d, p, rd, rp;
        int s = step / 8;
        for(int ri = 0; ri < pcount; ri++)
        {
            int i = ri;
            //alternating scan lines
            if ((i / size + s) % 2 == 0) i = (ri / size) * size + size - 1 - (ri % size);

            p = pixels[i];
            rp = rules[p];

            switch (rp)
            {
                case 1:
                    if (i >= size)
                    {
                        d = pixels[i - size];
                        rd = rules[d];
                        if (rd < rp)
                        {
                            pixels[i - size] = p;
                            pixels[i] = d;
                            break;
                        }
                        if (i % size != 0)
                        {
                            d = pixels[i - size - 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - size - 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        }
                        if (i % size != size - 1) // not right-most
                        {
                            d = pixels[i - size + 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - size + 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        }
                    }
                    if ((i + step) % 2 != 0)
                    {
                        if (i % size != 0)
                        {
                            d = pixels[i - 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        }
                    }
                    if (i % size != size-1)
                    {
                        d = pixels[i + 1];
                        rd = rules[d];
                        if (rd < rp)
                        {
                            pixels[i + 1] = p;
                            pixels[i] = d;
                            break;
                        }
                    }
                    if ((i + step) % 2 == 0)
                    {
                        if (i % size != 0)
                        {
                            d = pixels[i - 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                    if (i >= size)
                    {
                        d = pixels[i - size];
                        rd = rules[d];
                        if (rd < rp)
                        {
                            pixels[i - size] = p;
                            pixels[i] = d;
                            break;
                        } 
                        if (i % size != size - 1) // not right-most
                        {
                            d = pixels[i - size + 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - size + 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        } 
                        if (i % size != 0)
                        {
                            d = pixels[i - size - 1];
                            rd = rules[d];
                            if (rd < rp)
                            {
                                pixels[i - size - 1] = p;
                                pixels[i] = d;
                                break;
                            }
                        }
                    }
                    break;
                case 3:
                    if (i >= size)
                    {
                        d = pixels[i - size];
                        rd = rules[d];
                        if (rd < rp)
                        {
                            pixels[i - size] = p;
                            pixels[i] = d;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
