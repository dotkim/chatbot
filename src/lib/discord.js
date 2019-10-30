const Discord = require('discord.js');
const config = require('../config/configuration');

module.exports = class {
  /**
   * creates a new instance of the discord client
   * and sets it to this.client
   * @constructor
   */
  constructor() {
    this.client = new Discord.Client();
    return this.client;
  }

  /**
   * creates the bot session
   * uses the token from config
   * if token is undefined (default) the app quits
   * returns void
   */
  login() {
    if (!config.token) {
      console.error('Discord token is undefined, please provide a token');
      process.exit(1);
    }
    else {
      client.login(config.token);
    }
  }
}