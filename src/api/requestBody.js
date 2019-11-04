'use strict'

/**
 * Class for creating a request body
 * @class
 */
class RequestBody {
  /**
   * Create a request body for the POST request
   * @param {Object} body - The body object to stringify (must be valid JSON)
   * @constructor
   */
  constructor(body) {
    this.body = body;
  }

  /**
   * Creates the JSON string and returns it
   */
  json() {
    return JSON.stringify(this.body);
  }
}

module.exports = RequestBody;