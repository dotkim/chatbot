'use strict'

/**
 * An Image
 * @typedef {Object} ImageSchema
 * @property {string} fileName - The name of the image
 * @property {string} url - The URL to the image
 * @exports ImageSchema
 */
const schema = {
  fileName: String,
  url: String
};

module.exports = schema;