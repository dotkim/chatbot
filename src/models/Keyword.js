'use strict'
const keywordSchema = require('./keywordSchema');

/**
 * Creates a Keyword
 * @class
 */
class Keyword {
  /**
   * Sets the object values to a schema object
   * @constructor
   * @param {Object} responseObject - The object received from the request response
   * @example
   * const Keyword = require('keyword');
   * const keyword = new Keyword({ values: ['I am very angry!!'], keyword: 'angry' });
   */
  constructor(responseObject) {
    this.schema = keywordSchema;
    Object.keys(this.schema).forEach((key) => {
      this.schema[key] = responseObject[key];
    });
  }

  Reply(message) {
    const GetRandom = require('../old/getRandomInt');
    let rand = GetRandom(this.schema.values.length);
    const reply = this.schema.values[rand];
    message.channel.send(reply);
  }
}

module.exports = Keyword;