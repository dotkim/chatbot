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
   * Creates the JSON string
   * @returns {String} String object represents a stringified JSON object.
   */
  json() {
    return JSON.stringify(this.body);
  }
}

module.exports = RequestBody;