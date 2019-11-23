'use strict'
const commandSchema = require('./commandSchema');

class Command {
  /**
   * Sets the object values to a schema object
   * @constructor
   * @param {Object} commandData - The name and optional prefix
   * @example
   * const Command = require('command');
   * const command = new Command({ name: 'yell', prefix: '!' });
   */
  constructor(commandData) {
    this.schema = commandSchema;
    Object.keys(this.schema).forEach((key) => {
      this.schema[key] = commandData[key];
    });
  }
}

module.exports = Command;