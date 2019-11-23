'use strict'
const imageSchema = require('./imageSchema');

/**
 * Creates an Image
 * @class
 */
class Image {
  /**
   * Sets the object values to a schema object
   * @constructor
   * @param {Object} responseObject - The object received from the request response
   * @example
   * const Image = require('image');
   * const image = new Image({ fileName: '1234.jpg', url: 'http://example.com/1234.jpg' });
   */
  constructor(responseObject) {
    this.schema = imageSchema;
    Object.keys(this.schema).forEach((key) => {
      this.schema[key] = responseObject[key];
    });
  }
}

module.exports = Image;