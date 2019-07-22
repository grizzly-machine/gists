using System;
using UnityEngine;

namespace GrizzlyMachine
{
    public class BatchedParticleEmitter : MonoBehaviour
    {
        [Header("Emission")]
        [SerializeField] private float _emissionRate;
        [SerializeField] private bool _useTransformAsEmitterPosition;
        [SerializeField] private bool _applyEmitterShape;

        [Header("Emitter Overrides")]
        [SerializeField] private FloatOverride _angularVelocity;
        [SerializeField] private Vector3Override _angularVelocity3D;
        [SerializeField] private Vector3Override _axisOfRotation;
        [SerializeField] private UIntOverride _randomSeed;
        [SerializeField] private FloatOverride _rotation;
        [SerializeField] private Vector3Override _rotation3D;
        [SerializeField] private Color32Override _startColor;
        [SerializeField] private FloatOverride _startLifetime;
        [SerializeField] private FloatOverride _startSize;
        [SerializeField] private Vector3Override _startSize3D;
        [SerializeField] private Vector3Override _velocity;

        public float EmissionRate => _emissionRate;
        public float EmissionTimer { get; set; }

        public virtual ParticleSystem.EmitParams GetEmitParams(ParticleSystem targetSystem)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

            if (_useTransformAsEmitterPosition)
            {
                emitParams.position = ConvertWorldPointToSimulationSpace(transform.position, targetSystem);
            }
            emitParams.applyShapeToPosition = _applyEmitterShape;

            if (_angularVelocity.enabled) { emitParams.angularVelocity = _angularVelocity.value; }
            if (_angularVelocity3D.enabled) { emitParams.angularVelocity3D = _angularVelocity3D.value; }
            if (_axisOfRotation.enabled) { emitParams.axisOfRotation = _axisOfRotation.value; }
            if (_randomSeed.enabled) { emitParams.randomSeed = _randomSeed.value; }
            if (_rotation.enabled) { emitParams.rotation = _rotation.value; }
            if (_rotation3D.enabled) { emitParams.rotation3D = _rotation3D.value; }
            if (_startColor.enabled) { emitParams.startColor = _startColor.value; }
            if (_startLifetime.enabled) { emitParams.startLifetime = _startLifetime.value; }
            if (_startSize.enabled) { emitParams.startSize = _startSize.value; }
            if (_startSize3D.enabled) { emitParams.startSize3D = _startSize3D.value; }
            if (_velocity.enabled) { emitParams.velocity = _velocity.value; }

            return emitParams;
        }

        private static Vector3 ConvertWorldPointToSimulationSpace(Vector3 worldPos, ParticleSystem targetSystem)
        {
            ParticleSystem.MainModule mainModule = targetSystem.main;
            switch (mainModule.simulationSpace)
            {
                case ParticleSystemSimulationSpace.World:
                    return worldPos;

                case ParticleSystemSimulationSpace.Custom:
                    return mainModule.customSimulationSpace.InverseTransformPoint(worldPos);

                default:
                case ParticleSystemSimulationSpace.Local:
                    return targetSystem.transform.InverseTransformPoint(worldPos);
            }
        }

        [Serializable]
        public struct UIntOverride
        {
            public bool enabled;
            public uint value;
        }

        [Serializable]
        public struct FloatOverride
        {
            public bool enabled;
            public float value;
        }

        [Serializable]
        public struct Vector3Override
        {
            public bool enabled;
            public Vector3 value;
        }

        [Serializable]
        public struct Color32Override
        {
            public bool enabled;
            public Color32 value;
        }
    }
}