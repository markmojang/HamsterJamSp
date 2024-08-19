using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDisplay : MonoBehaviour
{
    public Pattern pat;
    public int bulletct;
    public float angle;
    void Start()
    {
        bulletct = pat.bulletct;
        angle = pat.angle;
    }
}
