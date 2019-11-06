'use strict'
const Request = require('../api/request');
const keywordSchema = require('../models/keywordSchema');

/**
 * Class for building keyword objects
 * @class
 */
class KeywordFactory {
  /**
   * @constructor
   */
  constructor() {
    this.route = 'keyword'
  }

  /**
   * Add a new or update an existing keyword
   * @param {String|Array<String>} keyword - The new or existing keyword
   * @param {String} message - The message to add
   * @returns {Promise<Object>} Promise object represents a new or updated keyword object
   * @example
   * const KeywordFactory = require('lib/keywordFactory');
   * const factory = new KeywordFactory();
   * (async function() {
   *   let result = await factory.add('angry', 'I am very angry!!'); // returns added keyword object
   * })();
   */
  add(keyword, message) {
    let req = new Request(this.route, keyword, { message });
    return req.post();
  }

  /**
   * Get a specific keyword from the API
   * @param {String} keyword - The keyword to get from the API
   * @returns {Promise<Object>} Promise object represents a keyword object
   */
  get(keyword) {
    let req = new Request(this.route, keyword);
    return req.get()
  }
}

module.exports = KeywordFactory;