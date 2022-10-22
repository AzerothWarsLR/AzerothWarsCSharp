using WCSharp.Events;

namespace AzerothWarsCSharp.MacroTools.QuestSystem.UtilityStructs
{
  /// <summary>
  ///   Completes when a unit casts a specific spell.
  /// </summary>
  public sealed class ObjectiveCastSpell : Objective
  {
    private readonly bool _holderOnly;

    /// <summary>
    ///   Completes when a unit casts a specific spell.
    /// </summary>
    /// <param name="spellId">The spell that needs to be casted for completion.</param>
    /// <param name="holderOnly">If true, the quest holder must cast the spell themselves.</param>
    public ObjectiveCastSpell(int spellId, bool holderOnly)
    {
      PlayerUnitEvents.Register(PlayerUnitEvent.SpellFinish, OnCast, spellId);
      if (holderOnly)
        Description = "Cast " + GetObjectName(spellId);
      else
        Description = "Anyone casts " + GetObjectName(spellId);

      _holderOnly = holderOnly;
    }

    /// <summary>
    ///   The unit that was used to complete the quest.
    /// </summary>
    public unit? Caster { get; private set; }

    private void OnCast()
    {
      if (Progress != QuestProgress.Complete &&
          (!_holderOnly || EligibleFactions.Contains(GetOwningPlayer(GetTriggerUnit()))))
      {
        Caster = GetTriggerUnit();
        Progress = QuestProgress.Complete;
      }
    }
  }
}