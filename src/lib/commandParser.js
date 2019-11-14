'use strict'

const CommandParser = function CommandParser(message) {
  const regex = /^(?<prefix>.)(?<name>\w+)$/giu;
  return regex.exec(message);
}

module.exports = CommandParser;