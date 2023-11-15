using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public class RockEffector : MonoBehaviour
    {
        [SerializeField] private Rock _rock;
        [SerializeField] private ParticleSystem _dust;

        private void OnEnable()
        {
            _rock.Destroyed += OnActivated;
        }

        private void OnDisable()
        {
            _rock.Destroyed -= OnActivated;
        }

        private void OnActivated()
        {
            _dust.Play();
        }
    }
}