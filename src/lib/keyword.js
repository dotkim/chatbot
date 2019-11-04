'use strict'
const Request = require('../api/request');

/**
 * Class for getting and adding keywords
 * @class
 */
class Keyword {
  /**
   * @constructor
   */
  constructor() {
    this.route = 'keyword'
  }

  /**
   * Add a new or update an existing keyword
   * @param {String} keyword - The new or existing keyword
   * @param {String} message - The message to add
   * @returns {Promise} Promise object represents a new or updated keyword object
   */
  add(keyword, message) {
    let req = new Request(this.route, keyword, { message });
    return req.post();
  }

  /**
   * Get a specific keyword from the API
   * @param {String} keyword - The keyword to get from the API
   * @returns {Promise} Promise object represents a keyword object
   */
  get(keyword) {
    let req = new Request(this.route, keyword);
    return req.get()
  }
}

module.exports = Keyword;