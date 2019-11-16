'use strict'

const RequestFactory = require('../api/RequestFactory');
const RequestBody = require('../components/RequestBody');

/**
 * Request module for getting and posting.
 * @class
 */
class Request {
  /**
   * Create a new instance of Request.
   * @param {String} route - The desired API route.
   * @param {Object} options - An object containing the nessecary keys and values for the request.
   * @param {String} options.parameters - The request parameters, this is in PATH params for the API.
   * @param {String} [options.method] - The request method.
   * @param {Object} [options.body] - The request body, if omited the request will not POST.
   * @constructor
   * @example
   * const Request = require('../api/request');
   * let req = new Request(this.route, { parameters: 'happy', method: 'AUTHGET' });
   */
  //eslint-disable-next-line no-undefined
  constructor(route, options) {
    if (typeof options.parameters == 'string') this.parameters = [options.parameters];
    else this.parameters = options.parameters;

    if (options.body) {
      this.method = options.method;
      this.body = new RequestBody(options.body).json();
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
   * @returns {Promise<Object>} Promise represents received API data.
   * @example
   * (async function() {
   *   let res = await req.send();
   * })();
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