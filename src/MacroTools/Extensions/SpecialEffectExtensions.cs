﻿using WCSharp.Effects;
using static War3Api.Common;

namespace MacroTools.Extensions
{
  /// <summary>
  /// Provides a useful set of extension methods for Warcraft 3 effects,
  /// which are purely visual representations of game models.
  /// </summary>
  public static class SpecialEffectExtensions
  {
    /// <summary>
    /// Sets the size of the effect.
    /// </summary>
    /// <returns>The same effect that was provided.</returns>
    public static effect SetScale(this effect effect, float scale)
    {
      BlzSetSpecialEffectScale(effect, scale);
      return effect;
    }

    /// <summary>
    /// Sets the effect's yaw rotation instantly.
    /// </summary>
    /// <returns>The same effect that was provided.</returns>
    public static effect SetYaw(this effect effect, float yaw)
    {
      BlzSetSpecialEffectYaw(effect, yaw);
      return effect;
    }

    /// <summary>
    /// Causes the effect to be removed after the provided duration has elapsed.
    /// </summary>
    /// <returns>The same effect that was provided.</returns>
    public static effect SetLifespan(this effect effect, float lifespan = 0.03125F)
    {
      EffectSystem.Add(effect, lifespan);
      return effect;
    }
  }
}