'use strict'

/**
 * A Keyword
 * @typedef {Object} KeywordSchema
 * @property {string} keyword - The keyword
 * @property {string} values - All values for that keyword
 * @exports KeywordSchema
 */
const schema = {
  keyword: String,
  values: Array
};

module.exports = schema;