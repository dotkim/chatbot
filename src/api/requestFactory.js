'use strict'
const config = require('../config/configuration');

/**
 * module for creating a new request
 * @class
 */
class RequestFactory {
  /**
   * request constructor
   * @constructor
   * @param {String} route - the route for the api request
   * @param {Object} parameters - object with parameters and names
   * @param {String} [method='GET'] - request method
   * 
   * returns an instance of a request options
   */
  constructor(route, parameters, method='GET') {
    this.route = route;
    this.parameters = parameters;
    this.method = method;
  }

  /**
   * build a new URL for the request
   * 
   * returns an instance of URL
   */
  get() {
    let query = '';

    Object.keys(this.parameters).forEach((key) => {
      query += '/' + this.parameters[key];
    });

    const url = `${config.apiUrl}/${this.route}${query}`
    return new URL(url);
  }

  /**
   * build a option object for a post request
   * @param {Number} bodyLength - the lenght of the body object sent to the api
   * 
   * returns a 'options' object for the request
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
    options.host   = url.host;
    options.method = this.method;
    options.path   = url.pathname;

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