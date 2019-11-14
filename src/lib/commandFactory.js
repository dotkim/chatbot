'use strict'
const Command = require('./command');
const CommandParser = require('./commandParser');

class CommandFactory {
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