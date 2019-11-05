'use strict'
const Request = require('../api/request');

/**
 * Class for getting and inserting images
 * @class
 */
class Image {
  /**
   * @constructor
   * @param {String} [route='images'] - API route
   */
  constructor(route = 'images') {
    this.route = route;
  }

  /**
   * Insert an image to the API
   * @param {String|Array<String>} param - Path parameters
   * @param {Object} image - image object with name and data
   * @returns {Promise<Object>} Promise object represents a new or updated image object
   */
  insert(param, image) {
    let req = new Request('insert', param, { image });
    return req.post();
  }

  /**
   * Get a random or a specific image from the API
   * @param {String|Array<String>} param - Path parameters
   * @returns {Promise<Object>} Promise object represents an image object
   */
  get(param='getRandom') {
    let req = new Request(this.route, param);
    return req.get();
  }
}

module.exports = Image;