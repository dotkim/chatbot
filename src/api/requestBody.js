'use strict'

/**
 * Create a request body for the POST request
 * @param {Object} body - The body object to stringify (must be valid JSON)
 * 
 * Returns a JSON string
 */
const RequestBody = function RequestBody(body) {
  return JSON.stringify(body);
}

module.exports = RequestBody;