'use strict'
const keywordSchema = require('../models/keywordSchema');

/**
 * Creates a Keyword
 * @class
 */
class Keyword {
  /**
   * Sets the object values to a schema object
   * @constructor
   * @param {Object} responseObject - The object received from the request response
   * @example
   * const Keyword = require('keyword');
   * const keyword = new Keyword({ values: ['I am very angry!!'], keyword: 'angry' });
   */
  constructor(responseObject) {
    this.schema = keywordSchema;
    Object.keys(this.schema).forEach((key) => {
      this.schema[key] = responseObject[key];
    });
  }
}

module.exports = Keyword;
