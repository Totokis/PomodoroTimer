using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest
{
 
    [UnityTest]
    public IEnumerator PlayModeTestWithEnumeratorPasses()
    {
        yield return null;
        Assert.AreEqual(0,0);
    }
}
