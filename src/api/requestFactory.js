'use strict'
const config = require('../config/configuration');

/**
 * module for creating a new request
 * @module requestFactory
 */
module.exports = class {
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
