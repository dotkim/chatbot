'use strict'
const config = require('../config/configuration');

/**
 * Factory for a new request.
 * @class
 */
class RequestFactory {
  /**
   * Request constructor.
   * @constructor
   * @param {String} route - The route for the api request.
   * @param {String|Array<String>} parameters - Array with parameters and names in the correct order.
   * @param {String} [method='GET'] - Request method.
   * @example
   * const RequestFactory = require('./requestFactory');
   * const factory = new RequestFactory(route, parameters, method);
   */
  constructor(route, parameters, method = 'GET') {
    this.route = route;
    this.parameters = parameters;
    this.method = method;
  }

  /**
   * Build a new URL for the request.
   * @returns {Object} Object represents a new URL.
   * @example const options = factory.get();
   */
  get() {
    if (!config.apiUrl) {
      throw new Error('config.apiUrl is undefined, there is nowhere to send this request');
    }
    if (!this.parameters) {
      throw new Error('Missing parameters in the request');
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
    options.host = urlObj.host;
    options.method = 'GET';
    options.path = urlObj.pathname;
    options.port = config.apiPort;
    options.headers = {
      'Authorization': 'Basic ' + auth
    };

    return {
      options,
      urlObj
    };
  }

  /**
   * Build a option object for a post request.
   * @param {Number} bodyLength - The lenght of the body object sent to the api.
   * @returns {Object} Object represents options for use in a request.
   * @example const options = factory.post(this.body.length);
   */
  post(bodyLength) {
    if (!bodyLength) return new Error('bodyLength parameter is missing');
    if (typeof bodyLength != 'number') return new Error('bodyLength parameter must be a Number');

    if (!config.apiUrl) {
      throw new Error('config.apiUrl is undefined, there is nowhere to send this request');
    }
    if (!this.parameters) {
      throw new Error('Missing parameters in the request');
    }
    if (this.method != 'POST') {
      throw new Error('You cannot POST a non-post request, check method and try again');
    }

    let query = '';
    this.parameters.forEach((key) => {
      query += '/' + key;
    });

    const url = `${config.apiUrl}/${this.route}${query}`
    const auth = new Buffer.from(config.apiAuth).toString('base64');
    const urlObj = new URL(url);

    let options = {};

    options.host = urlObj.host;
    options.method = 'POST';
    options.path = urlObj.pathname;
    options.port = config.apiPort;
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