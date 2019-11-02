'use strict'
const config = require('../config/configuration');

/**
 * module for creating a new request
 * @module request
 */
module.exports = class {
  /**
   * request constructor
   * @constructor
   * @param {Object} parameters - object with parameters and names
   * @param {String} method - request method
   * 
   * returns an instance of Url for GET or an option object for POST
   */
  constructor(parameters, method='GET') {
    this.parameters = parameters;
    this.method = method;

    this.lib = config.apiUrl
      .startsWith('https') ? require('https') : require('http');

    if (method == 'GET') return;
    else if (method == 'POST') return;
  }

  get() {
    //TODO: Find out where to correctly send the request
    //should the request be sent somewhere else?
  }
}