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
            var parentParticle = attacks[i].transform;

            ParticleHelper.ChangeShaderColor(parentParticle, "_AddColor", slash);
            ParticleHelper.ChangeShaderTexture(parentParticle, "_EmissionTex", slashTexture);

            if (i > 2)
            {
                ParticleHelper.ChangeShaderColor(parentParticle, "Slash2", "_AddColor", slash);
                ParticleHelper.ChangeShaderTexture(parentParticle, "Slash2", "_EmissionTex", slashTexture);

                ParticleHelper.ChangeShaderColor(parentParticle, "Slash3", "_AddColor", slash);
                ParticleHelper.ChangeShaderTexture(parentParticle, "Slash3", "_EmissionTex", slashTexture);

                ParticleHelper.ChangeParticleColor(parentParticle, "Fire", fire);
            }
            else
            {
                ParticleHelper.ChangeParticleColor(parentParticle, "Smoke", smoke);
            }

            ParticleHelper.ChangeParticleColor(parentParticle, "Sparks", spark);
            ParticleHelper.ChangeParticleColor(parentParticle, "Hit", hit);

            var attackHit = parentParticle.Find("Hit");

            ParticleHelper.ChangeShaderColor(attackHit, "_Color", hit);

            ParticleHelper.ChangeParticleColor(attackHit, "Flare", sparksCore);
            ParticleHelper.ChangeParticleColor(attackHit, "SparksCore", sparksCore);
        }
    }
}
