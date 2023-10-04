using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDisplayAnimation : MonoBehaviour
{
    public IEnumerator DelayAnimation(int position) 
    {
        yield return new WaitForSeconds(0.25f * position);
        gameObject.GetComponent<Animator>().enabled = true;
    }
}
