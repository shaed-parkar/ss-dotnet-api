﻿namespace SS.Commands.Core;

public interface ICommandHandlerFactory
{
    ICommandHandler<TCommand> Create<TCommand>(TCommand command) where TCommand : ICommand;
}