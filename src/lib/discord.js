'use strict'
const Discord = require('discord.js');
const config = require('../config/configuration');

/**
 * discord module
 * @module lib/discord
 * @class
 * helper module for the used discord.js functionality
 * 
 * only includes what the bot needs
 */
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
   * 
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
   * @param {String} [status='online'] - The discord status for the bot
   * @param {String} [type='PLAYING'] - What type of message
   * 
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

  /**
   * check if the message author is a bot
   * @param {Object} message - Message object from listener
   * 
   * returns boolean
   */
  isBot(message) {
    if (message.author.bot) return true;
    return false;
  }

  /**
   * reply to a message
   * @param {Object} message - Message object from listener
   * @param {String} content - The text content of the reply
   * @param {Boolean} [tts=false] - Switch for saying something in tts
   * 
   * returns a promise with the new message object
   */
  reply(message, content, tts=false) {
    return message.reply(content, { tts: tts });
  }

  /**
   * check if a message content includes a string
   * @param {Object} message - Message object from listener
   * @param {Array} strings - Array of strings
   * 
   * returns boolean
   */
  includes(message, strings) {
    let content = message.content.toLowerCase();
    let check = false;

    strings.forEach((string) => {
      if (content.includes(string)) {
        check = true;
        return;
      }
    });

    return check;
  }

  /**
   * Sends a new text message to the same channel
   * the message originated from
   * @param {Object} message - Message object from listener
   * @param {String} content - The text content of the new message
   * 
   * returns a promise with the new message object
   */
  send(message, content) {
    return message.channel.send(content)
  }
  
  /**
   * Sends a new message to the same channel
   * the message originated from
   * 
   * This message only includes an attachment
   * @param {Object} message - Message object from listener
   * @param {Object} attachment - the attachment object to send
   * @param {String|Buffer} attachment.file - The file content
   * @param {String} attachment.name - The filename
   * 
   * returns a promise with the new message object
   */
  sendAttachment(message, attachment) {
    return message.channel.send({
      files: [{
        attachment: attachment.file,
        name: attachment.name
      }]
    });
  }

  /**
   * searches the start of the message content
   * a specific string
   * @param {Object} message - Message object from listener
   * @param {String} string - the searchstring
   * 
   * returns boolean
   */
  startsWith(message, string) {
    let content = message.content;

    if (content.startsWith(string)) return true;
    return false;
  }
}