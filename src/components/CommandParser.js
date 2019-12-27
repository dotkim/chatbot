'use strict'

/**
 * Component parser for commands, this uses a regex to check if the beginning of the message has a prefix.
 * @param {String} message - Content from the discord message.
 * @returns {Object<RegExpExecArray>} Represents an executed regex string, containing the name and prefix properties.
 */
const CommandParser = function CommandParser(message) {
  const regex = /^(?<prefix>.)(?<name>\w+)$/giu;
  const match = regex.exec(message);
  if (!match) throw new Error('Command could not be parsed correctly');
  return match;
}

module.exports = CommandParser;