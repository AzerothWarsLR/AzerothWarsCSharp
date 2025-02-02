using System.Collections.Generic;
using MacroTools.DummyCasters;
using MacroTools.SpellSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;
using System.Linq;
using System;

namespace MacroTools.Spells
{
  public sealed class MassAnySpell : Spell
  {
    public int DummyAbilityId { get; init; }

    public int DummyAbilityOrderId { get; init; }

    public float Radius { get; init; }

    public required DummyCasterManager.CastFilter CastFilter { get; init; }

    public SpellTargetType TargetType { get; init; } = SpellTargetType.None;

    public DummyCasterType DummyCasterType { get; init; } = DummyCasterType.Global;
    public float Chance { get; init; } = 1.0f;
    /// <summary>
    /// Determines where the spell's dummmy units spawn when they cast <see cref="DummyAbilityId"/>.
    /// </summary>
    public DummyCastOriginType DummyCastOriginType { get; init; } = DummyCastOriginType.Target;

    public MassAnySpell(int id) : base(id)
    {
    }

    public override void OnCast(unit caster, unit target, Point targetPoint)
    {
      var center = TargetType == SpellTargetType.None ? new Point(GetUnitX(caster), GetUnitY(caster)) : targetPoint;
      var units = GetUnitsInRadius(center, Radius, CastFilter);

      var filteredUnits = units.Where(u => GetRandomReal(0, 1) <= Chance).ToList();
      Console.WriteLine($"Total units in radius: {units.Count()}");
      Console.WriteLine($"Units affected by chance: {filteredUnits.Count()}");
      if (DummyCasterType == DummyCasterType.Global)
      {
        DummyCasterManager.GetGlobalDummyCaster().CastOnUnitsInCircle(caster, DummyAbilityId, DummyAbilityOrderId, GetAbilityLevel(caster),
            center, Radius, (c, t) => filteredUnits.Contains(t), DummyCastOriginType);
      }
      else if (DummyCasterType == DummyCasterType.AbilitySpecific)
      {
        DummyCasterManager.GetAbilitySpecificDummyCaster(DummyAbilityId, DummyAbilityOrderId)
            .CastOnUnitsInCircle(caster, GetAbilityLevel(caster), center, Radius, (c, t) => filteredUnits.Contains(t), DummyCastOriginType);
      }
    }

    private IEnumerable<unit> GetUnitsInRadius(Point center, float radius, DummyCasterManager.CastFilter castFilter)
    {
      var group = CreateGroup();
      GroupEnumUnitsInRange(group, center.X, center.Y, radius, null);
      var units = new List<unit>();
      unit u;

      while ((u = FirstOfGroup(group)) != null)
      {
        GroupRemoveUnit(group, u);
        if (castFilter(null, u)) // Adjusted to match the expected signature
        {
          units.Add(u);
        }
      }

      DestroyGroup(group);
      return units;
    }
  }
}