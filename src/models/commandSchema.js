'use strict'

/**
 * A Command
 * @typedef {Object} CommandSchema
 * @property {String} name - The name of the command
 * @property {String} prefix - The prefix of the command, if exists.
 * @exports CommandSchema
 */
const schema = {
  name: String,
  prefix: String
};

module.exports = schema;