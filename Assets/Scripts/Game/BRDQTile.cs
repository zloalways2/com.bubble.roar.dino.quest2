using System;
using UnityEngine;

namespace BRDQ
{
    public class BRDQTile : MonoBehaviour
    {
        public event Action<BRDQTile> BRDQOnTileClicked;
        
        [SerializeField] private ParticleSystem _BRDQParticles;
        [SerializeField] private Color _BRDQColorFx;
        
        [field: SerializeField] public EBRDQColor BRDQColor { get; private set; }
        
        public Vector2Int BRDQPosition { get; set; }
        public bool BRDQIsActive { get; set; } = true;

        private void OnMouseUpAsButton()
        {
            if (BRDQStaticUtil.BRDQStopUpdate)
                return;
            
            BRDQOnTileClicked?.Invoke(this);
        }

        public void BRDQDestroy()
        {
            if (BRDQStaticUtil.BRDQStopUpdate)
                return;
            
            var position = transform.position;
            position.z = -1f;
            var particle = Instantiate(_BRDQParticles, position, Quaternion.identity);
            var particleMain = particle.main;
            particleMain.startColor = _BRDQColorFx;
        }
    }
}