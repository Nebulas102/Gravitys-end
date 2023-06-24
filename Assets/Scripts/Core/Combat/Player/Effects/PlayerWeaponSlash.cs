using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UnityEngine;


public class PlayerWeaponSlash : MonoBehaviour
{
    public ParticleSystem[] attacks;

    public void PlaySlash(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < attacks.Length && !attacks[attackIndex].isPlaying)
        {
            attacks[attackIndex].Play();
        }
    }

    public void SetSlashStyle(Color slash, Texture slashTexture, Color smoke, Color spark, Color hit, Color sparksCore, Color fire)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            ChangeShaderColor(i, "_AddColor", slash);
            ChangeShaderTexture(i, "_EmissionTex", slashTexture);

            if (i > 2)
            {
                ChangeShaderColor(i, "Slash2", "_AddColor", slash);
                ChangeShaderTexture(i, "Slash2", "_EmissionTex", slashTexture);

                ChangeShaderColor(i, "Slash3", "_AddColor", slash);
                ChangeShaderTexture(i, "Slash3", "_EmissionTex", slashTexture);

                ChangeParticleColor(i, "Fire", fire);
            }
            else
            {
                ChangeParticleColor(i, "Smoke", smoke);
            }

            ChangeParticleColor(i, "Sparks", spark);
            ChangeParticleColor(i, "Hit", hit);

            var attackHit = attacks[i].transform.Find("Hit");
            attackHit.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", hit);

            ChangeParticleColor("Flare", sparksCore, attackHit);
            ChangeParticleColor("SparksCore", sparksCore, attackHit);
        }
    }

    private void ChangeShaderTexture(int id, string nameField, Texture texture)
    {
        var particleRenderer = attacks[id].GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetTexture(nameField, texture);

        particleRenderer.material = newMat;
    }

    private void ChangeShaderTexture(int id, string name, string nameField, Texture texture)
    {
        var particleRenderer = attacks[id].transform.Find(name).GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetTexture(nameField, texture);

        particleRenderer.material = newMat;
    }

    private void ChangeShaderColor(int id, string nameField, Color color)
    {
        var particleRenderer = attacks[id].GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetColor(nameField, color);

        particleRenderer.material = newMat;
    }

    private void ChangeShaderColor(int id, string name, string nameField, Color color)
    {
        var particleRenderer = attacks[id].transform.Find(name).GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetColor(nameField, color);

        particleRenderer.material = newMat;
    }

    private void ChangeParticleColor(int id, string name, Color color)
    {
        var mainModuleSparks = attacks[id].transform.Find(name).GetComponent<ParticleSystem>().main;
        mainModuleSparks.startColor = color;
    }

    private void ChangeParticleColor(string name, Color color, Transform particleObject)
    {
        var mainModuleSparks = particleObject.Find(name).GetComponent<ParticleSystem>().main;
        mainModuleSparks.startColor = color;
    }
}
