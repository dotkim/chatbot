'use strict'
const Command = require('../models/Command');
const CommandParser = require('../components/CommandParser');

/**
 * Class for building command objects, objects are used for passing a command into another class
 */
class CommandFactory {
  /**
   * Parse the message for a command.
   * @param {String} content - Content from the discord message, usually message.content
   * @returns {Command} represents a new instance of a command, this has a schema property with the command schema.
   * @example
   * const CommandFactory = require('./commandFactory');
   * const factory = new CommandFactory();
   */
  parse(content) {
    const parse = CommandParser(content);

    const data = {
      name: parse.groups.name,
      prefix: parse.groups.prefix
    };

    return new Command(data).schema;
  }
}

module.exports = CommandFactory;
