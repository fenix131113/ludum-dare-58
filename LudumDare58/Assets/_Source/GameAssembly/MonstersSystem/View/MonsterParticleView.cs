using EntitySystem.Entities;
using HealthSystem;
using MonstersSystem;
using System;
using System.Linq;
using UnityEngine;

public class MonsterParticleView : MonoBehaviour
{
    [SerializeField] private MonsterVision vision;
    [SerializeField] private HealthEntity healthEntity;
    [SerializeField] private SpriteRenderer vfx;
    [SerializeField] private float startDistance = 5f;
    [SerializeField] private float fullVisibilityDistance = 0.3f;
    [SerializeField] private float maxOpacity = 0.1f;

    private void Start()
    {
        var startColor = vfx.color;
    }
    private void Update()
    {
        if (vision.CanSeeTarget)
        {
            float distance = Vector3.Distance(vision.Target.position, transform.position);
            UpdateParticleOpacity(distance);
        }
    }

    private void UpdateParticleOpacity(float distance)
    {
        float opacity = 0f;

        if (distance < fullVisibilityDistance)
        {
            opacity = maxOpacity;
        }
        else if (distance < startDistance)
        {
            opacity = Mathf.Lerp(maxOpacity, 0f, (distance - fullVisibilityDistance) / (startDistance - fullVisibilityDistance));
        }
        Color color = SelectColor();
        color.a = opacity;
        vfx.color = color;

        if (opacity > 0)
        {
            vfx.gameObject.SetActive(true);
        }
    }

    private Color SelectColor()
    {
        switch (healthEntity.VulnerabilityType) //TODO: Move to scriptable object
        {
            case DamageSourceType.VACUUM_CLEANER:
                return new Color(0, 101, 255);
            case DamageSourceType.CAMERA:
                return new Color(255, 254, 0);
            case DamageSourceType.FLUTE:
                return Color.white;
            default:
                return new Color(0, 101, 255);
        }
    }
}
