'use strict'

const RequestFactory = require('./requestFactory');
const requestBody = require('./requestBody');

/**
 * Request module for getting and posting
 * @class
 */
class Request {
  /**
   * Create a new instance of Request, 
   * @param {String} route - API route
   * @param {String|Array<String>} parameters - Path parameters
   * @param {Object} [body=undefined] - Optional body object to post
   * @constructor
   */
  //eslint-disable-next-line no-undefined
  constructor(route, options) {
    if (typeof options.parameters == 'string') this.parameters = [options.parameters];
    else this.parameters = options.parameters;

    if (options.body) {
      this.method = options.method;
      this.body = new requestBody(options.body).json();
      this.factory = new RequestFactory(route, this.parameters, options.method);
      this.options = this.factory.post(this.body.length);
    } else {
      this.method = options.method;
      this.factory = new RequestFactory(route, this.parameters, options.method);
      this.options = this.factory.get();
    }
  }

  /**
   * Send the request to the API.
   * @returns {Promise<Object>} Promise represents received API data
   */
  send() {
    return new Promise((resolve, reject) => {
      const lib = this.options.urlObj.protocol == 'https:'
        ? require('https')
        : require('http');

      const request = lib.request(this.options.urlObj, this.options.options, (response) => {

        if (response.statusCode < 200 || response.statusCode > 299) {
          reject(new Error('Failed to load page, status code: ' + response.statusCode));
        }

        const returnBody = [];

        response.on('data', (chunk) => returnBody.push(chunk));
        response.on('end', () => resolve(JSON.parse(returnBody.join(''))));
      });

      request.on('error', (err) => reject(new Error(err)));

      if (this.body) {
        request.write(this.body);
      }

      request.end();
    });
  }
}

module.exports = Request;