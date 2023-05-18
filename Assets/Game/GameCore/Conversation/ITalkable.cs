using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITalkable
{
    void BeginTalk(string text, System.Action onComplete);
    void EndTalk(System.Action onComplete);
    void ContinueTalk();
}
