using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct ComboSection
{
    public TargetScript Target;
    public float Time;

    public void Play()
        => Target.StartCountdown(Time);
}

[Serializable]
public struct Combo
{
    public List<ComboSection> comboSections;

    public IEnumerator Play()
    {
        foreach (var section in comboSections)
        {
            section.Play();
            yield return new WaitForSeconds(section.Time);
        }
    }
}

public class CombosScript : MonoBehaviour
{
    [SerializeField] private List<Combo> combos;

    public void LaunchCombo()
        => LaunchCombo(Random.Range(0, combos.Count));

    public void LaunchCombo(int combo)
    {
        StartCoroutine(combos[combo].Play());
    }
}
