﻿using System.Collections.Generic;
using MacroTools.Extensions;
using MacroTools.Utils;
using WCSharp.Shared.Data;


namespace MacroTools.DummyCasters
{
  /// <summary>A dummy caster that has been predefined to only ever cast one specific instant ability.</summary>
  public sealed class AbilitySpecificDummyCaster
  {
    private readonly unit _unit;
    private readonly int _abilityTypeId;
    private readonly int _abilityOrderId;
    
    internal AbilitySpecificDummyCaster(unit unit, int abilityTypeId, int abilityOrderId)
    {
      _unit = unit;
      _abilityTypeId = abilityTypeId;
      _abilityOrderId = abilityOrderId;
    }
    
    /// <summary>
    /// Causes the specified ability to be cast from the specified object at the specified target.
    /// </summary>
    public void CastUnit(unit caster, int level, unit target, DummyCastOriginType originType)
    {
      var originPoint = originType == DummyCastOriginType.Caster ? caster.GetPosition() : target.GetPosition();
      _unit.SetOwner(caster.Owner);
      _unit.SetPosition(originPoint);
      _unit.AddAbility(_abilityTypeId);
      _unit.SetAbilityLevel(_abilityTypeId, level);

      if (originType == DummyCastOriginType.Caster)
        _unit.FacePosition(target.GetPosition());

      _unit.IssueOrder(_abilityOrderId, target);
    }

    public void CastNoTarget(unit caster, int level)
    {
      _unit.SetOwner(caster.Owner);
      _unit.SetPosition(caster.GetPosition());
      _unit.AddAbility(_abilityTypeId);
      _unit.SetAbilityLevel(_abilityTypeId, level);
      _unit.IssueOrder(_abilityOrderId);
    }

    /// <summary>
    /// Causes the specified spell to be cast on a particular point.
    /// </summary>
    public void CastNoTargetOnUnit(unit caster, int level, unit target)
    {
      _unit.SetOwner(caster.Owner);
      _unit.SetPosition(target.GetPosition());
      _unit.AddAbility(_abilityTypeId);
      _unit.SetAbilityLevel(_abilityTypeId, level);
      _unit.IssueOrder(_abilityOrderId);
    }

    /// <summary>
    /// Causes the specified spell to be cast at a particular point.
    /// </summary>
    public void CastPoint(player whichPlayer, int level, Point target)
    {
      _unit.SetOwner(whichPlayer);
      _unit.SetPosition(target);
      _unit.AddAbility(_abilityTypeId);
      _unit.SetAbilityLevel(_abilityTypeId, level);
      _unit.IssueOrderOld(_abilityOrderId, target);
    }

    /// <summary>
    /// Causes the specified spell to be cast on all units in a circle.
    /// </summary>
    public void CastOnUnitsInCircle(unit caster, int level, Point center,
      float radius, DummyCasterManager.CastFilter castFilter, DummyCastOriginType originType)
    {
      foreach (var target in GroupUtils
                 .GetUnitsInRange(center, radius)
                 .FindAll(unit => castFilter(caster, unit)))
      {
        CastUnit(caster, level, target, originType);
      }
    }
    
    /// <summary>
    /// Causes the specified spell to be cast on all units in a group.
    /// </summary>
    public void CastOnTargets(unit caster, int level, IEnumerable<unit> targets,
      DummyCastOriginType originType)
    {
      foreach (var target in targets) 
        CastUnit(caster, level, target, originType);
    }
  }
}