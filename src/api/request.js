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
  constructor(route, parameters, body = undefined) {
    if (typeof param == 'string') this.parameters = [parameters];
    else this.parameters = parameters;

    if (body) {
      this.body = new requestBody(body).json();
      this.factory = new RequestFactory(route, this.parameters, 'POST');
      this.options = this.factory.post(body.length);
    } else {
      this.factory = new RequestFactory(route, this.parameters);
      this.url = this.factory.get();
    }
  }

  /**
   * Send a GET request to the received URL
   * @returns {Promise<Object>} Promise represents received API data
   */
  get() {
    return new Promise((resolve, reject) => {
      const lib = this.url.protocol == 'https'
        ? require('https')
        : require('http');

      const request = lib.get(this.url, (response) => {
        if (response.statusCode < 200 || response.statusCode > 299) {
          reject(new Error('Failed to load page, status code: ' + response.statusCode));
        }
        const body = [];
        response.on('data', (chunk) => body.push(chunk));
        response.on('end', () => resolve(JSON.parse(body.join(''))));
      });

      request.on('error', (err) => reject(err))
    });
  }

  /**
   * Send a POST request to the received URL
   * @returns {Promise<Object>} Promise represents received API data
   */
  post() {
    return new Promise((resolve, reject) => {
      const lib = this.options.protocol == 'https'
        ? require('https')
        : require('http');

      const request = lib.request(this.options, (response) => {

        if (response.statusCode < 200 || response.statusCode > 299) {
          reject(new Error('Failed to load page, status code: ' + response.statusCode));
        }

        const returnBody = [];

        response.on('data', (chunk) => returnBody.push(chunk));
        response.on('end', () => resolve(JSON.parse(returnBody.join(''))));
      });

      request.on('error', (err) => reject(err))

      request.write(this.body);
      request.end();
    });
  }
}

module.exports = Request;