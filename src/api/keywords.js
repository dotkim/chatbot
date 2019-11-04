'use strict'
const config = require('../config/configuration');

/**
 * Class for getting and adding keywords
 * @class
 */
class Keywords {
  /**
   * get a list of messages related to the keyword provided
   * @param {String} keyword - the keyword to fetch
   * 
   * return an array of messages
   */
  get(keyword) {
    return new Promise((resolve, reject) => {

      const url = config.apiUrl + '/keyword/' + keyword;
      
      const lib = url.startsWith('https') ? require('https') : require('http');

      const request = lib.get(url, (response) => {
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
   * add a new or update an existsing keyword
   * @param {String} keyword - The keyword to add/update
   * @param {String} message - The new message
   * 
   * returns an object with info about
   * the new keyword object
   */
  add(keyword, message) {
    return new Promise((resolve, reject) => {

      const url = config.apiUrl + '/keyword/' + keyword;
      
      const lib = url.startsWith('https') ? require('https') : require('http');
      const options = {
        hostname: config.apiUrl,
        method: 'POST',
        port: config.apiPort,
        path: `/keyword/?word=${keyword}&message=${message}`
      };

      const request = lib.request(options, (response) => {
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
}

module.exports = Keywords;