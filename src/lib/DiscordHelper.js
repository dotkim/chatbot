'use strict'
const Discord = require('discord.js');
const config = require('../config/Configuration');

/**
 * Discord module
 * @class
 * Helper module for the used discord.js functionality, this only includes what the bot needs
 */
class DiscordHelper {
  /**
   * Creates a new instance of the discord client and sets it to this.client
   * @constructor
   */
  constructor() {
    this.client = new Discord.Client();
  }

  /**
   * Creates the bot session, uses the token from config. If token is undefined (default) the app throws a new error
   */
  login() {
    if (!config.token) {
      throw new Error('Discord token is undefined, please provide a token')
    }

    this.client.login(config.token);
  }

  /**
   * Set the presence status for the bot
   * @param {String} name - The name of the "game"
   * @param {String} [status='online'] - The discord status for the bot
   * @param {String} [type='PLAYING'] - What type of message
   * @returns {Boolean} True if presence was set
   */
  setPresence(name, status = 'online', type = 'PLAYING') {
    if (!name) return;

    this.client.user.
      setPresence({
        status,
        game: {
          name,
          type
        }
      });

    return true;
  }

  /**
   * Check if the message author is a bot
   * @param {Object} message - Message object from listener
   * @returns {Boolean} True if message is from a bot, else false
   */
  isBot(message) {
    if (message.author.bot) return true;
    return false;
  }

  /**
   * Reply to a message
   * @param {Object} message - Message object from listener
   * @param {String} content - The text content of the reply
   * @param {Boolean} [tts=false] - Switch for saying something in tts
   * @returns {Promise} Promise represents a new message object
   */
  reply(message, content, tts = false) {
    return message.reply(content, { tts });
  }

  /**
   * Check if a message content includes a string
   * @param {Object} message - Message object from listener
   * @param {Array} strings - Array of strings
   * @returns {Boolean} True of any of the strings are included in the message
   */
  includes(message, strings) {
    let {content} = message
    content = content.toLowerCase();

    let check = false;

    strings.forEach((string) => {
      if (content.includes(string.toLowerCase())) {
        check = true;
        //eslint-disable-next-line no-useless-return
        return;
      }
    });

    return check;
  }

  /**
   * Sends a new text message to the same channel the message originated from
   * @param {Object} message - Message object from listener
   * @param {String} content - The text content of the new message
   * @returns {Promise} Promise represents a new message object
   */
  send(message, content) {
    return message.channel.send(content)
  }

  /**
   * Sends a new message to the same channelthe message originated from.
   * This message only includes an attachment
   * @param {Object} message - Message object from listener
   * @param {Object} attachment - the attachment object to send
   * @param {String|Buffer} attachment.file - The file content
   * @param {String} attachment.name - The filename
   * @returns {Promise} Promise represents a new message object
   */
  sendAttachment(message, attachment) {
    return message.channel.send({
      files: [
        {
          attachment: attachment.file,
          name: attachment.name
        }
      ]
    });
  }

  /**
   * Searches the start of the message content a specific string
   * @param {Object} message - Message object from listener
   * @param {String} string - The searchstring
   * @returns {Boolean} True if the message starts with the provided string
   */
  startsWith(message, string) {
    let {content} = message;

    if (content.startsWith(string)) return true;
    return false;
  }

  /**
   * Creates an eventlistener for the ready event.
   * @param {Function} callback - A callback function which runs when the event is emitted.
   * @returns {Function} - The eventlistener for that event.
   */
  ready(callback) {
    return this.client.on('ready', () => {
      callback();
    });
  }

  /**
   * Creates an eventlistener for the message event.
   * @param {Function} callback - A callback function which runs when the event is emitted.
   * @returns {Function} - The eventlistener for that event.
   */
  message(callback) {
    return this.client.on('message', (message) => {
      callback(message);
    });
  }
}

module.exports = DiscordHelper;