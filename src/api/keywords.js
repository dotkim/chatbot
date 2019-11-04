'use strict'
const Request = require('./request');
const route = 'keywords'

/**
 * Class for getting and adding keywords
 * @class
 */
class Keywords {
  /**
   * Get a specific keyword from the API
   * @param {String} keyword - The keyword to get from the API
   * 
   * Returns a promise with the keyword object
   */
  get(keyword) {
    let req = new Request(route, keyword);
    return req.get()
  }

  /**
   * Add a new or update an existing keyword
   * @param {String} keyword - The new or existing keyword
   * @param {String} message - The message to add
   * 
   * Returns a promise with the new keyword object
   */
  add(keyword, message) {
    let req = new Request(route, keyword, {message});
    return req.post();
  }
}

module.exports = Keywords;