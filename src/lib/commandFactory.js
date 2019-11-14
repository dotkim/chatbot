'use strict'
const Command = require('./command');
const CommandParser = require('./commandParser');

/**
 * Class for building command objects, objects are used for passing a command into another class
 */
class CommandFactory {
  /**
   * Parse the message for a command.
   * @param {String} content - Content from the discord message, usually message.content
   * @returns {Command} represents a new instance of a command, this has a schema property with the command schema.
   */
  parse(content) {
    const parse = CommandParser(content);
    const command = {
      name: parse.groups.name,
      prefix: parse.groups.prefix
    };

    if (!command) throw new Error('Command was not parsed correctly');

    return new Command(command);
  }
}

module.exports = CommandFactory;