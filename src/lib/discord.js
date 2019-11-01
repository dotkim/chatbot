'use strict'
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

  /**
   * set the presence status for the bot
   * @param {String} name - The name of the "game"
   * @param {String} status='online' - The discord status for the bot
   * @param {String} type='PLAYING' - What type of message
   * returns true if it sets
   */
  setPresence(name, status = 'online', type = 'PLAYING') {
    if (!name) return;

    this.client.user
      .setPresence({
        status: status,
        game: {
          name: name,
          type: type
        }
      });

    return true;
  }
}