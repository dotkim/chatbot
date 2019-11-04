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
   * @param {Object} parameters - Object with parameters and names
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
    let query = '';

    Object.keys(this.parameters).forEach((key) => {
      query += '/' + this.parameters[key];
    });

    const url = `${config.apiUrl}/${this.route}${query}`

    const urlObj = new URL(url);
    
    url.startsWith('https')
      ? urlObj.protocol = 'https'
      : urlObj.protocol = 'http';

    return urlObj;
  }

  /**
   * Build a option object for a post request
   * @param {Number} bodyLength - The lenght of the body object sent to the api
   * @returns {Object} Object represents options for use in a request
   */
  post(bodyLength) {
    if (!bodyLength) return new Error('bodyLength parameter is missing');
    if (typeof bodyLength != 'number') return new Error('bodyLength parameter must be a Number');

    const querystring = require('querystring');

    const query = querystring.stringify(this.parameters);
    const url = `${config.apiUrl}/${this.route}?${query}`
    const auth = new Buffer.from(config.apiAuth).toString('base64');

    let options = {};

    //build options object
    options.host = url.host;
    options.method = this.method;
    options.path = url.pathname;

    //options.protocol
    url.startsWith('https')
      ? options.protocol = 'https'
      : options.protocol = 'http';

    //options.port
    if (url.port != '') options.port = url.port;

    //options.headers
    options.headers = {
      'Content-Type': 'application/json',
      'Content-Length': bodyLength,
      'Authorization': 'Basic ' + auth
    }
  }
}

module.exports = RequestFactory;