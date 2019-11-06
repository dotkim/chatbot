'use strict'

/**
 * A Keyword
 * @typedef {Object} KeywordSchema
 * @property {string} keyword - The keyword
 * @property {string} artist - All values for that keyword
 * @exports KeywordSchema
 */
const schema = {
  keyword: String,
  messages: Array
};

module.exports = schema;