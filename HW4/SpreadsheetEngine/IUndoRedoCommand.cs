using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{

    /// <summary>
    /// Interface for commands to be executed on a spreadsheet.
    /// </summary>
    public interface IUndoRedoCommand
    {
        /// <summary>
        /// abstract interface method called exec, which is inherited by whatever uses this.
        /// </summary>
        /// <param name="spreadSheet">spreadsheet instance.</param>
        /// <returns>abstract something idk.</returns>
        IUndoRedoCommand Exec(Spreadsheet spreadSheet);
    }

    /// <summary>
    /// class for executing an array(or list) of commands.
    /// </summary>
    public class DoCommand
    {
        /// <summary>
        /// string containing our command name.
        /// </summary>
        public string CommandName;

        /// <summary>
        /// Array containing our commands.
        /// </summary>
        private IUndoRedoCommand[] commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoCommand"/> class.
        /// </summary>
        /// <param name="commands">array of the commands.</param>
        /// <param name="name">name of the command that was executed.</param>
        public DoCommand(IUndoRedoCommand[] commands, string name)
        {
            this.commands = commands;
            this.CommandName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoCommand"/> class.
        /// </summary>
        /// <param name="commands">list of commands.</param>
        /// <param name="name">name of the command that was executed.</param>
        public DoCommand(List<IUndoRedoCommand> commands, string name)
        {
            this.commands = commands.ToArray();
            this.CommandName = name;
        }

        /// <summary>
        /// Creates and returns an array containing a list of commands, and the name of the command.
        /// </summary>
        /// <param name="sheet">spreadSheet class instance.</param>
        /// <returns>(DoCommand)array of commands.</returns>
        public DoCommand Execute(Spreadsheet sheet)
        {
            List<IUndoRedoCommand> commandList = new List<IUndoRedoCommand>();

            foreach (IUndoRedoCommand command in this.commands)
            {
                commandList.Add(command.Exec(sheet));
            }

            DoCommand commandArray = new DoCommand(commandList.ToArray(), this.CommandName);

            return commandArray;
        }
    }
}
