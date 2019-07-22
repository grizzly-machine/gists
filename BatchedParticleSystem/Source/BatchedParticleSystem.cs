using System.Collections.Generic;
using UnityEngine;

namespace GrizzlyMachine
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BatchedParticleSystem : MonoBehaviour
    {
        [SerializeField] private List<BatchedParticleEmitter> _emitters;

        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            for (int i = 0; i < _emitters.Count; ++i)
            {
                if (_emitters[i] == null) { continue; }

                int emitCount = Mathf.FloorToInt(_emitters[i].EmissionTimer);
                if (emitCount > 0)
                {
                    for (int e = 0; e < emitCount; ++e)
                    {
                        ParticleSystem.EmitParams emitParams = _emitters[i].GetEmitParams(_particleSystem);
                        _particleSystem.Emit(emitParams, 1);
                    }

                    _emitters[i].EmissionTimer %= 1f;
                }
                else
                {
                    _emitters[i].EmissionTimer += _emitters[i].EmissionRate * Time.deltaTime;
                }
            }
        }
    }
}