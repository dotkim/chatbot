'use strict'
const Request = require('../api/request');

/**
 * Class for getting and inserting images
 * @class
 */
class Image {
  /**
   * @constructor
   */
  constructor() {
    this.route = 'images';
  }

  insert(param, image) {
    let req = new Request(this.route, param, { image });
    return req.post();
  }

  get(param='getRandom') {
    let req = new Request(this.route, param);
    return req.get();
  }
}

module.exports = Image;