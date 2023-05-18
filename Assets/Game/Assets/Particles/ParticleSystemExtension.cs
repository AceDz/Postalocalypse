using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public static class ParticleSystemExtension
    {
        public static Tween DOStartColor(this ParticleSystem effect, Color to, float duration)
        {
            return DOTween.To(() => effect.startColor, (Color color) =>
            {
                effect.startColor = color;
            }, to, duration);
        }

        public static Tween DOParticlesColor(this ParticleSystem effect, Color to, float duration)
        {
            return DOTween.To(() => effect.startColor, (Color color) =>
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[effect.particleCount];
                effect.GetParticles(particles);
                for (int i = 0; i < particles.Length; ++i)
                {
                    particles[i].color = color;
                }

            }, to, duration);
        }
    }
}