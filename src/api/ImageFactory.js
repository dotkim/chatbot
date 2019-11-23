'use strict'
const Request = require('../models/Request');
const Image = require('../models/Image');

/**
 * Class for building image objects.
 * @class
 */
class ImageFactory {
  /**
   * @constructor
   * @param {String} [route='images'] - API route.
   * @example
   * const ImageFactory = require('./imageFactory');
   * const factory = new ImageFactory();
   */
  constructor(route = 'images') {
    this.route = route;
  }

  /**
   * Insert an image to the API.
   * @param {String|Array<String>} param - Path parameters.
   * @param {Object} image - Image object with name and data.
   * @returns {Promise<Image>} Promise object represents a new or updated image object.
   */
  async insert(param, image) {
    this.options = {};
    this.options.parameters = param;
    this.options.body = { image };
    this.options.method = 'POST';

    let req = new Request('insert', param, { image });
    let res = await req.send();

    return new Image(res);
  }

  /**
   * Get a random or a specific image from the API.
   * @param {String} param - Path parameters.
   * @returns {Promise<Image>} Promise object represents an image object.
   * @example
   * (async function() {
   *   let result = await factory.get();
   * })();
   */
  async get(param='getRandom') {
    this.options = {};
    this.options.parameters = param;

    let req = new Request(this.route, this.options);
    let res = await req.send();

    return new Image(res);
  }
}

module.exports = ImageFactory;