using DG.Tweening;
using HealthSystem;
using UnityEngine;

namespace WeaponsSystem
{
    public class FluteCircle : MonoBehaviour
    {
        [SerializeField] private float sizeUpTime;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private DamageZone damageZone;

        public void SizeUp(float size, int damage)
        {
            damageZone.SetDamage(damage);
            damageZone.SetDeactivationTime(sizeUpTime);
            damageZone.SetZoneActive(true, DamageSourceType.FLUTE);
            transform.DOScale(new Vector3(size, size, size), sizeUpTime);
            spriteRenderer.DOColor(new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0),
                sizeUpTime).onComplete += () => Destroy(gameObject); //TODO: Change to object pool
        }
    }
}