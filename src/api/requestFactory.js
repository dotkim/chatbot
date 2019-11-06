'use strict'
const config = require('../config/configuration');

/**
 * Factory for a new request
 * @class
 */
class RequestFactory {
  /**
   * Request constructor
   * @constructor
   * @param {String} route - The route for the api request
   * @param {String|Array<String>} parameters - Array with parameters and names in the correct order
   * @param {String} [method='GET'] - Request method
   */
  constructor(route, parameters, method = 'GET') {
    this.route = route;
    this.parameters = parameters;
    this.method = method;
  }

  /**
   * Build a new URL for the request
   * @returns {Object} Object represents a new URL
   */
  get() {
    if (!config.apiUrl) {
      throw new Error('config.apiUrl is undefined, there is nowhere to send this request');
    }

    if (!this.parameters) {
      throw new Error('Missing keyword in the request');
    }

    if (this.method != 'GET' && this.method != 'AUTHGET') {
      throw new Error('You cannot GET a non-get request, check method and try again');
    }

    let query = '';
    this.parameters.forEach((key) => {
      query += '/' + key;
    });

    const url = `${config.apiUrl}/${this.route}${query}`
    const auth = new Buffer.from(config.apiAuth).toString('base64');
    const urlObj = new URL(url);

    let options = {};

    //build options object
    options.host = urlObj.host;
    options.method = this.method;
    options.path = urlObj.pathname;

    //options.port
    if (urlObj.port != '') options.port = config.apiPort;

    //options.headers
    options.headers = {
      'Authorization': 'Basic ' + auth
    };

    return {
      options,
      urlObj
    };
  }

  /**
   * Build a option object for a post request
   * @param {Number} bodyLength - The lenght of the body object sent to the api
   * @returns {Object} Object represents options for use in a request
   */
  post(bodyLength) {
    if (!bodyLength) return new Error('bodyLength parameter is missing');
    if (typeof bodyLength != 'number') return new Error('bodyLength parameter must be a Number');

    let query = '';
    this.parameters.forEach((key) => {
      query += '/' + key;
    });

    const url = `${config.apiUrl}/${this.route}${query}`
    const auth = new Buffer.from(config.apiAuth).toString('base64');
    const urlObj = new URL(url);

    let options = {};

    //build options object
    options.host = urlObj.host;
    options.method = this.method;
    options.path = urlObj.pathname;

    //options.port
    if (urlObj.port != '') options.port = config.apiPort;

    //options.headers
    options.headers = {
      'Content-Type': 'application/json',
      'Content-Length': bodyLength,
      'Authorization': 'Basic ' + auth
    };

    return {
      options,
      urlObj
    };
  }
}

module.exports = RequestFactory;