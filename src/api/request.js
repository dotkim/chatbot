'use strict'

const RequestFactory = require('./requestFactory');
const requestBody = require('./requestBody');

/**
 * Request module for getting and posting
 * @class
 */
class Request {
  //eslint-disable-next-line no-undefined
  constructor(route, param, body = undefined) {
    if (body) {
      this.body = requestBody(body);
      this.factory = new RequestFactory(route, param, 'POST');
      this.options = this.factory.post(body.length);
    } else {
      this.factory = new RequestFactory(route, param);
      this.url = this.factory.get();
    }
  }

  /**
   * Send a GET request to the received URL
   * Returns a promise with data
   */
  get() {
    return new Promise((resolve, reject) => {
      const lib = this.options.protocol == 'https'
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
   * Returns a promise with data
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