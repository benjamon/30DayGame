using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bentendo;
using NUnit.Framework;
using System;
using tt = UnityEngine.TestTools;

public class VarTests
{
    [Test(Description = "ActVar triggers action only on value change")]
    public void ActVarOnlyActsOnValueChange()
    {
        var crnt = 1;
        var calls = 0;
        Actvar<int> value = new Actvar<int>(crnt);
        value.onChanged = (int n) => { crnt = n; calls++; };
        Assert.AreEqual((int)value, crnt);
        value.crnt  = 5;
        Assert.AreEqual((int)value, crnt);
        Assert.AreEqual(calls, 1);
        value.crnt = 5;
        Assert.AreEqual(calls, 1);
        Assert.AreEqual((int)value, crnt);
    }

    [Test(Description = "SubVar only triggers action only on value change")]
    public void SubVarOnlyInvokesOnValueChange()
    {
        var crnt = 1;
        var calls = 0;
        Subvar<int> value = new Subvar<int>(crnt);
        value.onChanged.AddListener((int n) => { crnt = n; calls++; });
        Assert.AreEqual((int)value, crnt);
        value.Value = 5;
        Assert.AreEqual((int)value, crnt);
        Assert.AreEqual(calls, 1);
        value.Value = 5;
        Assert.AreEqual(calls, 1);
        Assert.AreEqual((int)value, crnt);
    }
}
