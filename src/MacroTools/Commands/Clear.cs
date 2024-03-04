﻿using MacroTools.CommandSystem;


namespace MacroTools.Commands
{
  /// <summary>
  /// A <see cref="CommandSystem.Command"/> that clears all messages from the player's screen.
  /// </summary>
  public sealed class Clear : Command
  {
    private readonly string _commandText;

    /// <summary>
    /// Initializes a new instance of the <see cref="Clear"/> class.
    /// </summary>
    public Clear(string commandText)
    {
      _commandText = commandText;
    }
    
    /// <inheritdoc />
    public override string CommandText => _commandText;
    
    /// <inheritdoc />
    public override bool Exact => true;
  
    /// <inheritdoc />
    public override int MinimumParameterCount => 0;

    /// <inheritdoc />
    public override CommandType Type => CommandType.Normal;

    /// <inheritdoc />
    public override string Description => "Clears all text from your screen.";

    /// <inheritdoc />
    public override string Execute(player cheater, params string[] parameters)
    {
      if (GetLocalPlayer() == cheater)
        ClearTextMessages();
      return "Clearing messages.";
    }
  }
}