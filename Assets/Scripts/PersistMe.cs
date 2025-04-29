using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistMe : MonoBehaviour
{
     void Awake() => DontDestroyOnLoad(gameObject);
}
