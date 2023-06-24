using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleHelper
{
    public static void ChangeShaderTexture(Transform particle, string nameField, Texture texture)
    {
        var particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetTexture(nameField, texture);

        particleRenderer.material = newMat;
    }

    public static void ChangeShaderTexture(Transform particle, string name, string nameField, Texture texture)
    {
        var particleRenderer = particle.transform.Find(name).GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetTexture(nameField, texture);

        particleRenderer.material = newMat;
    }

    public static void ChangeShaderColor(Transform particle, string nameField, Color color)
    {
        var particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetColor(nameField, color);

        particleRenderer.material = newMat;
    }

    public static void ChangeShaderColor(Transform particle, string name, string nameField, Color color)
    {
        var particleRenderer = particle.transform.Find(name).GetComponent<ParticleSystemRenderer>();
        var originalMat = particleRenderer.material;
        var newMat = new Material(originalMat);

        newMat.SetColor(nameField, color);

        particleRenderer.material = newMat;
    }

    public static void ChangeParticleColor(Transform particle, Color color)
    {
        var mainModuleSparks = particle.GetComponent<ParticleSystem>().main;
        mainModuleSparks.startColor = color;
    }

    public static void ChangeParticleColor(Transform particle, string name, Color color)
    {
        var mainModuleSparks = particle.Find(name).GetComponent<ParticleSystem>().main;
        mainModuleSparks.startColor = color;
    }
}
