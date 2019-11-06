'use strict'
const Request = require('../api/request');
const Keyword = require('./keyword');

/**
 * Class for building keyword objects
 * @class
 */
class KeywordFactory {
  /**
   * @constructor
   * @param {String} [route='keyword'] - The API route
   * @example
   * const KeywordFactory = require('lib/keywordFactory');
   * const factory = new KeywordFactory();
   */
  constructor(route='keyword') {
    this.route = route;
  }

  /**
   * Add a new or update an existing keyword
   * @param {String|String[]} keyword - The new or existing keyword
   * @param {String} message - The message to add
   * @returns {Promise<Keyword>} Promise object represents a new or updated keyword object
   * @example
   * (async function() {
   *   let result = await factory.add('angry', 'I am very angry!!'); // returns added keyword object
   * })();
   */
  async add(keyword, message) {
    let options = {
      parameters: keyword,
      method: 'POST',
      body: { message }
    };

    let req = new Request(this.route, options);
    let res = await req.send();

    return new Keyword(res);
  }

  /**
   * Get a specific keyword from the API
   * @param {String} keyword - The keyword to get from the API
   * @returns {Promise<Keyword>} Promise object represents a keyword object
   * @example
   * (async function() {
   *   let result = await factory.get('angry');
   * })();
   */
  async get(keyword) {
    let options = {
      parameters: keyword,
      method: 'AUTHGET'
    };

    let req = new Request(this.route, options);
    let res = await req.send();

    return new Keyword(res);
  }
}

module.exports = KeywordFactory;