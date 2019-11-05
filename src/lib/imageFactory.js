'use strict'
const Request = require('../api/request');
const imageSchema = require('../models/imageSchema');

/**
 * Class for building image objects
 * @class
 */
class ImageFactory {
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
    return req.send();
  }

  /**
   * Get a random or a specific image from the API
   * @param {String|Array<String>} param - Path parameters
   * @returns {Promise<Image>} Promise object represents an image object
   */
  async get(param='getRandom') {
    let req = new Request(this.route, param);
    let keys = Object.keys(imageSchema);
    let data = await req.send();

    let returnObject = {};
    
    keys.forEach((key) => {
      returnObject[key] = data[key];
    });

    return returnObject;
  }
}

module.exports = ImageFactory;