'use strict'
const CommandFactory = require('../api/CommandFactory');
const PrefixChecker = require('../config/prefixes');
const cmdfactory = new CommandFactory();

class Worker {
  constructor(bot) {
    this.bot = bot;
  }

  ParseMessage(message) {
    if (this.bot.isBot(message)) return;
    try {
      const command = cmdfactory.parse(message.content);
      const factoryPrefix = PrefixChecker(command.prefix);
      const Factory = require(`../api/${factoryPrefix}Factory`);
      const factory = new Factory();
      const data = factory.get(command.name);
      return data;
    } catch (err) {
      //eslint-disable-next-line no-console
      console.error(err);
    }
  }
}

module.exports = Worker;